using UnityEngine;
using GramophoneUtils.Stats;
using System.Collections.Generic;
using System;
using UnityEngine.Events;
using GramophoneUtils.Characters;

namespace GramophoneUtils.Items.Containers
{
	[Serializable]
	public class EquipmentInventory : Inventory
    {
		private Character character;
		public Character Character => character;

		public Action Refresh;

		public Dictionary<EquipmentType, int> equipmentTypeToSlotIndex;
		public EquipmentInventory(Character character, int size = 4, int money = 0)
		{
			this.character = character;
			itemSlots = new ItemSlot[size];
			this.money = money;
			equipmentTypeToSlotIndex = new Dictionary<EquipmentType, int>()
			{
				{ EquipmentType.Armor, 0 },
				{ EquipmentType.Weapon, 1 },
				{ EquipmentType.Trinket, 2 },
			};
			if (character.PartyInventory != null)
			{
				ConnectToCharacter(character);
			}
		}
        #region Callbacks
        private void OnDestroy()
		{
			if (character.PartyInventory != null)
				onInventoryItemsUpdated.RemoveListener(RefreshEquipmentInSlots);
		}
		#endregion

		#region API

		public void ConnectToCharacter(Character character)
		{
            this.onInventoryItemsUpdated = character.PartyInventory.onInventoryItemsUpdated;
            this.onInventoryItemsUpdated.AddListener(RefreshEquipmentInSlots);
        }

		public void RefreshEquipmentInSlots()
		{
			foreach(ItemSlot itemSlot in ItemSlots)
			{
				IEquippable equippable = (EquipmentItem)itemSlot.item;
				if (equippable != null)
				{
					equippable.Unequip(character);
					equippable.Equip(character);
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
				if (!Array.Exists(equipmentItem.ClassRestrictions, characterClass => characterClass.Name == equipmentInventory.Character.CharacterClass.Name))
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
						Swap(Character.PartyInventory, originalSlot.SlotIndex, this, 3);
					}
					else
					{
						Swap(Character.PartyInventory, originalSlot.SlotIndex, this, 2);
					}
				}
				else
				{
					Swap(Character.PartyInventory, originalSlot.SlotIndex, this, equipmentTypeToSlotIndex[equipmentItem.EquipmentType]);
				}

                RefreshEquipmentInSlots();
			}
		}

        #endregion

        #region Utilities

        private void EquipItemInSlot(ItemSlot itemSlot)
        {
            if (itemSlot.item as EquipmentItem != null)
            {
                IEquippable equippable = (EquipmentItem)itemSlot.item;
                equippable.Equip(character);
            }
        }

        private void UnequipItemFromSlot(ItemSlot itemSlot)
        {
            if (itemSlot.item as EquipmentItem != null)
            {
                IEquippable equippable = (EquipmentItem)itemSlot.item;
                equippable.Unequip(character);
            }
        }

        #endregion
    }

}