using GramophoneUtils.Stats;
using GramophoneUtils.Items.Containers;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using AYellowpaper;
using GramophoneUtils.Characters;

namespace GramophoneUtils.Items
{
	[CreateAssetMenu(fileName = "New Equipment Item", menuName = "Items/Equipment Item")]
	public class EquipmentItem : InventoryItem, IEquippable
	{
		[Header("Equipment Data")]
		[SerializeField] private EquipmentType equipmentType;
		[SerializeField] private bool isUnique = false;
		[SerializeField] private string flavourText = "Flavour text here.";
		[SerializeField] private List<InterfaceReference<IStatModifierBlueprint>> _weaponEffectBlueprints = new List<InterfaceReference<IStatModifierBlueprint>>();
        [SerializeField] private CharacterClass[] classRestrictions;

		private List<IStatModifier> equipmentEffects { get { return InstanceEquipmentEffectBlueprints(); } }

		public CharacterClass[] ClassRestrictions => classRestrictions;

		public EquipmentType EquipmentType => equipmentType;

		private List<IStatModifier> InstanceEquipmentEffectBlueprints()
		{
			List<IStatModifier> effects = new List<IStatModifier>();
			if (_weaponEffectBlueprints.Count > 0)
			{
				foreach (var blueprint in _weaponEffectBlueprints)
				{
					effects.Add(ServiceLocatorObject.Instance.StatModifierFactory.CreateStatModifierFromBlueprint(blueprint.Value, new object[] { this }));
				}
			}
			return effects;
		}

		public void Equip(Character character)
		{
			foreach (IStatModifier weaponEffect in equipmentEffects)
			{   // TODO, this could be tidier
				character.StatSystem.AddModifier(
					ServiceLocatorObject.Instance.StatModifierFactory.CreateStatModifierFromValue( // we make a new effect, which allows us to add the source as 'this' when we equip the item
						weaponEffect.Name,
						weaponEffect.UID,
						weaponEffect.Sprite,
						weaponEffect.StatType,
						weaponEffect.ModifierNumericType,
						weaponEffect.StatModifierType,
						weaponEffect.Value,
                        new object[] { this }
                        ));
			}
		}
		public override string GetInfoDisplayText()
		{
            StringBuilder builder = new StringBuilder();
            builder.Append(ServiceLocatorObject.Instance.UIConstants.TOOLTIP_TITLE_SIZE_OPEN_TAG)
				.Append(ColouredName)
				.Append(ServiceLocatorObject.Instance.UIConstants.TOOLTIP_SIZE_CLOSE_TAG)
				.AppendLine();
			builder.Append(Rarity.Name).AppendLine();
			if (classRestrictions.Length > 0)
			{
				for (int i = 0; i < classRestrictions.Length; i++)
				{
					if (i < classRestrictions.Length - 1)
					{
						builder.Append(classRestrictions[i].GetInfoDisplayText());
						builder.Append(", ");
					}
					else
					{
                        builder.Append(classRestrictions[i].GetInfoDisplayText());
                        builder.Append(" only.")
                            .Append(ServiceLocatorObject.Instance.UIConstants.TOOLTIP_COLOUR_CLOSE_TAG)
							.AppendLine();
					}
				}
			}
			foreach (var effect in equipmentEffects)
			{
				
				builder
					.Append(effect.GetInfoDisplayText())
					.AppendLine();
			}
						
			//builder.Append("<color=green>Use: ").Append(useText).Append("</color>").AppendLine();
			builder.Append("Max Stack: ").Append(MaxStack).AppendLine();
			builder.Append("Sell Price: ").Append(SellPrice).Append(" Gold").AppendLine();
			builder
				.Append(ServiceLocatorObject.Instance.UIConstants.TOOLTIP_FLAVOUR_TEXT_OPEN_TAG)
				.Append(flavourText)
				.Append(ServiceLocatorObject.Instance.UIConstants.TOOLTIP_FLAVOUR_TEXT_CLOSE_TAG);

			return builder.ToString();
		}

		public void Unequip(Character character)
		{
			foreach (KeyValuePair<IStatType, IStat> entry in character.StatSystem.Stats)
			{
				entry.Value.RemoveAllModifiersFromSources(new object[] { this });
			}
		}

		public override void Use(Character character, InventorySlotUI inventorySlotUI)
		{
			character.EquipmentInventory.TryToEquip(this, inventorySlotUI);
		}
	}
}