using GramophoneUtils.Characters;
using GramophoneUtils.Items.Hotbars;
using GramophoneUtils.Stats;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[Serializable, CreateAssetMenu(fileName = "New Skill", menuName = "Character Classes/Skills")]
public class Skill : Resource, ISkill, IHotbarItem
{
    #region Attributes/Fields/Properties

    [SerializeField] private CharacterClass[] _classRestrictions;
    public CharacterClass[] ClassRestrictions => _classRestrictions;

    [SerializeField] private School _school;
    public School School => _school;

    [SerializeField] private Skill[] _prerequisites;
    public ISkill[] Prerequisites => _prerequisites;

    [SerializeField] private int _usesToUnlock;
    public int UsesToUnlock => _usesToUnlock;

    [SerializeField] private TargetAreaFlag _targetAreaFlag;
    public TargetAreaFlag TargetAreaFlag => _targetAreaFlag;

    [SerializeField] private TargetNumberFlag _targetNumberFlag;
    public TargetNumberFlag TargetNumberFlag => _targetNumberFlag;

    [SerializeField] private TargetTypeFlag _targetTypeFlag = TargetTypeFlag.Alive;
    public TargetTypeFlag TargetTypeFlag => _targetTypeFlag;

    [SerializeField] private SkillAnimType _skillAnimType;
    public SkillAnimType SkillAnimType => _skillAnimType;

    [SerializeField] private GameObject _projectilePrefab;
    public GameObject ProjectilePrefab => _projectilePrefab;

    [SerializeField] private GameObject _effectPrefab;
    public GameObject EffectPrefab => _effectPrefab;

    [SerializeField] private RuntimeAnimatorController _animatorController;
    public RuntimeAnimatorController AnimatorController => _animatorController;

    [SerializeField] private List<StatModifierBlueprint> _statModifierBlueprints;
    [SerializeField] private List<DamageBlueprint> _damageBlueprints;
    [SerializeField] private List<HealingBlueprint> _healingBlueprints;

    public Action OnSkillUsed;
    
    private List<StatModifier> _skillStatModifiers { get { return InstanceSkillStatModifierBlueprints(); } }
    private List<Damage> _skillDamages { get { return InstanceSkillDamageBlueprints(); } }
    private List<Healing> _skillHealings { get { return InstanceSkillHealingBlueprints(); } }

    #endregion

    #region Constructors
    #endregion

    #region Callbacks
    #endregion

    #region Public Functions

    public void Use(List<Character> characterTargets, Character originator)
    {
        //Debug.LogWarning("Here is where we should instance the blueprints, instancing them with the originator. The target can then adapt them on reception.");

        originator.SkillSystem.OnSkillUsed?.Invoke(this, characterTargets);
    }

    public void DoNextBit(List<Character> characterTargets, Character originator)
    {
        foreach (Character character in characterTargets)
        {

            ApplyStatModifiers(character); // change these to 'sendModified' statModifier struct, to 'receiveModified' statModifier struct


            SendDamageStructs(character, originator);
            SendHealingStructs(character, originator);
        }
    }

    public bool CanStartUnlocking(ISkill skill, Character character)
    {
        if (character.CharacterClass.SkillsAvailable.Contains(skill))
        {
            if (_prerequisites.Intersect(character.SkillSystem.UnlockedSkills).Count() == _prerequisites.Count()) // if the intersection of the two lists is the same length, then the UnlockedSkills contains all the prerequiste skills
            {
                return true;
            }
        }
        return false;
    }

    public bool CanUnlock(ISkill skill, Character character)
    {
        if (CanStartUnlocking(skill, character))
        {
            if (skill.UsesToUnlock == 0)
            {
                return true;
            }
            else if (character.SkillSystem.LockedSkills.ContainsKey(skill))
            {
                if (character.SkillSystem.LockedSkills[skill] >= skill.UsesToUnlock)
                {
                    return true;
                }
            }
        }
        return false;
    }

#if UNITY_EDITOR

    [Button("Create Random Skill Object")]
    public void CreateRandomSkillObject()
    {
        RandomSkillObject randomSkillObject = CreateInstance(typeof(RandomSkillObject)) as RandomSkillObject;
        string assetPath = AssetDatabase.GenerateUniqueAssetPath("Assets/Resources/Skills/Random Skill Objects/" + this.name + "RandomSkillObject.asset");
        AssetDatabase.CreateAsset(randomSkillObject, assetPath);
        Skill skill = AssetDatabase.LoadAssetAtPath<Skill>("Assets/Resources/Skills/" + this.Name + ".asset");
        randomSkillObject.SetWeightedObject(skill, skill);
        randomSkillObject.Weighting = 1f;
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        EditorUtility.SetDirty(randomSkillObject);
    }

#endif

    #endregion

    #region Protected Functions
    #endregion

    #region Private Functions

    private List<StatModifier> InstanceSkillStatModifierBlueprints()
    {
        List<StatModifier> statModifiers = new List<StatModifier>();
        if (_statModifierBlueprints.Count > 0)
        {
            foreach (var blueprint in _statModifierBlueprints)
            {
                statModifiers.Add(blueprint.CreateBlueprintInstance<StatModifier>(this));
            }
        }
        return statModifiers;
    }

    private List<Damage> InstanceSkillDamageBlueprints()
    {
        List<Damage> damages = new List<Damage>();
        if (_damageBlueprints.Count > 0)
        {
            foreach (var blueprint in _damageBlueprints)
            {
                damages.Add(blueprint.CreateBlueprintInstance<Damage>(this));
            }
        }
        return damages;
    }

    private List<Healing> InstanceSkillHealingBlueprints()
    {
        List<Healing> healings = new List<Healing>();
        if (_healingBlueprints.Count > 0)
        {
            foreach (var blueprint in _healingBlueprints)
            {
                healings.Add(blueprint.CreateBlueprintInstance<Healing>(this));
            }
        }
        return healings;
    }

    private void ApplyStatModifiers(Character character)
    {
        foreach (StatModifier statModifier in _skillStatModifiers)
        {
            character.StatSystem.AddModifier(statModifier);
        }
    }

    private void SendDamageStructs(Character target, Character originator)
    {
        List<Damage> modifiedDamages = ModifyDamageStructs(originator);
        target.StatSystem.ReceiveModifiedDamageStructs(modifiedDamages, originator);
    }

    private void SendHealingStructs(Character target, Character originator)
    {
        List<Healing> modifiedHealings = ModifyHealingStructs(originator);
        target.StatSystem.ReceiveModifiedHealingStructs(modifiedHealings, originator);
    }

    private List<Damage> ModifyDamageStructs(Character originator)
    {
        List<Damage> modifiedDamages = new List<Damage>();
        foreach (Damage damage in _skillDamages)
        {
            modifiedDamages.Add(originator.StatSystem.ModifyOutgoingDamage(damage));
        }
        return modifiedDamages;
    }

    private List<Healing> ModifyHealingStructs(Character originator)
    {
        List<Healing> modifiedHealings = new List<Healing>();
        foreach (Healing healing in _skillHealings)
        {
            modifiedHealings.Add(originator.StatSystem.ModifyOutgoingHealing(healing));
        }
        return modifiedHealings;
    }

    #endregion

    #region Inner Classes
    #endregion

}
