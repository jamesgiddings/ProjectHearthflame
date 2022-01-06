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
		[SerializeField] StatType StatType;
		[SerializeField] StatModifierTypes ModifierType;
		[SerializeField] float Value;
		[SerializeField] int Duration;

		public T CreateBlueprintInstance<T>(object source = null)
		{
			StatModifier blueprintInstance = new StatModifier(StatType, ModifierType, Value, Duration, source);
			return (T) Convert.ChangeType(blueprintInstance, typeof(T));
		}


		//StatComponentObject 

		// the design of this class is to be a blueprint for the creation of StatModifiers at runtime.

		// some will be set, some can be level scaled to the player, some can be ranges, so can be random stats 

		// there could be a WeaponStatComponents object, which groups together some statComponenets and determines which of a collection are present on an item
	}
}

