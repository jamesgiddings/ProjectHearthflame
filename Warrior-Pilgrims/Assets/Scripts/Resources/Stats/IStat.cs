using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GramophoneUtils.Stats
{
	public interface IStat
	{
        Action OnStatChanged { get; set; }
        float Value { get; }
        List<StatModifier> StatModifiers { get; }

        float GetBaseValue();
        void UpdateBaseValue(float newBase);
		void IncrementBaseValue(float increment);
        void AddModifier(StatModifier modifier);
		bool RemoveModifier(StatModifier modifier);
        bool RemoveAllModifiersFromSource(object source);

    }
}