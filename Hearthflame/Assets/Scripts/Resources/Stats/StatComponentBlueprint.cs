using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GramophoneUtils.Stats
{
	[Serializable]
	[CreateAssetMenu(fileName = "New Stat Component Blueprint", menuName = "StatSystem/Stat Component Blueprint")]
	public class StatComponentBlueprint : ScriptableObject, IBlueprint
	{
		[SerializeField] private StatType statType;
		[SerializeField] private StatModifierTypes modifierType;
		[SerializeField] private float value;
		[SerializeField] private int duration;

		public StatModifierTypes ModifierType => modifierType; //getter
		public StatType StatType => statType; //getter
		public float Value => value; //getter
		public int Duration => duration; //getter

		public T CreateBlueprintInstance<T>(object source = null)
		{
			StatModifier blueprintInstance = new StatModifier(statType, modifierType, value, duration, source);
			return (T) Convert.ChangeType(blueprintInstance, typeof(T));
		}


		// StatComponentObject 

		// the design of this class is to be a blueprint for the creation of StatModifiers at runtime.

		// some will be set, some can be level scaled to the player, some can be ranges, so can be random stats 

		// there could be a WeaponStatComponents object, which groups together some statComponenets and determines which of a collection are present on an item
	}
}

