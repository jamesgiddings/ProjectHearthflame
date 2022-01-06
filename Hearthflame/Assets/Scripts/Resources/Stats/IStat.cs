using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GramophoneUtils.Stats
{
	public interface IStat
	{
		float Value { get; }

		void UpdateBaseValue(float newBase);
		void AddModifier(StatModifier modifier);
		bool RemoveModifier(StatModifier modifier);
	}
}

