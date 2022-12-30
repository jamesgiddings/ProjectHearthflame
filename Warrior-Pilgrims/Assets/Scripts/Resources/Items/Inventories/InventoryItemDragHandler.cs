using UnityEngine;
using UnityEngine.EventSystems;

namespace GramophoneUtils.Items.Containers
{
    public class InventoryItemDragHandler : ResourceDragHandler
    {
        [SerializeField] private ItemDestroyer itemDestroyer = null;

        public ItemDestroyer ItemDestroyer
        {
            get
            {
                return itemDestroyer;
            }
            set
            {
                itemDestroyer = value;
            }
        }

        public override void OnPointerUp(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                base.OnPointerUp(eventData);

                if (eventData.hovered.Count == 0 && (ResourceSlotUI as EquipmentSlotUI == null))
                {
                    InventorySlotUI thisSlot = ResourceSlotUI as InventorySlotUI;
                    itemDestroyer.Activate(thisSlot.ItemSlot, thisSlot.SlotIndex);
                }
            }
        }
    }
}
