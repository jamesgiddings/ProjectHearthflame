using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using GramophoneUtils.Characters;

namespace GramophoneUtils.Stats
{
	public enum StatModifierTypes
	{
		Flat = 0,
		PercentAdditive = 1,
		PercentMultiplicative = 2,
	}

	[Serializable]
	public class StatModifier
	{
		#region Attributes/Fields/Properties

		private bool _isSubscribedToOnCharacterTurnAdvance = false;

        [SerializeField] private IStatType statType;
        public IStatType StatType => statType;


        [SerializeField] private StatModifierTypes modifierType;
        public StatModifierTypes ModifierType => modifierType;


        [SerializeField] private float value;
        public float Value => value;


        [SerializeField] private int duration = -1;
        public int Duration => duration;


        [SerializeField] private readonly object source = null;
        public object Source => source;

        public Action<StatModifier> OnDurationElapsed;

        #endregion

        #region Constructors

        public StatModifier(IStatType statType, StatModifierTypes modifierType, float value, int duration = -1, object source = null)
		{
			this.statType = statType;
			this.modifierType = modifierType;
			this.value = value;
			this.duration = duration;
			this.source = source;
			if (duration != -1)
			{
				Turn.OnTurnElapsed += DecrementDuration; //subscribe to turn clock
			}
		}

        #endregion

        #region Public Functions

		public void SubscribeToCharacterOnTurnElapsed(Character character)
		{
			if (!_isSubscribedToOnCharacterTurnAdvance)
			{
                character.OnCharacterTurnElapsed += DecrementDuration;
            }
			_isSubscribedToOnCharacterTurnAdvance = true;
        }

        public void UnsubscribeFromCharacterOnTurnElapsed(Character character)
        {
            if (_isSubscribedToOnCharacterTurnAdvance)
            {
                character.OnCharacterTurnElapsed -= DecrementDuration;
            }
            _isSubscribedToOnCharacterTurnAdvance = false;
        }

        public void DecrementDuration()
		{
			duration -= 1;
			if (duration <= 0)
			{
				ElapseDuration();
			}
		}

		public void ElapseDuration()
		{
			OnDurationElapsed?.Invoke(this);
		}

		#endregion
	}
}
