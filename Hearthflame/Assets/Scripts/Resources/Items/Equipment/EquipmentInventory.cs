using UnityEngine;
using GramophoneUtils.Stats;

namespace GramophoneUtils.Items.Containers
{
	public class EquipmentInventory : Inventory
    {
        [SerializeField] private CharacterBehaviour characterBehaviour;
		public CharacterBehaviour CharacterBehaviour { get => characterBehaviour; }

		private void Start()
		{
			onInventoryItemsUpdated.AddListener(RefreshEquipmentInSlots);
		}		

		private void OnDestroy()
		{
			onInventoryItemsUpdated.RemoveListener(RefreshEquipmentInSlots);
		}

		private void EquipItemInSlot(ItemSlot itemSlot)
		{
			if (itemSlot.item as EquipmentItem != null)
			{
				IEquippable equippable = (EquipmentItem)itemSlot.item;
				equippable.Equip(characterBehaviour.Character);
			}
		}

		private void UnequipItemFromSlot(ItemSlot itemSlot)
		{
			if (itemSlot.item as EquipmentItem != null)
			{
				IEquippable equippable = (EquipmentItem)itemSlot.item;
				equippable.Unequip(characterBehaviour.Character);
			}
		}

		private void RefreshEquipmentInSlots()
		{
			foreach(ItemSlot itemSlot in ItemSlots)
			{
				IEquippable equippable = (EquipmentItem)itemSlot.item;
				if (equippable != null)
				{
					equippable.Unequip(characterBehaviour.Character);
					equippable.Equip(characterBehaviour.Character);
				}
			}
		}
	}
}