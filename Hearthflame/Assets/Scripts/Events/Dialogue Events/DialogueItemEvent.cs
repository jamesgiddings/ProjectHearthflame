using GramophoneUtils.Characters;
using GramophoneUtils.Items.Containers;
using UnityEngine;

namespace GramophoneUtils.Events.CustomEvents
{
    [CreateAssetMenu(fileName = "New Dialogue Item Event", menuName = "Game Events/Dialogue Item Event")]
    public class DialogueItemEvent : Resource
    {
		[SerializeField] InventoryItem inventoryItem;
		[SerializeField] int quantity;
		[SerializeField] Character character;
		public void AddItem()
		{
			Debug.Log("Add Item: " + inventoryItem.name + " x " + quantity);

			//character.PartyInventory.AddItem(new ItemSlot(inventoryItem, quantity));
		}

		public bool IsTrue()
		{
			return true;
		}
    }
}

