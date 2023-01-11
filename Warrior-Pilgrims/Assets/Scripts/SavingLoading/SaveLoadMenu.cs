using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;
using GramophoneUtils.SavingLoading;

public class SaveLoadMenu : MonoBehaviour
{
	[SerializeField] string[] slotFileNames;
	[SerializeField] Transform saveSlotParent;
	[SerializeField] GameObject saveSlotPrefab;

	private Dictionary<string, SaveSlotUI> saveSlotDictionary = new Dictionary<string, SaveSlotUI>();

	private void Awake()
	{
		foreach (string slotFileName in slotFileNames)
		{
			GameObject saveSlotObject = Instantiate(saveSlotPrefab, saveSlotParent);
			SaveSlotUI saveSlotUI = saveSlotObject.GetComponent<SaveSlotUI>();
			saveSlotDictionary.Add(slotFileName, saveSlotUI);
			saveSlotDictionary[slotFileName].Initialize(slotFileName);
		}
	}

	private void OnEnable()
	{
		ConnectSlots();
	}

	private void ConnectSlots(string arg = null)
	{
		foreach (string slotFileName in slotFileNames)
		{
			Debug.Log(saveSlotDictionary[slotFileName]);
			SaveSlotUI saveSlotUI = saveSlotDictionary[slotFileName];
			saveSlotUI.onSaveClicked += ServiceLocator.Instance.SavingSystem.Save;
			saveSlotUI.onSaveClicked += ConnectSlots;
			Debug.Log(SavingSystem.GetPathFromName(slotFileName));
			if (File.Exists(SavingSystem.GetPathFromName(slotFileName)))
			{
				saveSlotDictionary[slotFileName].SetLoadButtonActive(true);
				saveSlotDictionary[slotFileName].SetDeleteButtonActive(true);
				saveSlotDictionary[slotFileName].SetInfoText(slotFileName);
				saveSlotDictionary[slotFileName].onLoadClicked += ServiceLocator.Instance.SavingSystem.Load;
				saveSlotDictionary[slotFileName].onDeleteClicked += ServiceLocator.Instance.SavingSystem.Delete;
			}
			else
			{
				saveSlotDictionary[slotFileName].SetLoadButtonActive(false);
				saveSlotDictionary[slotFileName].SetDeleteButtonActive(false);
			}
		}
	}

	private void OnDisable()
	{
		foreach (string slotFileName in slotFileNames)
		{
			saveSlotDictionary[slotFileName].onSaveClicked -= ServiceLocator.Instance.SavingSystem.Save;
			if (File.Exists(SavingSystem.GetPathFromName(slotFileName)))
			{
				saveSlotDictionary[slotFileName].onLoadClicked -= ServiceLocator.Instance.SavingSystem.Load;
				saveSlotDictionary[slotFileName].onDeleteClicked -= ServiceLocator.Instance.SavingSystem.Delete;
			}
		}
	}
}
