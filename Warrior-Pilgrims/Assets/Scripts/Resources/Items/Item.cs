using AYellowpaper;
using GramophoneUtils.Items.Containers;
using GramophoneUtils.Stats;
using UnityEngine;

namespace GramophoneUtils.Items
{

    public abstract class Item : Resource
    {
        [Header("Basic Info")]
        [SerializeField] private string description = "New Item Description";

        public string Description => description;
        public abstract string ColouredName { get; }

        public abstract string GetInfoDisplayText();

        public abstract void Use(GramophoneUtils.Characters.Character character, InventorySlotUI inventorySlotUI);
    }
}
