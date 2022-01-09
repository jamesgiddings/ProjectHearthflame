using UnityEngine.EventSystems;

namespace GramophoneUtils.Items.Hotbars
{
    public class HotbarItemDragHandler : ResourceDragHandler
    {
        public override void OnPointerUp(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                base.OnPointerUp(eventData);

                if (eventData.hovered.Count == 0)
                {
                    (ResourceSlotUI as HotbarSlotUI).SlotResource = null;
                }
            }
        }
    }
}
