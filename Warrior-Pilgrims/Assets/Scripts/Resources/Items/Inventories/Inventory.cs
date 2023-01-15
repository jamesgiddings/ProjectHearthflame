using GramophoneUtils.SavingLoading;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace GramophoneUtils.Items.Containers
{
	[Serializable, CreateAssetMenu(fileName = "Inventory", menuName = "Containers/Inventory")]
    public class Inventory : ItemContainer, ISaveable
	{
		#region Attributes/Fields/Properties

		[SerializeField] private ItemSlot[] _startingItemSlots;

		#endregion

		#region Callbacks

		private void OnEnable()
		{
			if(itemSlots == null) { return; }

			for (int i = 0; i < itemSlots.Length; i++)
			{
				if(i < _startingItemSlots.Length)
				{
					itemSlots[i] = _startingItemSlots[i];

				}
				else
				{
					itemSlots[i].item = null;
                }
			}
		}

		#endregion

		public Inventory(int size = 20, int money = 0, UnityEvent onInventoryItemsUpdated = null)
			{
				if (onInventoryItemsUpdated != null)
				{
					this.onInventoryItemsUpdated = onInventoryItemsUpdated;
				}
				itemSlots = new ItemSlot[size];
				this.money = money;
			}

		#region SavingLoading
		
		public object CaptureState()
		{
			//Debug.Log("We are in Inventory.CaptureState()");
			//GameManager.Instance.ResourceDatabase.Print();
			List <ResourceSlotUI.ResourceSlotSaveData> tempList = new List<ResourceSlotUI.ResourceSlotSaveData>();
			for (int i = 0; i < ItemSlots.Length; i++)
			{
				if(ItemSlots[i].item != null)
				{
					//Debug.Log("We are saving:  + " + ItemSlots[i].item.Name);
					tempList.Add(new ResourceSlotUI.ResourceSlotSaveData(ItemSlots[i].item.UID, GetSlotByIndex(i).quantity));
				}
				else
				{
					//Debug.Log("This item slot is empty.");
					tempList.Add(new ResourceSlotUI.ResourceSlotSaveData("", 0));
				}
			}
			
			return new InventorySaveData
			{
				money = money,
				itemSlotSaveData = tempList
			};
		}

		public void RestoreState(object state)
		{
			var saveData = (InventorySaveData)state;
			//GameManager.Instance.ResourceDatabase.Print();
			money = saveData.money;
			for (int i = 0; i < ItemSlots.Length; i++)
			{

				if (saveData.itemSlotSaveData[i].resourceUID != null)
				{
					Resource resource = GameManager.Instance.ResourceDatabase.GetResourceByUID(saveData.itemSlotSaveData[i].resourceUID);
					ItemSlots[i].item = (InventoryItem)resource;
					ItemSlots[i].quantity = saveData.itemSlotSaveData[i].resourceQuantity;
				}
			}
			onInventoryItemsUpdated?.Invoke();
		}

		[Serializable]
		public struct InventorySaveData
		{
			public int money;
			public List<ResourceSlotUI.ResourceSlotSaveData> itemSlotSaveData;
		}
		#endregion
	}
}
