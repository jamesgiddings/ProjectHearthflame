using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace GramophoneUtils.Stats
{
	[Serializable]
	public class Stat : IStat
	{
        #region Attributes/Fields/Properties

        public Action OnStatChanged { get; set; }

        public float Value
        {
            get
            {
                if (isDirty)
                {
                    value = CalculateValue();
                    isDirty = false;
                }
                return value;
            }
        }

        private readonly List<StatModifier> _modifiers = new List<StatModifier>();
        public List<StatModifier> StatModifiers
        {
            get
            {
                return _modifiers;
            }
        }

        private float baseValue = 0f;

        private bool isDirty = true;

        private float value;




        #endregion

        #region Constructors

        public Stat(float initialValue) => baseValue = initialValue;
        public Stat(IStatType statType) => baseValue = statType.DefaultValue;

        #endregion

        #region Callbacks
        #endregion

        #region Public Functions

        public float GetBaseValue()
        {
            return baseValue;
        }

        public void UpdateBaseValue(float newBase)
        {
            isDirty = true;
            baseValue = newBase;
            baseValue = baseValue < 0f ? 0f : baseValue; // baseValue should not go below 0.
            OnStatChanged?.Invoke();
        }

        public void IncrementBaseValue(float increment)
        {
            isDirty = true;
            baseValue += increment;
            baseValue = baseValue < 0f ? 0f : baseValue; // baseValue should not go below 0.
            OnStatChanged?.Invoke();
        }

        public void AddModifier(StatModifier modifier)
        {
            isDirty = true;
            int index = _modifiers.BinarySearch(modifier, new ByPriority());
            if (index < 0) { index = ~index; }
            _modifiers.Insert(index, modifier);
            OnStatChanged?.Invoke();
        }

        public bool RemoveModifier(StatModifier modifier)
        {
            if (_modifiers.Remove(modifier))
            {
                isDirty = true;
                OnStatChanged?.Invoke();
                return true;
            }
            return false;
        }

        public bool RemoveAllModifiersFromSource(object source)
        {
            int numRemovals = _modifiers.RemoveAll(mod => mod.Source == source);

            if (numRemovals > 0)
            {
                isDirty = true;
                OnStatChanged?.Invoke();
                return true;
            }
            return false;
        }

        #endregion

        #region Protected Functions
        #endregion

        #region Private Functions

        private float CalculateValue()
        {
            float finalValue = baseValue;
            float sumPercentAdditive = 0;

            for (int i = 0; i < _modifiers.Count; i++)
            {
                var modifier = _modifiers[i];

                switch (modifier.ModifierType)
                {
                    case StatModifierTypes.Flat:
                        finalValue += modifier.Value;
                        break;
                    case StatModifierTypes.PercentAdditive:
                        sumPercentAdditive += modifier.Value;
                        if (i + 1 >= _modifiers.Count || _modifiers[i + 1].ModifierType != StatModifierTypes.PercentAdditive)
                        {
                            finalValue *= 1 + sumPercentAdditive;
                        }
                        break;
                    case StatModifierTypes.PercentMultiplicative:

                        finalValue *= 1 + modifier.Value;
                        break;
                }
            }
            // Workaround for float calculation errors, like displaying 12.00001 instead of 12
            return finalValue < 0f ? 0f : (float)Math.Round(finalValue, 4); // the final value should not be below 0.
        }
    }

    #endregion

    #region Inner Classes

    internal class ByPriority : IComparer<StatModifier>
    {
        public int Compare(StatModifier x, StatModifier y)
        {
            if (x.ModifierType > y.ModifierType) { return 1; }
            if (x.ModifierType < y.ModifierType) { return -1; }
            return 0;
        }
    }

    #endregion
}
