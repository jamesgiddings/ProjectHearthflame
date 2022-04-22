using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;
using GramophoneUtils.SavingLoading;

public class SaveLoadMenu : MonoBehaviour
{
	[SerializeField] SavingSystem savingSystem;
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
			saveSlotUI.onSaveClicked += savingSystem.Save;
			saveSlotUI.onSaveClicked += ConnectSlots;
			Debug.Log(SavingSystem.GetPathFromName(slotFileName));
			if (File.Exists(SavingSystem.GetPathFromName(slotFileName)))
			{
				saveSlotDictionary[slotFileName].SetLoadButtonActive(true);
				saveSlotDictionary[slotFileName].SetInfoText(slotFileName);
				saveSlotDictionary[slotFileName].onLoadClicked += savingSystem.Load;
			}
			else
			{
				saveSlotDictionary[slotFileName].SetLoadButtonActive(false);
			}
		}
	}

	private void OnDisable()
	{
		foreach (string slotFileName in slotFileNames)
		{
			saveSlotDictionary[slotFileName].onSaveClicked -= savingSystem.Save;
			if (File.Exists(SavingSystem.GetPathFromName(slotFileName)))
			{
				Debug.Log("Exists.");
				saveSlotDictionary[slotFileName].onLoadClicked -= savingSystem.Load;
			}
		}
	}
	//    private string slot1 = "Slot1";
	//    private string slot2 = "Slot2";
	//    private string slot3 = "Slot3";

	//    private string savePath = "not set";

	//    private string filePath;

	//    string saveFileName = "SaveData";
	//    private string SavePath => $"{Application.persistentDataPath}" + "/" + "{saveFileName}.sav";

	//    [SerializeField] GameObject inputField;

	//    private void Start()
	//    {
	//        filePath = Application.persistentDataPath;

	//        DirectoryInfo dir = new DirectoryInfo(filePath);
	//        FileInfo[] info = dir.GetFiles("*.*");

	//        foreach (FileInfo f in info)
	//        {
	//            Debug.Log(f.ToString());
	//        }

	//        Debug.Log(SavePath);



	//}


	//	public void Save()
	//    {

	//        savePath = inputField.GetComponent<TMP_InputField>().text;
	//        Debug.Log(savePath);
	//    }
}
