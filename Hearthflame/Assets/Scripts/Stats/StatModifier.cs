using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace GramophoneUtils.Stats
{
	public enum StatModifierTypes
	{
		Flat = 0,
		PercentAdditive = 1,
		PercentMultiplicative = 2,
	}

	//[Serializable]
	public class StatModifier
	{
		[SerializeField] private IStatType statType;
		[SerializeField] private StatModifierTypes modifierType;
		[SerializeField] private float value;
		[SerializeField] private int duration = -1;
		[SerializeField] private readonly object source = null;

		public Action<StatModifier> OnDurationElapsed;

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

		public IStatType StatType => statType;
		public StatModifierTypes ModifierType => modifierType;
		public float Value => value;
		public int Duration => duration;
		public object Source => source;

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
	}
	
}
