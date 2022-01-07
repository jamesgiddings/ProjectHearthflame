using GramophoneUtils.Interactables;
using UnityEngine;

namespace GramophoneUtils.Items
{
    public class ItemPickup : MonoBehaviour, IInteractable
    {
        [SerializeField] private ItemSlot itemSlot;

        public void Interact(GameObject other)
        {
            var itemContainer = other.GetComponent<IItemContainer>();

            if (itemContainer == null) { return; }

            if (itemContainer.AddItem(itemSlot).quantity == 0)
            {
                Destroy(gameObject);
            }
        }
    }
}