using GramophoneUtils.Items.Hotbars;
using GramophoneUtils.Items.Containers;
using System.Text;
using UnityEngine;
using GramophoneUtils.Stats;

namespace GramophoneUtils.Items
{
    [CreateAssetMenu(fileName = "New Consumable Item", menuName = "Items/Consumable Item")]
    public class ConsumableItem : InventoryItem, IHotbarItem
    {
        [Header("Consumable Data")]
        [SerializeField] private string useText = "Does something, maybe?";

        public override string GetInfoDisplayText()
        {
            StringBuilder builder = new StringBuilder();

            builder.Append(Rarity.Name).AppendLine();
            builder.Append("<color=green>Use: ").Append(useText).Append("</color>").AppendLine();
            builder.Append("Max Stack: ").Append(MaxStack).AppendLine();
            builder.Append("Sell Price: ").Append(SellPrice).Append(" Gold");

            return builder.ToString();
        }

		public override void Use(CharacterBehaviour characterBehaviour, InventorySlotUI inventorySlotUI)
		{
            Debug.Log($"Drinking {Name}");
            // Do use code here
        }
	}
}
