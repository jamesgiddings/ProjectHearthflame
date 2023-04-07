using GramophoneUtils.SavingLoading;
using System;
using System.Collections.Generic;
using UnityEngine;

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
				ResetSlotToStartingState(i);
			}
		}

		#endregion

        #region Private Functions

        private void ResetSlotToStartingState(int i)
        {
			if(_startingItemSlots != null)
			{
                if (i < _startingItemSlots.Length)
                {
                    itemSlots[i] = _startingItemSlots[i];
                }
				else
				{
                    itemSlots[i].item = null;
                }
            }
            else
            {
                itemSlots[i].item = null;
            }
        }

        #endregion

        #region SavingLoading

        public object CaptureState()
		{
			//Debug.Log("We are in Inventory.CaptureState()");
			//GameManager.Instance.ResourceDatabase.Print();
			List <ResourceSlotUI.ResourceSlotSaveData> tempList = new List<ResourceSlotUI.ResourceSlotSaveData>();
			for (int i = 0; i < IResourceSlots.Length; i++)
			{
				if(IResourceSlots[i].item != null)
				{
					//Debug.Log("We are saving:  + " + ItemSlots[i].item.Name);
					tempList.Add(new ResourceSlotUI.ResourceSlotSaveData(IResourceSlots[i].item.UID, GetSlotByIndex(i).quantity));
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
			for (int i = 0; i < IResourceSlots.Length; i++)
			{

				if (saveData.itemSlotSaveData[i].resourceUID != null)
				{
					Resource resource = GameManager.Instance.ResourceDatabase.GetResourceByUID(saveData.itemSlotSaveData[i].resourceUID);
					IResourceSlots[i].item = (InventoryItem)resource;
					IResourceSlots[i].quantity = saveData.itemSlotSaveData[i].resourceQuantity;
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
