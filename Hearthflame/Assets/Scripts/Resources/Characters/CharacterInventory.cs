using GramophoneUtils.Characters;
using GramophoneUtils.Events.CustomEvents;
using GramophoneUtils.Items.Containers;
using GramophoneUtils.SavingLoading;
using GramophoneUtils.Stats;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CharacterInventory : CharacterContainer, ISaveable
{
    #region API
    public CharacterInventory(VoidEvent onCharactersUpdated, int size = 8)
	{
		if (onCharactersUpdated != null)
		{
			this.onCharactersUpdated = onCharactersUpdated;
		}
		characterSlots = new CharacterSlot[size];
	}

	#endregion

	#region SavingLoading
	public object CaptureState()
	{
		List<CharacterSlotUI.ResourceSlotSaveData> tempList = new List<CharacterSlotUI.ResourceSlotSaveData>();
		for (int i = 0; i < CharacterSlots.Length; i++)
		{
			if (CharacterSlots[i].Character != null)
			{
				tempList.Add(new ResourceSlotUI.ResourceSlotSaveData(CharacterSlots[i].Character.UID, GetSlotByIndex(i).Quantity));
			}
			else
			{
				tempList.Add(new ResourceSlotUI.ResourceSlotSaveData("", 0));
			}
		}

		return new CharacterInventorySaveData
		{
			characterSlotSaveData = tempList
		};
	}

	public void RestoreState(object state)
	{
		var saveData = (CharacterInventorySaveData)state;

		for (int i = 0; i < CharacterSlots.Length; i++)
		{
			if (saveData.characterSlotSaveData[i].resourceUID != null)
			{
				Resource resource = GameManager.Instance.ResourceDatabase.GetResourceByUID(saveData.characterSlotSaveData[i].resourceUID);
				CharacterSlots[i].Character = (Character)resource;
				CharacterSlots[i].Quantity = saveData.characterSlotSaveData[i].resourceQuantity;
			}
		}
		onCharactersUpdated?.Raise();
	}

	[Serializable]
	public struct CharacterInventorySaveData
	{
		public List<CharacterSlotUI.ResourceSlotSaveData> characterSlotSaveData;
	}
	#endregion
}
