using UnityEngine;
using System;
using GramophoneUtils.Characters;
using System.Threading.Tasks;
using System.Threading;

namespace GramophoneUtils.Stats
{
	public enum ModifierNumericType
	{
		Flat = 0,
		PercentAdditive = 1,
		PercentMultiplicative = 2,
	}

	public enum StatModifierType
	{
		Physical = 0,
		Magical = 1
	}

	[Serializable]
	public class StatModifier : IStatModifier
	{
		#region Attributes/Fields/Properties

		private bool _isSubscribedToOnCharacterTurnAdvance = false;

        [SerializeField] private IStatType _statType;
        public IStatType StatType => _statType;


        [SerializeField] private ModifierNumericType _modifierNumericType;
        public ModifierNumericType ModifierNumericType => _modifierNumericType;


		[SerializeField] private StatModifierType _statModifierType;
		public StatModifierType StatModifierType => _statModifierType;


		[SerializeField] private float _value;
        public float Value => _value;


        [SerializeField] private int _duration = -1;
        public int Duration => _duration;


        [SerializeField] private readonly object[] _sources = null;
        public object[] Sources => _sources;

        public Action<IStatModifier> OnDurationElapsed { get; set; }
		     
        #endregion

        #region Constructors

        public StatModifier(
			IStatType statType, 
			ModifierNumericType modifierType, 
			StatModifierType statModifierType,
			float value, 
			object[] sources = null)
		{
			_statType = statType;
			_modifierNumericType = modifierType;
			_statModifierType = statModifierType;
			_value = value;
			_sources = sources;
		}

        #endregion

        #region Public Functions

		public async Task Apply(Character target, Character originator, CancellationTokenSource tokenSource)
		{
			throw new NotImplementedException();
		}

		public Task Remove(Character target, Character originator, CancellationTokenSource tokenSource)
		{
			throw new NotImplementedException();
		}

		#endregion
	}
}
