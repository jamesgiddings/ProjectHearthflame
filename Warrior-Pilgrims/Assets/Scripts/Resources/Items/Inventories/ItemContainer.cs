using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace GramophoneUtils.Items.Containers
{    

    public class ItemContainer : ScriptableObject, IItemContainer
    {
        [SerializeField] protected int money = 100;
        [SerializeField] protected ItemSlot[] itemSlots = new ItemSlot[0];

        public UnityEvent onInventoryItemsUpdated;

        public ItemSlot[] ItemSlots { get => itemSlots; }
        public int Money { get { return money; } set { money = value; } }

        public ItemSlot GetSlotByIndex(int index) 
        {
            if (index < 0 || index >= itemSlots.Length)
            {
                throw new Exception(index + " is out of range for item container: " + this.ToString());
            }
            return itemSlots[index];
        }

        public virtual ItemSlot AddItem(ItemSlot itemSlot)
        {
            for (int i = 0; i < itemSlots.Length; i++)
            {
                if (itemSlots[i].item != null)
                {
                    if (itemSlots[i].item == itemSlot.item)
                    {
                        int slotRemainingSpace = itemSlots[i].item.MaxStack - itemSlots[i].quantity;

                        if (itemSlot.quantity <= slotRemainingSpace)
                        {
                            itemSlots[i].quantity += itemSlot.quantity;

                            itemSlot.quantity = 0;

                            onInventoryItemsUpdated.Invoke();

                            return itemSlot;
                        }
                        else if (slotRemainingSpace > 0)
                        {
                            itemSlots[i].quantity += slotRemainingSpace;

                            itemSlot.quantity -= slotRemainingSpace;
                        }
                    }
                }
            }

            for (int i = 0; i < itemSlots.Length; i++)
            {
                if (itemSlots[i].item == null)
                {
                    if (itemSlot.quantity <= itemSlot.item.MaxStack)
                    {
                        itemSlots[i] = itemSlot;
                        itemSlot.quantity = 0;

                        onInventoryItemsUpdated.Invoke();

                        return itemSlot;
                    }
                    else
                    {
                        itemSlots[i] = new ItemSlot(itemSlot.item, itemSlot.item.MaxStack);

                        itemSlot.quantity -= itemSlot.item.MaxStack;
                    }
                }
            }
            onInventoryItemsUpdated.Invoke();
            return itemSlot;
        }

        public virtual void RemoveItem(ItemSlot itemSlot)
        {
            for (int i = 0; i < itemSlots.Length; i++)
            {
                if (itemSlots[i].item != null)
                {
                    if (itemSlots[i].item == itemSlot.item)
                    {
                        if (itemSlots[i].quantity < itemSlot.quantity)
                        {
                            itemSlot.quantity -= itemSlots[i].quantity;

                            itemSlots[i] = new ItemSlot();

                            onInventoryItemsUpdated.Invoke();
                        }
                        else
                        {
                            itemSlots[i].quantity -= itemSlot.quantity;

                            if (itemSlots[i].quantity == 0)
                            {
                                itemSlots[i] = new ItemSlot();

                                onInventoryItemsUpdated.Invoke();

                                return;
                            }
                        }
                    }
                }
            }
        }

        public List<InventoryItem> GetAllUniqueItems()
        {
            List<InventoryItem> items = new List<InventoryItem>();

            for (int i = 0; i < itemSlots.Length; i++)
            {
                if (itemSlots[i].item == null) { continue; }

                if (items.Contains(itemSlots[i].item)) { continue; }

                items.Add(itemSlots[i].item);
            }

            return items;
        }

        public virtual void RemoveAt(int slotIndex)
        {
            if (slotIndex < 0 || slotIndex > itemSlots.Length - 1) { return; }

            itemSlots[slotIndex] = new ItemSlot();

            onInventoryItemsUpdated.Invoke();
        }

        public virtual void Swap(IItemContainer itemContainerOne, int indexOne, IItemContainer itemContainerTwo, int indexTwo)
        {
            ItemSlot firstSlot = itemContainerOne.ItemSlots[indexOne];
            ItemSlot secondSlot = itemContainerTwo.ItemSlots[indexTwo];

            if (firstSlot.Equals(secondSlot)) { return; }

            if (secondSlot.item != null && firstSlot.item != null)
            {
                if (firstSlot.item == secondSlot.item)
                {
                    int secondSlotRemainingSpace = secondSlot.item.MaxStack - secondSlot.quantity;

                    if (firstSlot.quantity <= secondSlotRemainingSpace)
                    {
                        itemContainerTwo.ItemSlots[indexTwo].quantity += firstSlot.quantity;

                        itemContainerOne.ItemSlots[indexOne] = new ItemSlot();

                        onInventoryItemsUpdated.Invoke();

                        return;
                    }
                }
            }

            itemContainerOne.ItemSlots[indexOne] = secondSlot;
            itemContainerTwo.ItemSlots[indexTwo] = firstSlot;

            if (itemContainerOne as EquipmentInventory != null)
            {
                EquipmentInventory equipmentInventory = itemContainerOne as EquipmentInventory;
                EquipmentItem equipmentItemOne = equipmentInventory.GetSlotByIndex(indexOne).item as EquipmentItem;
                if (equipmentItemOne != null)
                {
                    equipmentItemOne.Equip(equipmentInventory.Character);
                }
                EquipmentItem equipmentItemTwo = itemContainerTwo.GetSlotByIndex(indexTwo).item as EquipmentItem;
                if (equipmentItemTwo != null)
                {
                    equipmentItemTwo.Unequip(equipmentInventory.Character);
                }

            }
            else if (itemContainerTwo as EquipmentInventory != null)
            {
                EquipmentInventory equipmentInventory = itemContainerTwo as EquipmentInventory;
                EquipmentItem equipmentItemOne = equipmentInventory.GetSlotByIndex(indexTwo).item as EquipmentItem;
                if (equipmentItemOne != null)
                {
                    equipmentItemOne.Equip(equipmentInventory.Character);
                }
                EquipmentItem equipmentItemTwo = itemContainerOne.GetSlotByIndex(indexOne).item as EquipmentItem;
                if (equipmentItemTwo != null)
                {
                    equipmentItemTwo.Unequip(equipmentInventory.Character);
                }

            }
            onInventoryItemsUpdated.Invoke();
        }

        public virtual bool HasItem(InventoryItem item)
        {
            foreach (ItemSlot itemSlot in itemSlots)
            {
                if (itemSlot.item == null) { continue; }
                if (itemSlot.item != item) { continue; }

                return true;
            }

            return false;
        }

        public virtual int GetTotalQuantity(InventoryItem item)
        {
            int totalCount = 0;

            foreach (ItemSlot itemSlot in itemSlots)
            {
                if (itemSlot.item == null) { continue; }
                if (itemSlot.item != item) { continue; }

                totalCount += itemSlot.quantity;
            }

            return totalCount;
        }
    }
}

