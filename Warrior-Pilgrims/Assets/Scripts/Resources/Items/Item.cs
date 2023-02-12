using GramophoneUtils.Characters;
using GramophoneUtils.Items.Containers;
using UnityEngine;

namespace GramophoneUtils.Items
{
    public abstract class Item : Resource
    {
        [Header("Basic Info")]
        [SerializeField] private string description = "New Item Description";

        public string Description => description;

        public abstract string ColouredName { get; }

        public abstract void Use(Character character, InventorySlotUI inventorySlotUI);
    }
}
