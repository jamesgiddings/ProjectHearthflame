using GramophoneUtils.Stats;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace GramophoneUtils.Items.Containers
{
	[CreateAssetMenu(fileName = "New Equipment Item", menuName = "Items/Equipment Item")]
	public class EquipmentItem : InventoryItem, IEquippable
	{
		[Header("Equipment Data")]
		[SerializeField] private EquipmentType equipmentType;
		[SerializeField] private bool isUnique = false;
		[SerializeField] private string flavourText = "Flavour text here.";
		[SerializeField] private List<StatComponentBlueprint> weaponEffectBlueprints;
		private List<StatModifier> weaponEffects = new List<StatModifier>();

		public EquipmentType EquipmentType => equipmentType;
		private void OnEnable()
		{
			InstanceWeaponEffectBlueprints();
		}

		private void InstanceWeaponEffectBlueprints()
		{
			if (weaponEffectBlueprints.Count > 0)
			{
				foreach (var blueprint in weaponEffectBlueprints)
				{
					weaponEffects.Add(blueprint.CreateBlueprintInstance<StatModifier>(this));
				}
			}
		}

		public void Equip(StatSystem statSystem)
		{
			foreach (StatModifier weaponEffect in weaponEffects)
			{
				Debug.Log("Equip method is adding " + weaponEffect.Value + " value.");
				statSystem.AddModifier(new StatModifier(weaponEffect.StatType, weaponEffect.ModifierType, weaponEffect.Value, weaponEffect.Duration, this)); // we make a new effect, which allows us to add the source as 'this' when we equip the item
			}
		}
		public override string GetInfoDisplayText()
		{
			StringBuilder builder = new StringBuilder();

			builder.Append(Rarity.Name).AppendLine();
			foreach (var effect in weaponEffects)
			{
				builder.Append("+").Append(effect.Value).Append(" ").Append(effect.StatType.Name).AppendLine();
			}
			//builder.Append("<color=green>Use: ").Append(useText).Append("</color>").AppendLine();
			builder.Append("Max Stack: ").Append(MaxStack).AppendLine();
			builder.Append("Sell Price: ").Append(SellPrice).Append(" Gold").AppendLine();
			builder.Append('"').Append(flavourText).Append('"');

			return builder.ToString();
		}

		public void Unequip(StatSystem statSystem)
		{
			
			foreach (KeyValuePair<IStatType, Stat> entry in statSystem.Stats)
			{
				entry.Value.RemoveAllModifiersFromSource(this);
			}
		}
	}
}