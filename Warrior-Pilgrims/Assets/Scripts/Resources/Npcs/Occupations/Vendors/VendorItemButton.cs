using GramophoneUtils.Events.CustomEvents;
using GramophoneUtils.Items.Containers;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace GramophoneUtils.Npcs.Occupations.Vendors
{
    public class VendorItemButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private TextMeshProUGUI itemNameText = null;
        [SerializeField] private Image itemIconImage = null;
        [SerializeField] private ResourceEvent resourceEvent;
        [SerializeField] private VoidEvent voidEvent;

        private VendorSystem vendorSystem = null;
        private InventoryItem item = null;

        public void Initialise(VendorSystem vendorSystem, InventoryItem item, int quantity)
        {
            this.vendorSystem = vendorSystem;
            this.item = item;

            itemNameText.text = $"{item.Name} ({quantity})";
            itemIconImage.sprite = item.Sprite;
        }

		public void SelectItem()
        {
            vendorSystem.SetItem(item);
        }
        public void OnPointerEnter(PointerEventData eventData)
        {
            resourceEvent?.Raise(item);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            voidEvent?.Raise();
        }
    }
}