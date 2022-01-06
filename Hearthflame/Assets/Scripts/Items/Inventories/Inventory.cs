using GramophoneUtils.SavingLoading;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace GramophoneUtils.Items.Containers
{
	public class Inventory : ItemContainer, ISaveable
	{ 
		#region SavingLoading
		public object CaptureState()
		{
			List<ItemSlotUI.ItemSlotSaveData> tempList = new List<ItemSlotUI.ItemSlotSaveData>();
			for (int i = 0; i < ItemSlots.Length; i++)
			{
				if(ItemSlots[i].item != null)
				{
					tempList.Add(new ItemSlotUI.ItemSlotSaveData(ItemSlots[i].item.UID, GetSlotByIndex(i).quantity));
				}
				else
				{
					tempList.Add(new ItemSlotUI.ItemSlotSaveData("", 0));
				}
			}
			
				return new SaveData
			{
				money = money,
				itemSlotSaveData = tempList
			};
		}

		public void RestoreState(object state)
		{
			var saveData = (SaveData)state;

			money = saveData.money;
			for (int i = 0; i < ItemSlots.Length; i++)
			{
				if (saveData.itemSlotSaveData[i].itemUID != null)
				{
					Resource resource = ResourceDatabase.GetResourceByUID(saveData.itemSlotSaveData[i].itemUID);
					ItemSlots[i].item = (InventoryItem)resource;
					ItemSlots[i].quantity = saveData.itemSlotSaveData[i].itemQuantity;
				}
			}
			onInventoryItemsUpdated?.Invoke();
		}

		[Serializable]
		private struct SaveData
		{
			public int money;
			public List<ItemSlotUI.ItemSlotSaveData> itemSlotSaveData;
		}
		#endregion
	}
}
