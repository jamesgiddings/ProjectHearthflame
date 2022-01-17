using GramophoneUtils.Interactables;
using UnityEngine;

namespace GramophoneUtils.Items
{
    public class ItemPickup : MonoBehaviour, IInteractable
    {
        [SerializeField] private ItemSlot[] itemSlots;

        public void Interact(GameObject other)
        {
			foreach (ItemSlot itemSlot in itemSlots)
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
}