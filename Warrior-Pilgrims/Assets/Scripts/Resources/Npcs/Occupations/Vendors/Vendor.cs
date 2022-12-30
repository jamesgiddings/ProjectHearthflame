using GramophoneUtils.Events.CustomEvents;
using GramophoneUtils.Items;
using GramophoneUtils.Items.Containers;
using GramophoneUtils.SavingLoading;
using GramophoneUtils.Stats;
using UnityEngine;
using UnityEngine.Events;

namespace GramophoneUtils.Npcs.Occupations.Vendors
{
    public class Vendor : MonoBehaviour, IOccupation, ISaveable
    {
        [SerializeField] private VendorDataEvent onStartVendorScenario;
        [SerializeField] private UnityEvent onInventoryItemsUpdated;
        [SerializeField] private VendorInventory vendorInventory;

        public string Name => "Would you like to see my wares?";

        private Inventory itemContainer = null;

        private void Start()
        {
            if (itemContainer == null)
			{
                itemContainer = new Inventory(vendorInventory.Inventory.ItemSlots.Length, vendorInventory.Inventory.Money);
                itemContainer.onInventoryItemsUpdated = onInventoryItemsUpdated;
                for (int i = 0; i < vendorInventory.Inventory.ItemSlots.Length; i++)
				{
                    itemContainer.ItemSlots[i] = vendorInventory.Inventory.ItemSlots[i];
                }
			}
        }
        

        public void Trigger(GameObject other)
        {
            var playerBehaviour = other.GetComponent<CharacterGameObjectManager>();
            if (playerBehaviour == null) { return; }

            var otherItemContainer = ServiceLocator.Instance.CharacterModel.PartyInventory;

            VendorData vendorData = new VendorData(otherItemContainer, itemContainer);
            Debug.Log(vendorData.BuyingItemContainer.GetAllUniqueItems().Count + " + " + vendorData.SellingItemContainer.GetAllUniqueItems().Count);
            onStartVendorScenario.Raise(vendorData);
        }

		# region SavingLoading
		public object CaptureState()
		{
            return itemContainer.CaptureState();
        }

		public void RestoreState(object state)
		{
            itemContainer.RestoreState(state);
            onInventoryItemsUpdated.Invoke();

        }
		#endregion
	}
}
