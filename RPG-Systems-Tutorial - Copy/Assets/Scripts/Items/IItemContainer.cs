using System.Collections.Generic;
using GramophoneUtils.Items.Containers;

namespace GramophoneUtils.Items
{
    public interface IItemContainer
    {
        int Money { get; set; }
        ItemSlot[] ItemSlots { get; }
        ItemSlot GetSlotByIndex(int index);
        ItemSlot AddItem(ItemSlot itemSlot);
        List<InventoryItem> GetAllUniqueItems();
        void RemoveItem(ItemSlot itemSlot);
        void RemoveAt(int slotIndex);
        void Swap(IItemContainer itemContainerOne, int indexOne, IItemContainer itemContainerTwo, int indexTwo);
        bool HasItem(InventoryItem item);
        int GetTotalQuantity(InventoryItem item);
    }
}
