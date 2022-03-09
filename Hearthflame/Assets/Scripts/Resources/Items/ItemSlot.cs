using GramophoneUtils.Items.Containers;
using System;
using UnityEngine;

namespace GramophoneUtils.Items
{
    [Serializable]
    public struct ItemSlot
    {
        public InventoryItem item;
        [MinAttribute(1)]
        public int quantity;

        public ItemSlot(InventoryItem item, int quantity)
        {
            this.item = item;
            this.quantity = quantity;
        }
    }
}
