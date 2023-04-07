using UnityEngine;
using GramophoneUtils.Stats;
using System.Collections.Generic;
using System;
using UnityEngine.Events;
using GramophoneUtils.Characters;
using Sirenix.OdinInspector;

namespace GramophoneUtils.Items.Containers
{
	[Serializable, CreateAssetMenu(fileName = "Equipment Inventory", menuName = "Containers/Equipment Inventory")]
	public class EquipmentInventory : Inventory
    {
		private ICharacter character;
		public ICharacter Character => character;

		public Action Refresh;

		public Dictionary<EquipmentType, int> equipmentTypeToSlotIndex;

        #region Callbacks
        private void OnDestroy()
		{
			if (character.PartyInventory != null)
				onInventoryItemsUpdated.RemoveListener(RefreshEquipmentInSlots);
		}
		#endregion

		#region Public Functions

		public void Initialise(int size = 4)
		{
            
            itemSlots = new ItemSlot[size];
            equipmentTypeToSlotIndex = new Dictionary<EquipmentType, int>()
            {
                { EquipmentType.Armor, 0 },
                { EquipmentType.Weapon, 1 },
                { EquipmentType.Trinket, 2 },
            };
        }

		public void ConnectToCharacter(ICharacter character, Inventory _partyInventory)
		{
            this.character = character;
            this.onInventoryItemsUpdated = _partyInventory.onInventoryItemsUpdated;
            this.onInventoryItemsUpdated.AddListener(RefreshEquipmentInSlots);
        }

		public void RefreshEquipmentInSlots()
		{
			foreach(ItemSlot itemSlot in IResourceSlots)
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