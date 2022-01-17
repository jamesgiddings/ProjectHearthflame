using UnityEngine;
using GramophoneUtils.Stats;
using System.Collections.Generic;
using System;

namespace GramophoneUtils.Items.Containers
{
	[Serializable]
	public class EquipmentInventory : Inventory
    {
        [SerializeField] private CharacterBehaviour characterBehaviour;
		public CharacterBehaviour CharacterBehaviour { get => characterBehaviour; }
		public Dictionary<EquipmentType, int> equipmentTypeToSlotIndex;

		private void Start()
		{
			equipmentTypeToSlotIndex = new Dictionary<EquipmentType, int>()
			{
				{ EquipmentType.Armor, 0 },
				{ EquipmentType.Weapon, 1 },
				{ EquipmentType.Trinket, 2 },
			};
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
			Debug.Log("we're doing refreshing");
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

		public bool CanEquipToClass(EquipmentItem equipmentItem, EquipmentInventory equipmentInventory)
		{
			if (equipmentItem == null)
			{
				Debug.Log("Item null");
				return true;
			}
			if (equipmentItem.ClassRestrictions.Length == 0)
			{
				return true;
			}
			else
			{
				if (!Array.Exists(equipmentItem.ClassRestrictions, characterClass => characterClass.Name == equipmentInventory.CharacterBehaviour.Character.CharacterClass.Name))
				{
					return false;
				}
				else
				{
					return true;
				}
			}
		}

		public bool CanEquipToSlot(EquipmentItem equipmentItem, EquipmentType equipmentType)
		{
			if (equipmentItem != null)
			{
				if (equipmentItem.EquipmentType == equipmentType)
				{
					return true;
				}
			}
			return false;
		}

		public void TryToEquip(EquipmentItem equipmentItem, InventorySlotUI originalSlot)
		{
			if (CanEquipToClass(equipmentItem, this))
			{
				if (equipmentItem.EquipmentType == EquipmentType.Trinket && GetSlotByIndex(2).item != null)
				{
					if (GetSlotByIndex(3).item == null)
					{
						Swap(CharacterBehaviour.gameObject.GetComponent<Inventory>(), originalSlot.SlotIndex, this, 3);

					}
				}
				else
				{
					Swap(CharacterBehaviour.gameObject.GetComponent<Inventory>(), originalSlot.SlotIndex, this, equipmentTypeToSlotIndex[equipmentItem.EquipmentType]);

				}
				RefreshEquipmentInSlots();
			}
		}
	}
}