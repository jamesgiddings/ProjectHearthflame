using GramophoneUtils.Stats; // for testing
using System;
using TMPro;
using UnityEditor; // for testing
using UnityEngine;
using UnityEngine.EventSystems;

namespace GramophoneUtils.Items.Containers
{
    public class InventorySlot : ItemSlotUI, IDropHandler
    {
        [SerializeField] protected Inventory inventory;
        [SerializeField] private TextMeshProUGUI itemQuantityText;
        public override Item SlotItem
        {
            get { return ItemSlot.item; }
            set { }
        }

        public ItemSlot ItemSlot => inventory.GetSlotByIndex(SlotIndex);
        public Inventory Inventory => inventory;

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

            if ((itemDragHandler.ItemSlotUI as InventorySlot) != null)
            {
                inventory.Swap(itemContainerOne, itemDragHandler.ItemSlotUI.SlotIndex, inventory, SlotIndex);
                if (itemContainerOne as EquipmentInventory != null)
                {
                    EquipmentInventory equipmentInventory = (EquipmentInventory)itemContainerOne;
                    UnequipFromInventory(equipmentInventory, SlotIndex);
                }
			}
        }

        private void UnequipFromInventory(EquipmentInventory equipmentInventory, int index)
		{
            IEquippable item = (EquipmentItem)inventory.GetSlotByIndex(index).item;
            item.Unequip(equipmentInventory.StatSystemBehaviour.StatSystem);
        }

		public override void UpdateSlotUI()
        {
            if (ItemSlot.item == null)
            {
                EnableSlotUI(false);
                return;
            }

            EnableSlotUI(true);
            itemIconImage.sprite = ItemSlot.item.Icon;
            itemQuantityText.text = ItemSlot.quantity > 1 ? ItemSlot.quantity.ToString() : "";
        }

        protected override void EnableSlotUI(bool enable)
        {
            base.EnableSlotUI(enable);
            itemQuantityText.enabled = enable;
        }
    }
}
