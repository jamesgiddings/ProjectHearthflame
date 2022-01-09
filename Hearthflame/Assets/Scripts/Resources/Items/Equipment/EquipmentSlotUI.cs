using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GramophoneUtils.Items.Containers
{
    public class EquipmentSlotUI : InventorySlotUI
    {
        [SerializeField] private EquipmentType equipmentType;

		//public void OnValidate()
		//{
		//	if (inventory as EquipmentInventory == null)
		//	{
  //              Debug.LogWarning("EquipmentSlot requires a reference to an EquipmentInventory, not just an inventory.");
		//	}
		//}

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
            if (resourceDragHandler == null) { return; }

            EquipmentItem item = GetItemFromItemSlotUI(resourceDragHandler.ResourceSlotUI);
            EquipmentInventory equipmentInventory = (EquipmentInventory)inventory;

            if (CanEquipToSlot(item) && CanEquipToClass(item, equipmentInventory))
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

        private void EquipFromInventory()
        {
            EquipmentInventory equipmentInventory = (EquipmentInventory)inventory;
            EquipmentItem item = (EquipmentItem)inventory.GetSlotByIndex(SlotIndex).item;
            item.Equip(equipmentInventory.CharacterBehaviour.Character);
        }
        private void UnequipFromInventory(IItemContainer itemContainer, int index)
        {
            EquipmentInventory equipmentInventory = (EquipmentInventory)inventory;
            IEquippable item = (EquipmentItem)itemContainer.GetSlotByIndex(index).item;
            item.Unequip(equipmentInventory.CharacterBehaviour.Character);
        }
    }
}
