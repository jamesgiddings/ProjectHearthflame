using GramophoneUtils.Stats;
using GramophoneUtils.Items.Containers;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Sirenix.OdinInspector;
using AYellowpaper;

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

		public void Equip(GramophoneUtils.Characters.Character character)
		{
			foreach (IStatModifier weaponEffect in equipmentEffects)
			{   // TODO, this could be tidier
				character.StatSystem.AddModifier(
					ServiceLocatorObject.Instance.StatModifierFactory.CreateStatModifierFromValue( // we make a new effect, which allows us to add the source as 'this' when we equip the item
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
				entry.Value.RemoveAllModifiersFromSources(new object[] { this });
			}
		}

		public override void Use(GramophoneUtils.Characters.Character character, InventorySlotUI inventorySlotUI)
		{
			character.EquipmentInventory.TryToEquip(this, inventorySlotUI);
		}
	}
}