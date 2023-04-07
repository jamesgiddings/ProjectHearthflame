using System.Collections.Generic;
using GramophoneUtils.Items.Containers;

namespace GramophoneUtils.Items
{
    public interface IItemContainer
    {
        int Currency { get; set; }
        ItemSlot[] IResourceSlots { get; }
        ItemSlot GetSlotByIndex(int index);
        ItemSlot Add(ItemSlot itemSlot);
        List<InventoryItem> GetAllUnique();
        void Remove(ItemSlot itemSlot);
        void RemoveAt(int slotIndex);
        void Swap(IItemContainer itemContainerOne, int indexOne, IItemContainer itemContainerTwo, int indexTwo);
        bool Has(InventoryItem item);
        int GetTotalQuantity(InventoryItem item);
    }
}
