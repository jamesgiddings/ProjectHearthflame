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
        float TuningMultiplier { get; }
        List<IStatModifier> StatModifiers { get; }

        float GetBaseValue();
        void UpdateBaseValue(float newBase);
		void IncrementBaseValue(float increment);
        void AddModifier(IStatModifier modifier);
		bool RemoveModifier(IStatModifier modifier);
        bool RemoveAllModifiersFromSources(object[] sources);

    }
}