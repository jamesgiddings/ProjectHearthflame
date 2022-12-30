using UnityEngine;

namespace GramophoneUtils.Items.Hotbars
{
    public interface IHotbarItem
    {
        string Name { get; }
        Sprite Sprite { get; }
    }
}

