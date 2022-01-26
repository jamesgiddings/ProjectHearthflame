using GramophoneUtils.Stats; // for testing
using System;
using TMPro;
using UnityEditor; // for testing
using UnityEngine;
using UnityEngine.EventSystems;

namespace GramophoneUtils.Items.Containers
{
    public class InventorySlotUI : ResourceSlotUI, IDropHandler
    {
        [SerializeField] protected Inventory inventory;
        [SerializeField] private TextMeshProUGUI itemQuantityText;
        [SerializeField] protected PlayerBehaviour playerBehaviour;

        private Character character;

        public override Resource SlotResource
        {
            get { return ItemSlot.item; }
            set { }
        }

		public virtual ItemSlot ItemSlot => inventory.GetSlotByIndex(SlotIndex);
        public Inventory Inventory => inventory;
        public PlayerBehaviour CharacterBehaviour => playerBehaviour;
        public Character Character => character;

        public virtual void Initialise(PlayerBehaviour playerBehaviour, Character character, ItemDestroyer itemDestroyer)
		{
            this.playerBehaviour = playerBehaviour;
            this.inventory = playerBehaviour.PartyInventory;
            this.character = character;
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
            if (resourceDragHandler == null) { return; }

            if ((resourceDragHandler.ResourceSlotUI as InventorySlotUI) != null)
            {
                inventory.Swap(itemContainerOne, resourceDragHandler.ResourceSlotUI.SlotIndex, inventory, SlotIndex);
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
            item.Unequip(equipmentInventory.Character);
        }

        public override void UpdateSlotUI()
        {
            if (ItemSlot.item == null)
            {
                EnableSlotUI(false);
                return;
            }
            EnableSlotUI(true);
            resourceIconImage.sprite = ItemSlot.item.Icon;
            itemQuantityText.text = ItemSlot.quantity > 1 ? ItemSlot.quantity.ToString() : "";
        }

        protected override void EnableSlotUI(bool enable)
        {
            base.EnableSlotUI(enable);
            itemQuantityText.enabled = enable;
        }
    }
}
