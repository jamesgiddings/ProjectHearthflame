using GramophoneUtils.Characters;
using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace GramophoneUtils.Stats
{
	[Serializable]
	[CreateAssetMenu(fileName = "New Stat Modifier Blueprint", menuName = "Skills/Effects/Stat Modifier Blueprint")]
	public class StatModifierBlueprint : ScriptableObject, IStatModifierBlueprint
    {
        #region Attributes/Fields/Properties

        [SerializeField] private StatType statType;
        public IStatType StatType => statType;


        [SerializeField] private ModifierNumericType modifierType;
        public ModifierNumericType ModifierNumericType => modifierType;


        [SerializeField] private StatModifierType _statModifierType;
        public StatModifierType StatModifierType => _statModifierType;


        [SerializeField, Tooltip("Percentage values should be expressed as a decimal between 0 and 1 (or higher), i.e. not 50 for 50%.")] private float value;
        public float Value => value;


        [SerializeField] private int duration;
        public int Duration => duration;

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
        #endregion

        #region Inner Classes
        #endregion
    }
}

