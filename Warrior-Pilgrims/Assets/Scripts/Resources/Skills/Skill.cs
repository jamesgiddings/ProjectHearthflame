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

[Serializable, CreateAssetMenu(fileName = "New Skill", menuName = "Skills/New Skill")]
public class Skill : Resource, ISkill, IHotbarItem
{
    #region Attributes/Fields/Properties

    [BoxGroup("General")]
    [SerializeField] private string _description;
    public string Description => _description;

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


    [BoxGroup("Target Effect Blueprints")]
    [SerializeField] private List<InterfaceReference<IStatusEffectBlueprint>> _targetStatusEffectBlueprints;

    [BoxGroup("Target Effect Blueprints")]
    [SerializeField] private List<DamageBlueprint> _targetDamageBlueprints;

    [BoxGroup("Target Effect Blueprints")]
    [SerializeField] private List<HealingBlueprint> _targetHealingBlueprints;

    [BoxGroup("Target Effect Blueprints")]
    [SerializeField] private List<MoveBlueprint> _targetMoveBlueprints;


    [BoxGroup("Self Effect Blueprints")]
    [SerializeField] private List<InterfaceReference<IStatusEffectBlueprint>> _selfStatusEffectBlueprints;

    [BoxGroup("Self Effect Blueprints")]
    [SerializeField] private List<DamageBlueprint> _selfDamageBlueprints;

    [BoxGroup("Self Effect Blueprints")]
    [SerializeField] private List<HealingBlueprint> _selfHealingBlueprints;

    [BoxGroup("Self Effect Blueprints")]
    [SerializeField] private List<MoveBlueprint> _selfMoveBlueprints;


    public Action OnSkillUsed;

    private List<Damage> _targetSkillDamages { get { return InstanceSkillDamageBlueprints(_targetDamageBlueprints); } }
    private List<Healing> _targetSkillHealings { get { return InstanceSkillHealingBlueprints(_targetHealingBlueprints); } }
    private List<Move> _targetSkillMoves { get { return InstanceSkillMovesBlueprints(_targetMoveBlueprints); } }

    private List<Damage> _selfSkillDamages { get { return InstanceSkillDamageBlueprints(_selfDamageBlueprints); } }
    private List<Healing> _selfSkillHealings { get { return InstanceSkillHealingBlueprints(_selfHealingBlueprints); } }
    private List<Move> _selfSkillMoves { get { return InstanceSkillMovesBlueprints(_selfMoveBlueprints); } }

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
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(); // TODO we currently don't do anything with this token, it should have some logic to cancel an ability if damage doesn't land, for example

            ApplyTargetStatusEffects(character, originator, cancellationTokenSource);
            SendTargetDamageStructs(character, originator, cancellationTokenSource);
            SendTargetHealingStructs(character, originator, cancellationTokenSource);
            SendTargetMoveStructs(character, originator, cancellationTokenSource);

            ApplySelfStatusEffects(originator, cancellationTokenSource);
            SendSelfTargetDamageStructs(originator, cancellationTokenSource);
            SendSelfTargetHealingStructs(originator, cancellationTokenSource);
            SendSelfTargetMoveStructs(originator, cancellationTokenSource);
        }
    }

    public override string GetInfoDisplayText()
    {
        return "";
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

    private List<Damage> InstanceSkillDamageBlueprints(List<DamageBlueprint> _damageBlueprints)
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

    private List<Healing> InstanceSkillHealingBlueprints(List<HealingBlueprint> _healingBlueprints)
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

    private List<Move> InstanceSkillMovesBlueprints(List<MoveBlueprint> _moveBlueprints)
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

    private void ApplyTargetStatusEffects(Character character, Character originator, CancellationTokenSource tokenSource)
    {
        foreach (InterfaceReference<IStatusEffectBlueprint> statusEffectBlueprint in _targetStatusEffectBlueprints)
        {
            IStatusEffect statusEffect = ServiceLocatorObject.Instance.StatusEffectFactory.CreateStatusEffectFromBlueprint(statusEffectBlueprint.Value, new object[] { this, originator });
            statusEffect.Apply(character, originator, tokenSource);
        }
    }

    private void SendTargetDamageStructs(Character target, Character originator, CancellationTokenSource tokenSource)
    {
        List<Damage> modifiedDamages = ModifyTargetDamageStructs(originator);
        target.StatSystem.ReceiveModifiedDamageStructs(modifiedDamages, originator, tokenSource);
    }

    private void SendTargetHealingStructs(Character target, Character originator, CancellationTokenSource tokenSource)
    {
        List<Healing> modifiedHealings = ModifyTargetHealingStructs(originator);
        target.StatSystem.ReceiveModifiedHealingStructs(modifiedHealings, originator, tokenSource);
    }

    private void SendTargetMoveStructs(Character target, Character originator, CancellationTokenSource tokenSource)
    {
        List<Move> modifiedStructs = ModifyTargetMoveStructs(originator);
        target.StatSystem.ReceiveModifiedMoveStructs(modifiedStructs, originator, tokenSource);
    }

    private void ApplySelfStatusEffects(Character originator, CancellationTokenSource tokenSource)
    {
        foreach (InterfaceReference<IStatusEffectBlueprint> statusEffectBlueprint in _selfStatusEffectBlueprints)
        {
            IStatusEffect statusEffect = ServiceLocatorObject.Instance.StatusEffectFactory.CreateStatusEffectFromBlueprint(statusEffectBlueprint.Value, new object[] { this, originator });
            statusEffect.Apply(originator, originator, tokenSource);
        }
    }

    private void SendSelfTargetDamageStructs(Character originator, CancellationTokenSource tokenSource)
    {
        List<Damage> modifiedDamages = ModifySelfDamageStructs(originator);
        originator.StatSystem.ReceiveModifiedDamageStructs(modifiedDamages, originator, tokenSource);
    }

    private void SendSelfTargetHealingStructs(Character originator, CancellationTokenSource tokenSource)
    {
        List<Healing> modifiedHealings = ModifySelfHealingStructs(originator);
        originator.StatSystem.ReceiveModifiedHealingStructs(modifiedHealings, originator, tokenSource);
    }

    private void SendSelfTargetMoveStructs(Character originator, CancellationTokenSource tokenSource)
    {
        List<Move> modifiedStructs = ModifySelfMoveStructs(originator);
        originator.StatSystem.ReceiveModifiedMoveStructs(modifiedStructs, originator, tokenSource);
    }

    private List<Damage> ModifyTargetDamageStructs(Character originator)
    {
        List<Damage> modifiedDamages = new List<Damage>();
        foreach (Damage damage in _targetSkillDamages)
        {
            modifiedDamages.Add(originator.StatSystem.ModifyOutgoingDamage(damage));
        }
        return modifiedDamages;
    }

    private List<Healing> ModifyTargetHealingStructs(Character originator)
    {
        List<Healing> modifiedHealings = new List<Healing>();
        foreach (Healing healing in _targetSkillHealings)
        {
            modifiedHealings.Add(originator.StatSystem.ModifyOutgoingHealing(healing));
        }
        return modifiedHealings;
    }

    private List<Move> ModifyTargetMoveStructs(Character originator)
    {
        List<Move> modifiedMoves = new List<Move>();
        foreach (Move move in _targetSkillMoves)
        {
            modifiedMoves.Add(originator.StatSystem.ModifyOutgoingMove(move));
        }
        return modifiedMoves;
    }

    private List<Damage> ModifySelfDamageStructs(Character originator)
    {
        List<Damage> modifiedDamages = new List<Damage>();
        foreach (Damage damage in _selfSkillDamages)
        {
            modifiedDamages.Add(originator.StatSystem.ModifyOutgoingDamage(damage));
        }
        return modifiedDamages;
    }

    private List<Healing> ModifySelfHealingStructs(Character originator)
    {
        List<Healing> modifiedHealings = new List<Healing>();
        foreach (Healing healing in _selfSkillHealings)
        {
            modifiedHealings.Add(originator.StatSystem.ModifyOutgoingHealing(healing));
        }
        return modifiedHealings;
    }

    private List<Move> ModifySelfMoveStructs(Character originator)
    {
        List<Move> modifiedMoves = new List<Move>();
        foreach (Move move in _selfSkillMoves)
        {
            modifiedMoves.Add(originator.StatSystem.ModifyOutgoingMove(move));
        }
        return modifiedMoves;
    }

    #endregion

    #region Inner Classes
    #endregion

}
