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
        [SerializeField] private Inventory _itemContainer;

        public string Name => "Would you like to see my wares?";

        private void Start()
        {
            if (_itemContainer == null)
			{
                _itemContainer.onInventoryItemsUpdated = onInventoryItemsUpdated;
                for (int i = 0; i < vendorInventory.Inventory.IResourceSlots.Length; i++)
				{
                    _itemContainer.IResourceSlots[i] = vendorInventory.Inventory.IResourceSlots[i];
                }
			}
        }
        

        public void Trigger(GameObject other)
        {
            var playerBehaviour = other.GetComponent<CharacterGameObjectManager>();
            if (playerBehaviour == null) { return; }

            var otherItemContainer = ServiceLocator.Instance.CharacterModel.PartyInventory;

            VendorData vendorData = new VendorData(otherItemContainer, _itemContainer);
            Debug.Log(vendorData.BuyingItemContainer.GetAllUnique().Count + " + " + vendorData.SellingItemContainer.GetAllUnique().Count);
            onStartVendorScenario.Raise(vendorData);
        }

		# region SavingLoading
		public object CaptureState()
		{
            return _itemContainer.CaptureState();
        }

		public void RestoreState(object state)
		{
            _itemContainer.RestoreState(state);
            onInventoryItemsUpdated.Invoke();

        }
		#endregion
	}
}
