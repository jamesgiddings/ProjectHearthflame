using GramophoneUtils.Items.Containers;
using GramophoneUtils.Stats;
using TMPro;
using UnityEngine;

namespace GramophoneUtils.Items
{
    public class ItemDestroyer : MonoBehaviour
    {
        [SerializeField] private CharacterModel playerModel = null;
        [SerializeField] private TextMeshProUGUI areYouSureText = null;

        private int slotIndex = 0;

        private void OnDisable() => slotIndex = -1;

        public void Activate(ItemSlot itemSlot, int slotIndex)
        {
            this.slotIndex = slotIndex;
            areYouSureText.text = $"Are you sure you wish to destroy {itemSlot.quantity}x {itemSlot.item.ColouredName}?";

            gameObject.SetActive(true);
        }

        public void Destroy()
        {
            playerModel.PartyInventory.RemoveAt(slotIndex);

            gameObject.SetActive(false);
        }
    }
}
