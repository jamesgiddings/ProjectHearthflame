using GramophoneUtils.SavingLoading;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace GramophoneUtils.Items.Containers
{
	[Serializable]
	public class Inventory : ItemContainer, ISaveable
	{
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
			List<ResourceSlotUI.ResourceSlotSaveData> tempList = new List<ResourceSlotUI.ResourceSlotSaveData>();
			for (int i = 0; i < ItemSlots.Length; i++)
			{
				if(ItemSlots[i].item != null)
				{
					tempList.Add(new ResourceSlotUI.ResourceSlotSaveData(ItemSlots[i].item.UID, GetSlotByIndex(i).quantity));
				}
				else
				{
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

			money = saveData.money;
			for (int i = 0; i < ItemSlots.Length; i++)
			{
				if (saveData.itemSlotSaveData[i].resourceUID != null)
				{
					Resource resource = ResourceDatabase.GetResourceByUID(saveData.itemSlotSaveData[i].resourceUID);
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
