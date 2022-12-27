using GramophoneUtils.Stats;
using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GramophoneUtils.Items.Containers
{
    public class EquipmentSlotUI : InventorySlotUI
    {
        [SerializeField] private EquipmentType equipmentType;

		public override void Initialise(GramophoneUtils.Characters.Character character, ItemDestroyer itemDestroyer)
		{
            inventory = character.EquipmentInventory;
            InventoryItemDragHandler dragHandler = transform.GetChild(0).gameObject.GetComponent<InventoryItemDragHandler>();
            if (dragHandler != null)
			{
                dragHandler.ItemDestroyer = itemDestroyer;
			}
        }

		public override void OnDrop(PointerEventData eventData)
        {
            IItemContainer itemContainerOne;
            ResourceDragHandler resourceDragHandler = eventData.pointerDrag.GetComponent<ResourceDragHandler>();
            if (resourceDragHandler.ResourceSlotUI as InventorySlotUI != null) // if the other ItemSlotUI is actually an InventorySlot // bug when you drag in the vendor window with inventories open
            {
                InventorySlotUI inventorySlot = (InventorySlotUI)resourceDragHandler.ResourceSlotUI;
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
            if (resourceDragHandler == null) { 
                Debug.Log("resourceDragHandler is null");
                return; }

            EquipmentItem item = GetItemFromItemSlotUI(resourceDragHandler.ResourceSlotUI);
            EquipmentInventory equipmentInventory = (EquipmentInventory)inventory;

            if (equipmentInventory.CanEquipToSlot(item, equipmentType) && equipmentInventory.CanEquipToClass(item, equipmentInventory))
            {
                inventory.Swap(itemContainerOne, resourceDragHandler.ResourceSlotUI.SlotIndex, inventory, SlotIndex);
            }
        }

		private EquipmentItem GetItemFromItemSlotUI(ResourceSlotUI itemSlot)
		{
            if (itemSlot as InventorySlotUI != null)
            {
                if (itemSlot.SlotResource is EquipmentItem)
                {
                    return (EquipmentItem)itemSlot.SlotResource;
                }
            }
            return null;
        }

        private void EquipFromInventory()
        {
            EquipmentInventory equipmentInventory = (EquipmentInventory)inventory;
            EquipmentItem item = (EquipmentItem)inventory.GetSlotByIndex(SlotIndex).item;
            item.Equip(equipmentInventory.Character);
        }
        private void UnequipFromInventory(IItemContainer itemContainer, int index)
        {
            EquipmentInventory equipmentInventory = (EquipmentInventory)inventory;
            IEquippable item = (EquipmentItem)itemContainer.GetSlotByIndex(index).item;
            item.Unequip(equipmentInventory.Character);
        }
    }
}
