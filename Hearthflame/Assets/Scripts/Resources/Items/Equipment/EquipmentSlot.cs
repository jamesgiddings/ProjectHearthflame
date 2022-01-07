using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GramophoneUtils.Items.Containers
{
    
    public class EquipmentSlot : InventorySlot
    {
        [SerializeField] private EquipmentType equipmentType;

		public void OnValidate()
		{
			if (inventory as EquipmentInventory == null)
			{
                Debug.LogWarning("EquipmentSlot requires a reference to an EquipmentInventory, not just an inventory.");
			}
		}

        public override void OnDrop(PointerEventData eventData)
        {
            IItemContainer itemContainerOne;
            ItemDragHandler itemDragHandler = eventData.pointerDrag.GetComponent<ItemDragHandler>();
            if (itemDragHandler.ItemSlotUI as InventorySlot != null) // if the other ItemSlotUI is actually an InventorySlot // bug when you drag in the vendor window with inventories open
            {
                InventorySlot inventorySlot = (InventorySlot)itemDragHandler.ItemSlotUI;
                itemContainerOne = inventorySlot.Inventory; // get its associated inventory
            }
            else
            {
                itemContainerOne = inventory; // if not, then just use the inventory associated with this slot
            }
            if (itemContainerOne == null)
            {
                Debug.LogError("itemContainerOne is null.");
            }
            if (inventory == null)
            {
                Debug.LogError("inventory is null.");
            }
            if (itemDragHandler == null) { return; }

            EquipmentItem item = GetItemFromItemSlotUI(itemDragHandler.ItemSlotUI);
            EquipmentInventory equipmentInventory = (EquipmentInventory)inventory;

            if (CanEquipToSlot(item) && CanEquipToClass(item, equipmentInventory))
            {
                inventory.Swap(itemContainerOne, itemDragHandler.ItemSlotUI.SlotIndex, inventory, SlotIndex);
            }
        }

		private EquipmentItem GetItemFromItemSlotUI(ItemSlotUI itemSlot)
		{
            if (itemSlot as InventorySlot != null)
            {
                if (itemSlot.SlotItem is EquipmentItem)
                {
                    return (EquipmentItem)itemSlot.SlotItem;
                }
            }
            return null;
        }

		public bool CanEquipToSlot(EquipmentItem equipmentItem)
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
            
        private bool CanEquipToClass(EquipmentItem equipmentItem, EquipmentInventory equipmentInventory)
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
                if (!Array.Exists(equipmentItem.ClassRestrictions, characterClass => characterClass.Name == equipmentInventory.StatSystemBehaviour.StatSystem.CharacterClass.Name))
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        private void EquipFromInventory()
        {
            EquipmentInventory equipmentInventory = (EquipmentInventory)inventory;
            EquipmentItem item = (EquipmentItem)inventory.GetSlotByIndex(SlotIndex).item;
            item.Equip(equipmentInventory.StatSystemBehaviour.StatSystem);
        }
        private void UnequipFromInventory(IItemContainer itemContainer, int index)
        {
            EquipmentInventory equipmentInventory = (EquipmentInventory)inventory;
            IEquippable item = (EquipmentItem)itemContainer.GetSlotByIndex(index).item;
            item.Unequip(equipmentInventory.StatSystemBehaviour.StatSystem);
        }
    }
}
