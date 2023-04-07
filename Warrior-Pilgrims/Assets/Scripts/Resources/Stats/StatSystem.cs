using System;
using System.Collections.Generic;
using System.Threading;

namespace GramophoneUtils.Stats
{
    public class StatSystem
    {
        #region Attributes/Fields/Properties

        private ICharacter _character;

        public StatusEffectType ActiveStatusEffectTypes => GetActiveStatusEffectTypes();

        private List<StatusEffectTypeWrapper> _activeStatusEffects = new List<StatusEffectTypeWrapper>();

        public List<StatusEffectTypeWrapper> ActiveStatusEffects { get => _activeStatusEffects; }

        private readonly Dictionary<IStatType, IStat> stats = new Dictionary<IStatType, IStat>();

        private List<IStatusEffect> _statusEffects = new List<IStatusEffect>();
        public List<IStatusEffect> StatusEffects => _statusEffects;

        private Dictionary<string, IStatType> _statTypeStringRefDictionary = new Dictionary<string, IStatType>();
        public Dictionary<string, IStatType> StatTypeStringRefDictionary => _statTypeStringRefDictionary;

        public Action<BattlerNotificationImpl> OnStatSystemNotification;

        public Action<List<IStatusEffect>> OnStatusEffectsListUpdated;
        public Dictionary<IStatType, IStat> Stats => stats; //getter      

        #endregion

        #region Constructors

        public StatSystem(ICharacter character) //constructor 2
        {
            foreach (var stat in character.Stats.Stats)
            {
                stats.Add(stat.StatType, new Stat(stat.Value, stat.StatType.TuningMultiplier));
                _statTypeStringRefDictionary.Add(stat.StatType.Name, stat.StatType);
            }
            this._character = character;
        }

        #endregion

        #region Callbacks
        #endregion

        #region Public Functions

        public void AddStatusEffectObject(IStatusEffect statusEffect)
        {
            _statusEffects.Add(statusEffect);
            OnStatusEffectsListUpdated?.Invoke(_statusEffects);
        }

        public void RemoveStatusEffectObject(IStatusEffect statusEffect)
        {
            bool isEqual = statusEffect.Equals(_statusEffects[0]);
            _statusEffects.Remove(statusEffect);
            OnStatusEffectsListUpdated?.Invoke(_statusEffects);
        }

        public void RemoveStatusEffectTypeWrapper(StatusEffectTypeWrapper statusEffectTypeWrapper)
        {
            _activeStatusEffects.Remove(statusEffectTypeWrapper);
        }

        public void UpdateStatBaseValue(StatModifierBlueprint statComponentBlueprint)
        {
            if (!stats.TryGetValue(statComponentBlueprint.StatType, out IStat stat))
            {
                stat = new Stat(statComponentBlueprint.StatType);
                stats.Add(statComponentBlueprint.StatType, stat);
            }
            stat.UpdateBaseValue(statComponentBlueprint.Value);
        }

        public void IncrementStatBaseValue(StatModifierBlueprint statComponentBlueprint)
        {
            if (!stats.TryGetValue(statComponentBlueprint.StatType, out IStat stat))
            {
                stat = new Stat(statComponentBlueprint.StatType);
                stats.Add(statComponentBlueprint.StatType, stat);
            }
            stat.IncrementBaseValue(statComponentBlueprint.Value);
        }

        public IStat GetStat(IStatType type)
        {
            if (!stats.TryGetValue(type, out IStat stat))
            {
                stat = new Stat(type);
                stats.Add(type, stat);
            }
            return stat;
        }

        public float GetStatValue(IStatType type)
        {
            if (!stats.TryGetValue(type, out IStat stat))
            {
                stat = new Stat(type);
                stats.Add(type, stat);
            }

            return stat.Value;
        }

        public float GetStatTuningMultiplier(IStatType type)
        {
            if (!stats.TryGetValue(type, out IStat stat))
            {
                stat = new Stat(type);
                stats.Add(type, stat);
            }

            return stat.TuningMultiplier;
        }

        public float GetBaseStatValue(IStatType type)
        {
            if (!stats.TryGetValue(type, out IStat stat))
            {
                stat = new Stat(type);
                stats.Add(type, stat);
            }

            return stat.GetBaseValue();
        }

        public void AddModifier(IStatModifier modifier)
        {
            if (!stats.TryGetValue(modifier.StatType, out IStat stat))
            {
                stat = new Stat(modifier.StatType);
                stats.Add(modifier.StatType, stat);
            }
            stat.AddModifier(modifier);
        }

        public void RemoveModifier(IStatModifier modifier)
        {
            if (!stats.TryGetValue(modifier.StatType, out IStat stat))
            {
                return;
            }
            stat.RemoveModifier(modifier);
        }

        public void AddElapsible(IElapsible elapsible)
        {
            elapsible.OnDurationElapsed += RemoveElapsible;
        }

        public void RemoveElapsible(IElapsible elapsible)
        {
            elapsible.Remove(_character, null, null); // TODO, we might not need to pass the rest
        }

        public void ReceiveStatusEffectType(StatusEffectTypeWrapper receivedStatusEffectType, ICharacter originator, CancellationTokenSource tokenSource)
        {
            StatusEffectTypeWrapper filteredStatusEffectType = FilterStatusEffectTypes(receivedStatusEffectType, originator);
            _activeStatusEffects.Add(filteredStatusEffectType);
        }

        public void ReceiveModifiedStatModifiers(List<IStatModifier> receivedStatModifiers, ICharacter originator, CancellationTokenSource tokenSource)
        {
            List<IStatModifier> filteredStatModifiers = FilterResists(receivedStatModifiers, originator);
            if (filteredStatModifiers.Count == 0 && receivedStatModifiers.Count > 0)
            {
                string evasionString = receivedStatModifiers[0].StatModifierType == StatModifierType.Magical ? ServiceLocatorObject.Instance.Constants.MagicEvasionText : ServiceLocatorObject.Instance.Constants.PhysicalEvasionText;
                OnStatSystemNotification?.Invoke(new BattlerNotificationImpl(evasionString));
                tokenSource.Cancel();
                return;
            }
            List<IStatModifier> modifiedFilteredStatModifiers = new List<IStatModifier>();

            foreach (IStatModifier statModifier in filteredStatModifiers)
            {
                modifiedFilteredStatModifiers.Add(ModifyIncomingStatModifier(statModifier));
            }
            ApplyModifiedStatModifiers(modifiedFilteredStatModifiers);
        }

        public void ReceiveModifiedDamageStructs(List<Damage> receivedDamages, ICharacter originator, CancellationTokenSource tokenSource)
        {
            List<Damage> filteredDamages = FilterMisses(receivedDamages, originator);
            if (filteredDamages.Count == 0 && receivedDamages.Count > 0)
            {
                string evasionString = receivedDamages[0].AttackType == AttackType.Magic ? ServiceLocatorObject.Instance.Constants.MagicEvasionText : ServiceLocatorObject.Instance.Constants.PhysicalEvasionText;
                OnStatSystemNotification?.Invoke(new BattlerNotificationImpl(evasionString));
                tokenSource.Cancel();
                return;
            }
            List<Damage> modifiedDamages = new List<Damage>();
            foreach (Damage damage in filteredDamages)
            {
                modifiedDamages.Add(ModifyIncomingDamage(damage));
            }
            ApplyModifiedDamageObjects(modifiedDamages);
        }

        public void ReceiveModifiedHealingStructs(List<Healing> receivedHealings, ICharacter originator, CancellationTokenSource tokenSource)
        {
            List<Healing> modifiedHealings = new List<Healing>();
            foreach (Healing healing in receivedHealings)
            {
                modifiedHealings.Add(ModifyIncomingHealing(healing));
            }

            ApplyModifiedHealingObjects(modifiedHealings);
        }

        public void ReceiveModifiedMoveStructs(List<Move> receivedMoves, ICharacter originator, CancellationTokenSource tokenSource)
        {
            List<Move> modifiedMoves = new List<Move>();
            foreach (Move move in receivedMoves)
            {
                modifiedMoves.Add(ModifyIncomingMove(move));
            }

            ApplyModifiedMoveObjects(modifiedMoves);
        }

        public Damage ModifyOutgoingDamage(Damage damage)
        {
            float newValue = damage.Value + (GetStatValue(StatTypeStringRefDictionary["Strength"]) * GetStatTuningMultiplier(StatTypeStringRefDictionary["Strength"]));

            return new Damage(damage.Name, damage.UID, damage.Sprite, newValue, damage.Element, damage.AttackType, damage.Source);
        }

        public Healing ModifyOutgoingHealing(Healing healing)
        {
            float newValue = healing.Value + (GetStatValue(StatTypeStringRefDictionary["Magic"]) * GetStatTuningMultiplier(StatTypeStringRefDictionary["Magic"]));

            return new Healing(healing.Name, healing.UID, healing.Sprite, newValue, healing.Source);
        }

        public Move ModifyOutgoingMove(Move move)
        {
            int newValue = move.Value; // TODO, decide if this will have any effect

            return new Move(move.Name, move.UID, move.Sprite, newValue, move.MoveByValue, move.Source);
        }

        public IStatModifier ModifyOutgoinStatModifiers(IStatModifier statModifier, IStatusEffect statusEffect = null)
        {
            return ServiceLocatorObject.Instance.StatModifierFactory.CreateStatModifierFromInstance(statModifier); // TODO, decide if this will have any effect
        }

        #region Derived Stats

        public float GetMeleeAccuracy()
        {
            return ServiceLocatorObject.Instance.StatSystemConstants.BaseMeleeAccuracy + (GetStatValue(StatTypeStringRefDictionary["Strength"]) * GetStatTuningMultiplier(StatTypeStringRefDictionary["Strength"])) + GetStatValue(StatTypeStringRefDictionary["Melee Accuracy"]);
        }

        public float GetRangedAccuracy()
        {
            return ServiceLocatorObject.Instance.StatSystemConstants.BaseRangedAccuracy + (GetStatValue(StatTypeStringRefDictionary["Dexterity"]) * GetStatTuningMultiplier(StatTypeStringRefDictionary["Dexterity"])) + GetStatValue(StatTypeStringRefDictionary["Ranged Accuracy"]);
        }

        public float GetMagicAccuracy()
        {
            return ServiceLocatorObject.Instance.StatSystemConstants.BaseRangedAccuracy + (GetStatValue(StatTypeStringRefDictionary["Magic"]) * GetStatTuningMultiplier(StatTypeStringRefDictionary["Magic"])) + GetStatValue(StatTypeStringRefDictionary["Magic Accuracy"]);
        }

        public float GetMeleeEvasion()
        {
            return ServiceLocatorObject.Instance.StatSystemConstants.BaseMeleeEvasion + (GetStatValue(StatTypeStringRefDictionary["Speed"]) * GetStatTuningMultiplier(StatTypeStringRefDictionary["Speed"])) + GetStatValue(StatTypeStringRefDictionary["Melee Evasion"]);
        }

        public float GetRangedEvasion()
        {
            return ServiceLocatorObject.Instance.StatSystemConstants.BaseRangedEvasion + (GetStatValue(StatTypeStringRefDictionary["Speed"]) * GetStatTuningMultiplier(StatTypeStringRefDictionary["Speed"])) + GetStatValue(StatTypeStringRefDictionary["Ranged Evasion"]);
        }

        public float GetMagicEvasion()
        {
            return ServiceLocatorObject.Instance.StatSystemConstants.BaseMagicEvasion + (GetStatValue(StatTypeStringRefDictionary["Wits"]) * GetStatTuningMultiplier(StatTypeStringRefDictionary["Wits"])) + GetStatValue(StatTypeStringRefDictionary["Magic Evasion"]);
        }

        #endregion

        public Damage ModifyIncomingDamage(Damage damage)
        {
            float newValue = damage.Value - ServiceLocatorObject.Instance.StatSystemConstants.BasePhysicalArmour;

            return new Damage(damage.Name, damage.UID, damage.Sprite, newValue, damage.Element, damage.AttackType, damage.Source);
        }

        public Healing ModifyIncomingHealing(Healing healing)
        {
            float newValue = healing.Value; // Find modifier here

            return new Healing(healing.Name, healing.UID, healing.Sprite, newValue, healing.Source);
        }

        public Move ModifyIncomingMove(Move move)
        {
            int newValue = move.Value; // TODO, decide if this will have any effect

            return new Move(move.Name, move.UID, move.Sprite, newValue, move.MoveByValue, move.Source);
        }

        public bool CharacterIsNotInControl()
        {
            foreach (StatusEffectType statusEffectType in ServiceLocatorObject.Instance.LossOfControlStatusEffectTypes)
            {
                if (GetActiveStatusEffectTypes().HasFlag(statusEffectType))
                {
                    return true;
                }
            }
            return false;
        }

        #endregion

        #region Protected Functions
        #endregion

        #region Private Functions

        private StatusEffectType GetActiveStatusEffectTypes()
        {
            StatusEffectType activeStatusEffects = StatusEffectType.None;

            foreach (StatusEffectTypeWrapper wrapper in _activeStatusEffects)
            {
                activeStatusEffects |= wrapper.StatusEffectTypeFlag;
            }

            return activeStatusEffects;
        }

        private IStatModifier ModifyIncomingStatModifier(IStatModifier statModifier)
        {
            return ServiceLocatorObject.Instance.StatModifierFactory.CreateStatModifierFromInstance(statModifier); // TODO, decide if this will have any effect
        }

        private void ApplyModifiedStatModifiers(List<IStatModifier> modifiedFilteredStatModifiers)
        {
            foreach (IStatModifier statModifier in modifiedFilteredStatModifiers)
            {
                AddModifier(statModifier);
            }
        }

        private void ApplyModifiedDamageObjects(List<Damage> modifiedDamages)
        {
            foreach (Damage damage in modifiedDamages)
            {
                _character.HealthSystem.AddDamage(damage);

            }
        }

        private void ApplyModifiedHealingObjects(List<Healing> modifiedDamages)
        {
            foreach (Healing healing in modifiedDamages)
            {
                _character.HealthSystem.AddHealing(healing);
            }
        }

        private void ApplyModifiedMoveObjects(List<Move> modifiedMoves)
        {
            foreach (Move move in modifiedMoves)
            {
                if (move.MoveByValue == true && move.Value == 0) // if we're not swapping the character, and it's a 0 move, cancel the move
                {
                    continue;
                }

                CharacterOrder characterOrder = ServiceLocatorObject.Instance.CharacterModel.GetCharacterOrderByCharacter(_character);
                if (move.MoveByValue)
                {
                    characterOrder.MoveCharacter(_character, move.Value);
                }
                else
                {
                    characterOrder.SwapCharacterIntoSlot(_character, move.Value);
                }
            }
        }

        private List<IStatModifier> FilterResists(List<IStatModifier> receivedStatModifiers, ICharacter originator)
        {
            List<IStatModifier> filteredModifiers = new List<IStatModifier>();
            foreach (IStatModifier modifier in receivedStatModifiers)
            {
                if (!GetIfResists(modifier, originator))
                {
                    filteredModifiers.Add(modifier);
                }
            }
            return filteredModifiers;
        }

        private bool GetIfResists(IStatModifier modifier, ICharacter originator)
        {
            switch (modifier.StatModifierType)
            {
                case StatModifierType.Magical:
                    if (GetMagicEvasion() > originator.StatSystem.GetMagicAccuracy()) // TODO, these are not the right stats for statModifier resistance
                    {
                        return true;
                    }
                    else return false;
                case StatModifierType.Physical:
                    if (GetMeleeEvasion() > originator.StatSystem.GetMeleeAccuracy()) // TODO, these are not the right stats for statModifier resistance
                    {
                        return true;
                    }
                    else return false;
            }
            return false;
        }

        private List<Damage> FilterMisses(List<Damage> receivedDamages, ICharacter originator)
        {
            List<Damage> filteredDamages = new List<Damage>();
            foreach (Damage damage in receivedDamages)
            {
                if (!GetIfMisses(damage, originator))
                {
                    filteredDamages.Add(damage);
                }
            }
            return filteredDamages;
        }

        private bool GetIfMisses(Damage damage, ICharacter originator)
        {
            switch (damage.AttackType)
            {
                case AttackType.Magic:
                    if (GetMagicEvasion() > originator.StatSystem.GetMagicAccuracy())
                    {
                        return true;
                    }
                    else return false;
                case AttackType.Ranged:
                    if (GetRangedEvasion() > originator.StatSystem.GetRangedAccuracy())
                    {
                        return true;
                    }
                    else return false;
                case AttackType.Melee:
                    if (GetMeleeEvasion() > originator.StatSystem.GetMeleeAccuracy())
                    {
                        return true;
                    }
                    else return false;
                default:
                    return false;
            }
        }

        private StatusEffectTypeWrapper FilterStatusEffectTypes(StatusEffectTypeWrapper receivedStatusEffectType, ICharacter originator)
        {
            List<StatusEffectType> listOfIndividualStatusEffectTypes = new List<StatusEffectType>();

            var flags = (StatusEffectType[])StatusEffectType.GetValues(typeof(StatusEffectType)); // get an array of the flags

            foreach (var flag in flags)
            {
                if (flag == StatusEffectType.None)
                {
                    continue;
                }

                if ((receivedStatusEffectType.StatusEffectTypeFlag & flag) != 0)
                {
                    listOfIndividualStatusEffectTypes.Add(flag);
                }
            }

            StatusEffectType filteredUnresistedStatusEffectType = StatusEffectType.None;

            foreach (StatusEffectType statusEffectTypeFlag in listOfIndividualStatusEffectTypes)
            {
                if (!GetIsStatusEffectTypeResisted(statusEffectTypeFlag, originator))
                {
                    filteredUnresistedStatusEffectType |= statusEffectTypeFlag;
                }
            }

            return new StatusEffectTypeWrapper(filteredUnresistedStatusEffectType, originator);
        }

        private bool GetIsStatusEffectTypeResisted(StatusEffectType receivedStatusEffectTypeFlag, ICharacter originator)
        {
            switch (receivedStatusEffectTypeFlag)
            {
                case StatusEffectType.Stun:
                    if (GetMeleeEvasion() > originator.StatSystem.GetMeleeAccuracy()) // TODO, these are not the right stats for statModifier resistance
                    {
                        return true;
                    }
                    else return false;
                case StatusEffectType.Burn:
                    if (GetMagicEvasion() > originator.StatSystem.GetMagicAccuracy()) // TODO, these are not the right stats for statModifier resistance
                    {
                        return true;
                    }
                    else return false;
                case StatusEffectType.None:
                    return true;
                default:
                    return false;
            }
        }

        #endregion

        #region Inner Classes
        #endregion

    }
}
