﻿using UnityEngine;

namespace GramophoneUtils.Items
{
    public abstract class Item : Resource
    {
        [Header("Basic Info")]
        [SerializeField] private new string name = "New Item Name";
        [SerializeField] private string description = "New Item Description";
        [SerializeField] private Sprite icon = null;

        public string Name => name;
        public string Description => description;
        public abstract string ColouredName { get; }
        public Sprite Icon => icon;

        public abstract string GetInfoDisplayText();
    }
}
