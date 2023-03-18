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

        [SerializeField] private string _name;
        public string Name => _name;


        [SerializeField] private readonly string _uid;
        public string UID => _uid;


        [SerializeField] private Sprite _sprite;
        public Sprite Sprite => _sprite;


        [SerializeField] private StatType statType;
        public IStatType StatType => statType;


        [SerializeField] private ModifierNumericType modifierType;
        public ModifierNumericType ModifierNumericType => modifierType;


        [SerializeField] private StatModifierType _statModifierType;
        public StatModifierType StatModifierType => _statModifierType;


        [SerializeField, Tooltip("Percentage values should be expressed as a decimal between 0 and 1 (or higher), i.e. not 50 for 50%.")] private FloatReference _value;
        public float Value => _value;


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

