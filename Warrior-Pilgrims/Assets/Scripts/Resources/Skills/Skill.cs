using AYellowpaper;
using GramophoneUtils.Characters;
using GramophoneUtils.Items.Hotbars;
using GramophoneUtils.Stats;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

[Serializable, CreateAssetMenu(fileName = "New Skill", menuName = "Skills")]
public class Skill : Resource, ISkill, IHotbarItem
{
    #region Attributes/Fields/Properties

    [BoxGroup("Skill Progression")]
    [SerializeField] private CharacterClass[] _classRestrictions;
    public CharacterClass[] ClassRestrictions => _classRestrictions;

    [BoxGroup("Skill Progression")]
    [SerializeField] private School _school;
    public School School => _school;

    [BoxGroup("Skill Progression")]
    [SerializeField] private Skill[] _prerequisites;
    public ISkill[] Prerequisites => _prerequisites;

    [BoxGroup("Skill Progression")]
    [SerializeField] private int _usesToUnlock;
    public int UsesToUnlock => _usesToUnlock;

    [BoxGroup("Positioning")]
    [BoxGroup("Positioning/Target")]
    [SerializeField] private TargetAreaFlag _targetAreaFlag;
    public TargetAreaFlag TargetAreaFlag => _targetAreaFlag;

    [BoxGroup("Positioning/Target")]
    [SerializeField] private TargetNumberFlag _targetNumberFlag;
    public TargetNumberFlag TargetNumberFlag => _targetNumberFlag;

    [BoxGroup("Positioning/Target")]
    [SerializeField] private TargetTypeFlag _targetTypeFlag = TargetTypeFlag.Alive;
    public TargetTypeFlag TargetTypeFlag => _targetTypeFlag;
    
    [BoxGroup("Positioning/Target")]
    [SerializeField] private SkillAnimType _skillAnimType;
    public SkillAnimType SkillAnimType => _skillAnimType;

    [BoxGroup("Positioning/Target")]
    [SerializeField] private InterfaceReference<ITargetToSlots> _targetToSlots;
    public ITargetToSlots TargetToSlots => _targetToSlots.Value;

    [BoxGroup("Positioning/Use From")]
    [SerializeField] private InterfaceReference<IUseFromSlot> _useFromSlot;
    public IUseFromSlot UseFromSlot => _useFromSlot.Value;

    [BoxGroup("Scene Objects")]
    [SerializeField] private GameObject _projectilePrefab;
    public GameObject ProjectilePrefab => _projectilePrefab;

    [BoxGroup("Scene Objects")]
    [SerializeField] private GameObject _effectPrefab;
    public GameObject EffectPrefab => _effectPrefab;

    [BoxGroup("Scene Objects")]
    [SerializeField] private RuntimeAnimatorController _animatorController;
    public RuntimeAnimatorController AnimatorController => _animatorController;

    [BoxGroup("Effect Blueprints")]
    [SerializeField] private List<InterfaceReference<IStatusEffectBlueprint>> _statusEffectBlueprints;

    [BoxGroup("Effect Blueprints")]
    [SerializeField] private List<StatModifierBlueprint> _statModifierBlueprints;

    [BoxGroup("Effect Blueprints")]
    [SerializeField] private List<DamageBlueprint> _damageBlueprints;

    [BoxGroup("Effect Blueprints")]
    [SerializeField] private List<HealingBlueprint> _healingBlueprints;

    [BoxGroup("Effect Blueprints")]
    [SerializeField] private List<MoveBlueprint> _moveBlueprints;

    public Action OnSkillUsed;
    
    private List<IStatModifier> _skillStatModifiers { get { return InstanceSkillStatModifierBlueprints(); } }
    private List<Damage> _skillDamages { get { return InstanceSkillDamageBlueprints(); } }
    private List<Healing> _skillHealings { get { return InstanceSkillHealingBlueprints(); } }

    private List<Move> _skillMoves { get { return InstanceSkillMovesBlueprints(); } }

    #endregion

    #region Constructors
    #endregion

    #region Callbacks
    #endregion

    #region Public Functions

    public virtual void Use(List<Character> characterTargets, Character originator)
    {
        //Debug.LogWarning("Here is where we should instance the blueprints, instancing them with the originator. The target can then adapt them on reception.");

        originator.SkillSystem.OnSkillUsed?.Invoke(this, characterTargets);
    }

    public virtual void DoNextBit(List<Character> characterTargets, Character originator)
    {
        foreach (Character character in characterTargets)
        {
            
            //ApplyStatModifiers(character); // change these to 'sendModified' statModifier struct, to 'receiveModified' statModifier struct
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(); // TODO refactoring hack
            ApplyStatusEffects(character, originator, cancellationTokenSource);
            SendDamageStructs(character, originator, cancellationTokenSource);
            SendHealingStructs(character, originator, cancellationTokenSource);
            SendMoveStructs(character, originator, cancellationTokenSource);
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

    public Task Apply(Character target, Character originator, CancellationTokenSource tokenSource)
    {
        throw new NotImplementedException();
    }

    public Task Remove(Character target, Character originator, CancellationTokenSource tokenSource)
    {
        throw new NotImplementedException();
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

    private List<IStatModifier> InstanceSkillStatModifierBlueprints()
    {
        List<IStatModifier> statModifiers = new List<IStatModifier>();
        if (_statModifierBlueprints.Count > 0)
        {
            foreach (var blueprint in _statModifierBlueprints)
            {
                statModifiers.Add(ServiceLocatorObject.Instance.StatModifierFactory.CreateStatModifierFromBlueprint(blueprint, new object[] { this }));
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

    private List<Move> InstanceSkillMovesBlueprints()
    {
        List<Move> moves = new List<Move>();
        if (_moveBlueprints.Count > 0)
        {
            foreach (var blueprint in _moveBlueprints)
            {
                moves.Add(blueprint.CreateBlueprintInstance<Move>(this));
            }
        }
        return moves;
    }

    private void ApplyStatusEffects(Character character, Character originator, CancellationTokenSource cancellationTokenSource)
    {
        foreach (InterfaceReference<IStatusEffectBlueprint> statusEffectBlueprint in _statusEffectBlueprints)
        {
            IStatusEffect statusEffect = ServiceLocatorObject.Instance.StatusEffectFactory.CreateStatusEffectFromBlueprint(statusEffectBlueprint.Value, new object[] { this, originator });
            statusEffect.Apply(character, originator, cancellationTokenSource);
        }
    }

    private void SendDamageStructs(Character target, Character originator, CancellationTokenSource tokenSource)
    {
        List<Damage> modifiedDamages = ModifyDamageStructs(originator);
        target.StatSystem.ReceiveModifiedDamageStructs(modifiedDamages, originator, tokenSource);
    }

    private void SendHealingStructs(Character target, Character originator, CancellationTokenSource tokenSource)
    {
        List<Healing> modifiedHealings = ModifyHealingStructs(originator);
        target.StatSystem.ReceiveModifiedHealingStructs(modifiedHealings, originator, tokenSource);
    }

    private void SendMoveStructs(Character target, Character originator, CancellationTokenSource tokenSource)
    {
        List<Move> modifiedStructs = ModifyMoveStructs(originator);
        target.StatSystem.ReceiveModifiedMoveStructs(modifiedStructs, originator, tokenSource);
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

    private List<Move> ModifyMoveStructs(Character originator)
    {
        List<Move> modifiedMoves = new List<Move>();
        foreach (Move move in _skillMoves)
        {
            modifiedMoves.Add(originator.StatSystem.ModifyOutgoingMove(move));
        }
        return modifiedMoves;
    }

    #endregion

    #region Inner Classes
    #endregion

}
