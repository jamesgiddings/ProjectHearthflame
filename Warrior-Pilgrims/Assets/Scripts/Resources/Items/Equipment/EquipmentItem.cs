using GramophoneUtils.Stats;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEditor;
using UnityEngine;
using Sirenix.OdinInspector;

namespace GramophoneUtils.Items.Containers
{
	[CreateAssetMenu(fileName = "New Equipment Item", menuName = "Items/Equipment Item")]
	public class EquipmentItem : InventoryItem, IEquippable
	{
		[Header("Equipment Data")]
		[SerializeField] private EquipmentType equipmentType;
		[SerializeField] private bool isUnique = false;
		[SerializeField] private string flavourText = "Flavour text here.";
        [TableList(AlwaysExpanded = true)]
        [SerializeField] private List<StatModifierBlueprint> weaponEffectBlueprints;
        [TableList(AlwaysExpanded = true)]
        [SerializeField] private CharacterClass[] classRestrictions;

		private List<StatModifier> equipmentEffects { get { return InstanceEquipmentEffectBlueprints(); } }

		public CharacterClass[] ClassRestrictions => classRestrictions;

		public EquipmentType EquipmentType => equipmentType;

		private List<StatModifier> InstanceEquipmentEffectBlueprints()
		{
			List<StatModifier> effects = new List<StatModifier>();
			if (weaponEffectBlueprints.Count > 0)
			{
				foreach (var blueprint in weaponEffectBlueprints)
				{
					effects.Add(blueprint.CreateBlueprintInstance<StatModifier>(this));
				}
			}
			return effects;
		}

		public void Equip(GramophoneUtils.Characters.Character character)
		{
			foreach (StatModifier weaponEffect in equipmentEffects)
			{
				character.StatSystem.AddModifier(new StatModifier(weaponEffect.StatType, weaponEffect.ModifierType, weaponEffect.Value, weaponEffect.Duration, this)); // we make a new effect, which allows us to add the source as 'this' when we equip the item
			}
		}
		public override string GetInfoDisplayText()
		{
			Debug.Log("Inside Get Info Display text 1");
			StringBuilder builder = new StringBuilder();
			Debug.Log("Inside Get Info Display text 2");
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
			foreach (var effect in equipmentEffects)
			{
				
				builder.Append("+").Append(effect.Value).Append(" ").Append(effect.StatType.Name).AppendLine();
			}
						
			//builder.Append("<color=green>Use: ").Append(useText).Append("</color>").AppendLine();
			builder.Append("Max Stack: ").Append(MaxStack).AppendLine();
			builder.Append("Sell Price: ").Append(SellPrice).Append(" Gold").AppendLine();
			builder.Append('"').Append(flavourText).Append('"');

			Debug.Log("Inside Get Info Display text5");
			return builder.ToString();
		}

		public void Unequip(GramophoneUtils.Characters.Character character)
		{
			foreach (KeyValuePair<IStatType, IStat> entry in character.StatSystem.Stats)
			{
				entry.Value.RemoveAllModifiersFromSource(this);
			}
		}

		public override void Use(GramophoneUtils.Characters.Character character, InventorySlotUI inventorySlotUI)
		{
			character.EquipmentInventory.TryToEquip(this, inventorySlotUI);
		}
	}
}