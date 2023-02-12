using GramophoneUtils.Stats;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GramophoneUtils.Stats
{
    [Serializable]
    [CreateAssetMenu(fileName = "New Status Effect Blueprint", menuName = "Skills/Status Effects/Status Effect Blueprint")]
    public class StatusEffectBlueprint : ScriptableObject, IStatusEffectBlueprint
    {
        #region Attributes/Fields/Properties

        private object _source = null;
        public object Source { get => _source; set => _source = value; }

        [SerializeField] private StatusEffectType _statusEffectTypeFlag;
        public StatusEffectType StatusEffectTypeFlag => _statusEffectTypeFlag;

        [SerializeField] List<StatModifierBlueprint> _statModifierBlueprints;

        [SerializeField] private int _duration;
        public int Duration => _duration;


        [SerializeField] private TurnDamageHealingAndMoveEffects[] _turnDamageHealingAndMoveEffects = new TurnDamageHealingAndMoveEffects[0];
        public TurnDamageHealingAndMoveEffects[] TurnDamageAndHealingEffects => _turnDamageHealingAndMoveEffects.Reverse().ToArray(); // Reversed so that we can access the current turn effect by duration, which is decremented


        [SerializeField] private bool _damageMustLandForOtherEffectsToLand;
        public bool DamageMustLandForOtherEffectsToLand => _damageMustLandForOtherEffectsToLand;


        [SerializeField] private string _name;
        public string Name => _name;


        [SerializeField, TextArea] private string _tooltipText;
        public string TooltipText => _tooltipText;


        [SerializeField] private Sprite _icon;
        public Sprite Icon => _icon;


        public List<IStatModifier> StatModifiers => CreateStatModifierInstances(_source);

        public Damage[][] TurnDamageEffects => CreateDamageArrays(_source);

        public Healing[][] TurnHealingEffects => CreateHealingArrays(_source);

        public Move[][] TurnMoveEffects => CreateMoveArrays(_source);

        #endregion

        #region Constructors
        #endregion

        #region Callbacks
        #endregion

        #region Public Functions
        #endregion

        #region Protected Functions
        #endregion

        #region Private Functions

        private List<IStatModifier> CreateStatModifierInstances(object source)
        {
            List<IStatModifier> instancedStatModifiers = new List<IStatModifier>();

            foreach (var statModifierBlueprint in _statModifierBlueprints)
            {
                instancedStatModifiers.Add(ServiceLocatorObject.Instance.StatModifierFactory.CreateStatModifierFromBlueprint(statModifierBlueprint, new object[] { source }));
            }

            return instancedStatModifiers;
        }

        private Damage[][] CreateDamageArrays(object source)
        {
            Damage[][] damageArray = new Damage[_duration][];

            int smallestArrayLength = _turnDamageHealingAndMoveEffects.Length < _duration ? _turnDamageHealingAndMoveEffects.Length : _duration;

            for (int i = 0; i < smallestArrayLength; i++)
            {
                damageArray[i] = InstanceDamageEffects(_turnDamageHealingAndMoveEffects[i].TurnDamageEffects, source);
            }

            return damageArray;
        }

        private Healing[][] CreateHealingArrays(object source)
        {
            Healing[][] healingArray = new Healing[_duration][];

            int smallestArrayLength = _turnDamageHealingAndMoveEffects.Length < _duration ? _turnDamageHealingAndMoveEffects.Length : _duration;

            for (int i = 0; i < smallestArrayLength; i++)
            {
                healingArray[i] = InstanceHealingEffects(_turnDamageHealingAndMoveEffects[i].TurnHealingEffects, source);
            }

            return healingArray;
        }

        private Move[][] CreateMoveArrays(object source)
        {
            Move[][] moveArray = new Move[_duration][];

            int smallestArrayLength = _turnDamageHealingAndMoveEffects.Length < _duration ? _turnDamageHealingAndMoveEffects.Length : _duration;

            for (int i = 0; i < smallestArrayLength; i++)
            {
                moveArray[i] = InstanceMoveEffects(_turnDamageHealingAndMoveEffects[i].TurnMoveEffects, source);
            }
            return moveArray;
        }

        private Healing[] InstanceHealingEffects(HealingBlueprint[] _turnHealingBlueprints, object source)
        {
            Healing[] healings = new Healing[_turnHealingBlueprints.Length];
            for (int i = 0; i < _turnHealingBlueprints.Length; i++)
            {
                if (_turnHealingBlueprints[i] != null)
                {
                    healings[i] = _turnHealingBlueprints[i].CreateBlueprintInstance<Healing>(source);
                }
            }
            return healings;
        }

        private Damage[] InstanceDamageEffects(DamageBlueprint[] _turnDamageBlueprints, object source)
        {
            Damage[] damages = new Damage[_turnDamageBlueprints.Length];
            for (int i = 0; i < _turnDamageBlueprints.Length; i++)
            {
                if(_turnDamageBlueprints[i] != null)
                {
                    damages[i] = _turnDamageBlueprints[i].CreateBlueprintInstance<Damage>(source);
                }
            }
            return damages;
        }


        private Move[] InstanceMoveEffects(MoveBlueprint[] _turnMoveBlueprints, object source)
        {
            Move[] moves = new Move[_turnMoveBlueprints.Length];
            for (int i = 0; i < _turnMoveBlueprints.Length; i++)
            {
                if (_turnMoveBlueprints != null)
                {
                    moves[i] = _turnMoveBlueprints[i].CreateBlueprintInstance<Move>(source);
                }
            }
            return moves;
        }

        #endregion

        #region Inner Classes
        #endregion

    }
}

