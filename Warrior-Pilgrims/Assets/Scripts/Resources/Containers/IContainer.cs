using System.Collections.Generic;

namespace GramophoneUtils.Containers
{
    interface IContainer
    {
        int Currency { get; set; }
        IResourceSlot[] IResourceSlots { get; }
        IResourceSlot GetSlotByIndex(int index);
        bool CanAdd(IResourceSlot iResourceSlot);
        IResourceSlot Add(IResourceSlot iResourceSlot);
        bool CanAddAt(int slotIndex, IResourceSlot iResourceSlot);
        IResourceSlot AddAt(int slotIndex, IResourceSlot iResourceSlot);
        bool Has(IResource iResource);
        void Remove(IResourceSlot itemSlot);
        void RemoveAt(int slotIndex);
        bool CanSwap(IContainer iResourceContainerOne, int indexOne, IContainer iResourceContainerTwo, int indexTwo);
        void Swap(IContainer iResourceContainerOne, int indexOne, IContainer iResourceContainerTwo, int indexTwo);
        int GetTotalQuantity(IResource iResource);
        List<IResource> GetAllUnique();
    }
}
