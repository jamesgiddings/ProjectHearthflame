using UnityEngine;

namespace GramophoneUtils.Items.Hotbars
{
    public class Hotbar : MonoBehaviour
    {
        [SerializeField] private HotbarSlotUI[] hotbarSlots = new HotbarSlotUI[10];

        public void Add(Item itemToAdd)
        {
            foreach (HotbarSlotUI hotbarSlot in hotbarSlots)
            {
                if (hotbarSlot.AddResource(itemToAdd)) { return; }
            }
        }
    }
}
