using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using GramophoneUtils.SavingLoading;
using System;

public class SaveSlotUI : MonoBehaviour
{
	[SerializeField] private Button saveButton;
	[SerializeField] private Button loadButton;
	[SerializeField] private Button deleteButton;
	[SerializeField] private TextMeshProUGUI infoText; 

	[SerializeField] private string emptySlotText = "Empty Slot";

	private string fileName;
	public Action<string> onSaveClicked;
	public Action<string> onLoadClicked;
	public Action<string> onDeleteClicked;

	public void Initialize(string fileName)
	{
		RemoveListeners();
		this.fileName = fileName;
		saveButton.onClick.AddListener(OnSaveClicked);
		loadButton.onClick.AddListener(OnLoadClicked);
		deleteButton.onClick.AddListener(OnDeleteClicked);
	}

	public void SetLoadButtonActive(bool value)
	{
		loadButton.interactable = value;
	}

	public void SetDeleteButtonActive(bool value)
	{
		deleteButton.interactable = value;
	}

	public void SetInfoText(string value)
	{
		infoText.text = value;
	}

	private void OnSaveClicked()
	{
		onSaveClicked?.Invoke(fileName);
		Initialize(fileName);
	}

	private void OnLoadClicked()
	{
		onLoadClicked?.Invoke(fileName);
	}

	private void OnDeleteClicked()
	{
		onDeleteClicked?.Invoke(fileName);

		SetDeleteButtonActive(false); // this sets the buttons to false until the next time the window is enabled again, which will detect the file's
		SetLoadButtonActive(false);
		SetInfoText(emptySlotText);
	}

	private void RemoveListeners()
	{
		saveButton.onClick.RemoveAllListeners();
		loadButton.onClick.RemoveAllListeners();
		deleteButton.onClick.RemoveAllListeners();
	}

	private void OnDestroy()
	{
		RemoveListeners();
	}
}
