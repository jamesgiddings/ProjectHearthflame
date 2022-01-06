using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEditor;
using GramophoneUtils.Stats;

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

		public bool CanEquipToSlot(ItemSlotUI itemSlot)
		{
            if ((itemSlot as InventorySlot) != null)
            {
                if (itemSlot.SlotItem is EquipmentItem)
                {
                    EquipmentItem equipmentItem = (EquipmentItem)itemSlot.SlotItem;
                    if (equipmentItem.EquipmentType == equipmentType)
                    {
                        return true;
                    }
                }
            }
            return false;
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
            
            if (CanEquipToSlot(itemDragHandler.ItemSlotUI)) 
            {
                inventory.Swap(itemContainerOne, itemDragHandler.ItemSlotUI.SlotIndex, inventory, SlotIndex);
                if (inventory as EquipmentInventory != null)
                {
                    if((itemContainerOne.GetSlotByIndex(itemDragHandler.ItemSlotUI.SlotIndex).item != null) && 
                        (inventory.GetSlotByIndex(SlotIndex).item != null))
					{
                        UnequipFromInventory(itemContainerOne, itemDragHandler.ItemSlotUI.SlotIndex);
                    }
                    EquipFromInventory();
                }
            }
        }

        private void EquipFromInventory()
        {
            EquipmentInventory equipmentInventory = (EquipmentInventory)inventory;
            EquipmentItem item = (EquipmentItem)inventory.GetSlotByIndex(SlotIndex).item;
            Debug.Log(item.Name);
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
