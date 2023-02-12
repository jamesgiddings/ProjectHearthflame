using GramophoneUtils.Items.Hotbars;
using GramophoneUtils.Items.Containers;
using System.Text;
using UnityEngine;
using System.Collections.Generic;
using AYellowpaper;
using GramophoneUtils.Characters;

namespace GramophoneUtils.Items
{
    [CreateAssetMenu(fileName = "New Consumable Item", menuName = "Items/Consumable Item")]
    public class ConsumableItem : InventoryItem, IHotbarItem
    {
        [Header("Consumable Data")]
        [SerializeField] private string useText = "Does something, maybe?";
        
        [SerializeField] private CharacterClass[] classRestrictions;

        [SerializeField] private InterfaceReference<ISkill> _skill;
        public ISkill Skill => _skill.Value;

        public override string GetInfoDisplayText()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(ServiceLocatorObject.Instance.UIConstants.TOOLTIP_TITLE_SIZE_OPEN_TAG)
                .Append(ColouredName)
                .Append(ServiceLocatorObject.Instance.UIConstants.TOOLTIP_SIZE_CLOSE_TAG)
                .AppendLine();
            builder.Append(Rarity.Name).AppendLine();
            builder.Append("<color=green>Use: ").Append(useText).Append("</color>").AppendLine();
            builder.Append("Max Stack: ").Append(MaxStack).AppendLine();
            builder.Append("Sell Price: ").Append(SellPrice).Append(" Gold");

            return builder.ToString();
        }

		public override void Use(Character character, InventorySlotUI inventorySlotUI)
		{
            Debug.Log($"Drinking {Name}");
            // Do use code here
        }
	}
}
