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
		[SerializeField] private CharacterClass[] classRestrictions;

		private List<StatModifier> weaponEffects;

		public CharacterClass[] ClassRestrictions => classRestrictions;

		public EquipmentType EquipmentType => equipmentType;
		private void OnEnable()
		{
			weaponEffects = new List<StatModifier>();
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

		public void Equip(Character character)
		{
			foreach (StatModifier weaponEffect in weaponEffects)
			{
				character.StatSystem.AddModifier(new StatModifier(weaponEffect.StatType, weaponEffect.ModifierType, weaponEffect.Value, weaponEffect.Duration, this)); // we make a new effect, which allows us to add the source as 'this' when we equip the item
			}
		}
		public override string GetInfoDisplayText()
		{
			StringBuilder builder = new StringBuilder();

			builder.Append(Rarity.Name).AppendLine();
			if (classRestrictions.Length > 0)
			{
				for (int i = 0; i < classRestrictions.Length; i++)
				{
					if (i < classRestrictions.Length - 1)
					{
						builder.Append("<color=yellow>" + classRestrictions[i].Name).Append(", </color>");
					}
					else
					{
						builder.Append("<color=yellow>" + classRestrictions[i].Name).Append(" only.</color>").AppendLine();
					}
				}
			}
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

		public void Unequip(Character character)
		{
			foreach (KeyValuePair<IStatType, Stat> entry in character.StatSystem.Stats)
			{
				entry.Value.RemoveAllModifiersFromSource(this);
			}
		}

		public override void Use(CharacterBehaviour characterBehaviour, InventorySlotUI inventorySlotUI)
		{
			//Debug.Log("Using equipment item " + Name + " on " + characterBehaviour.Character.Name);
			characterBehaviour.gameObject.GetComponent<EquipmentInventory>().TryToEquip(this, inventorySlotUI);
		}
	}
}