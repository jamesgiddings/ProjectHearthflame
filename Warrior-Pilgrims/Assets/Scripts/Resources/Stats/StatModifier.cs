using UnityEngine;
using System;
using System.Threading.Tasks;
using System.Threading;
using System.Text;

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

		[SerializeField] private string _name;
		public string Name => _name;


        private readonly string _uid;
        public string UID => _uid;


		private Sprite _sprite;
		public Sprite Sprite => _sprite;


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
			string name,
			string uid,
			Sprite sprite,
			IStatType statType, 
			ModifierNumericType modifierType, 
			StatModifierType statModifierType,
			float value, 
			object[] sources = null)
		{
			_name = name;
			_uid = uid;
			_sprite = sprite;
			_statType = statType;
			_modifierNumericType = modifierType;
			_statModifierType = statModifierType;
			_value = value;
			_sources = sources;
		}

        #endregion

        #region Public Functions

		public async Task Apply(ICharacter target, ICharacter originator, CancellationTokenSource tokenSource)
		{
			throw new NotImplementedException();
		}

		public Task Remove(ICharacter target, ICharacter originator, CancellationTokenSource tokenSource)
		{
			throw new NotImplementedException();
		}

		public string GetInfoDisplayText()
		{
			StringBuilder builder = new StringBuilder();

			string plusOrMinus = _value >= 0 ? "+" : "";

			float percentValue = _value * 100f;

			string flatOrPercentageValueString = _modifierNumericType == ModifierNumericType.Flat ? _value.ToString() : percentValue.ToString() + "%";

			builder
				.Append(plusOrMinus)
				.Append(flatOrPercentageValueString)
				.Append(" ")
				.Append(_statType.Name);

			return builder.ToString();
        }

		#endregion
	}
}
