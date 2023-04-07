using GramophoneUtils.Items.Containers;
using GramophoneUtils.Stats;
using System.Text;
using UnityEngine;

namespace GramophoneUtils.Items
{
    [CreateAssetMenu(fileName = "New Ammunition", menuName = "Items/Ammunition")]
    public class Ammunition : InventoryItem
    {
        [SerializeField] private GameObject ammunitionPrefab = null;

        public GameObject AmmunitionPrefab => ammunitionPrefab;

        public override string GetInfoDisplayText()
        {
            StringBuilder builder = new StringBuilder();

            builder.Append(Rarity.Name).AppendLine();
            builder.Append("Max Stack: ").Append(MaxStack).AppendLine();
            builder.Append("Sell Price: ").Append(SellPrice).Append(" Gold");

            return builder.ToString();
        }

		public override void Use(ICharacter character, InventorySlotUI inventorySlotUI)
		{
			throw new System.NotImplementedException();
		}
	}
}
