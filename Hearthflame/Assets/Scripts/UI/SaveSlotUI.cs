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
	[SerializeField] private TextMeshProUGUI infoText;

	private string fileName;
	public Action<string> onSaveClicked;
	public Action<string> onLoadClicked;

	public void Initialize(string fileName)
	{
		RemoveListeners();
		Debug.Log("Initializing");
		this.fileName = fileName;
		saveButton.onClick.AddListener(OnSaveClicked);
		loadButton.onClick.AddListener(OnLoadClicked);
	}

	public void SetLoadButtonActive(bool value)
	{
		loadButton.interactable = value;
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

	private void RemoveListeners()
	{
		saveButton.onClick.RemoveAllListeners();
		loadButton.onClick.RemoveAllListeners();
	}

	private void OnDestroy()
	{
		RemoveListeners();
	}
}
