using UnityEngine;
using UnityEngine.EventSystems;

namespace GramophoneUtils.Items.Containers
{
    public class InventoryItemDragHandler : ResourceDragHandler
    {
        [SerializeField] private ItemDestroyer itemDestroyer = null;

        public override void OnPointerUp(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                base.OnPointerUp(eventData);

                if (eventData.hovered.Count == 0)
                {
                    InventorySlotUI thisSlot = ResourceSlotUI as InventorySlotUI;
                    itemDestroyer.Activate(thisSlot.ItemSlot, thisSlot.SlotIndex);
                }
            }
        }
    }
}
