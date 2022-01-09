using GramophoneUtils.Items.Containers;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GramophoneUtils.Items.Hotbars
{
    public class HotbarSlotUI : ResourceSlotUI, IDropHandler
    {
        [SerializeField] private Inventory inventory = null;
        [SerializeField] private TextMeshProUGUI itemQuantityText = null;

        private Resource slotResource = null;

        public override Resource SlotResource
        {
            get { return slotResource; }
            set { slotResource = value; UpdateSlotUI(); }
        }

        public bool AddResource(Resource resourceToAdd)
        {
            if (SlotResource != null) { return false; }

            SlotResource = resourceToAdd;

            return true;
        }

        public void UseSlot(int index)
        {
            if (index != SlotIndex) { return; }

            //Use item
        }

        public override void OnDrop(PointerEventData eventData)
        {
            ResourceDragHandler resourceDragHandler = eventData.pointerDrag.GetComponent<ResourceDragHandler>();
            SkillDragHandler skillDragHandler = eventData.pointerDrag.GetComponent<SkillDragHandler>();
            if (resourceDragHandler == null) { return; }
            if (!(resourceDragHandler == null))
			{
                InventorySlotUI inventorySlot = resourceDragHandler.ResourceSlotUI as InventorySlotUI;
                if (inventorySlot != null)
                {
                    Debug.Log(inventorySlot.ItemSlot.item.Name);
                    SlotResource = inventorySlot.ItemSlot.item;
                    UpdateSlotUI();
                    Debug.Log(SlotResource.Name);
                    return;
                }

                HotbarSlotUI hotbarSlot = resourceDragHandler.ResourceSlotUI as HotbarSlotUI;
                if (hotbarSlot != null)
                {
                    Debug.Log(resourceDragHandler.ResourceSlotUI.SlotResource);
                    Resource oldResource = SlotResource;
                    SlotResource = hotbarSlot.SlotResource;
                    hotbarSlot.SlotResource = oldResource;
                    UpdateSlotUI();
                    return;
                }

                SkillSlotUI skillSlot = skillDragHandler.ResourceSlotUI as SkillSlotUI;
                if (skillSlot != null)
                {
                    Debug.Log(skillSlot.SlotResource.Name);
                    SlotResource = skillSlot.SlotResource;
                    UpdateSlotUI();
                    Debug.Log(SlotResource.Name);
                    return;
                }

            }
        }

        public override void UpdateSlotUI()
        {
            if (SlotResource == null)
            {
				EnableSlotUI(false);
                return;
            }
            Debug.Log("UPDATING UI");
			resourceIconImage.sprite = SlotResource.Icon;

            EnableSlotUI(true);

            SetItemQuantityUI();
        }

        private void SetItemQuantityUI()
        {
            if (SlotResource is InventoryItem inventoryItem)
            {
                if (inventory.HasItem(inventoryItem))
                {
                    int quantityCount = inventory.GetTotalQuantity(inventoryItem);
					itemQuantityText.text = quantityCount > 1 ? quantityCount.ToString() : "";
                }
                else
                {
                    SlotResource = null;
                }
            }
            else
            {
				itemQuantityText.enabled = false;
            }
        }
        protected override void EnableSlotUI(bool enable)
        {
            resourceIconImage.enabled = enable;
            itemQuantityText.enabled = enable;
        }
    }
}
