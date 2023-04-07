using GramophoneUtils.Characters;
using GramophoneUtils.Items.Containers;
using UnityEngine;

namespace GramophoneUtils.Items
{
    public abstract class Item : Resource
    {
        [Header("Basic Info")]
        [SerializeField] private string _description = "New Item Description";

        public string Description => _description;

        public abstract string ColouredName { get; }

        public abstract void Use(ICharacter character, InventorySlotUI inventorySlotUI);
    }
}
