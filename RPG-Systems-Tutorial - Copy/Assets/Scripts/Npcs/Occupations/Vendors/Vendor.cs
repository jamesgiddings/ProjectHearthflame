using GramophoneUtils.Events.CustomEvents;
using GramophoneUtils.Items;
using UnityEngine;

namespace GramophoneUtils.Npcs.Occupations.Vendors
{
    public class Vendor : MonoBehaviour, IOccupation
    {
        [SerializeField] private VendorDataEvent onStartVendorScenario = null;

        public string Name => "Would you like to see my wares?";

        private IItemContainer itemContainer = null;

        private void Start() => itemContainer = GetComponent<IItemContainer>();

        public void Trigger(GameObject other)
        {
            var otherItemContainer = other.GetComponent<IItemContainer>();

            if (otherItemContainer == null) { return; }

            VendorData vendorData = new VendorData(otherItemContainer, itemContainer);

            onStartVendorScenario.Raise(vendorData);
        }
    }
}
