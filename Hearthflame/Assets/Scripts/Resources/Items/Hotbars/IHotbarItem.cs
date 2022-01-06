using UnityEngine;

namespace GramophoneUtils.Items.Hotbars
{
    public interface IHotbarItem
    {
        string Name { get; }
        Sprite Icon { get; }
        void Use();
    }
}

