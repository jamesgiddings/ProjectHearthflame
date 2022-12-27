using GramophoneUtils.Characters;
using GramophoneUtils.Items;
using GramophoneUtils.Stats;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabPanelUI : MonoBehaviour
{
	[SerializeField] private Transform tabParent;
	[SerializeField] private Transform tabContentParent;
	[SerializeField] private GameObject tabPrefab;
	[SerializeField] private GameObject tabContentPrefab;
	[SerializeField] private ItemDestroyer itemDestroyer;

	private void OnEnable()
	{
		tabContentParent.gameObject.SetActive(false);
		if (ServiceLocator.Instance.CharacterModel != null)
		{
			foreach (Character character in ServiceLocator.Instance.CharacterModel.PlayerCharacters)
			{
				if (character != null)
				{
					if (character.IsUnlocked)
					{
						GameObject tabContentUI = Instantiate(tabContentPrefab, tabContentParent);
						tabContentUI.GetComponent<TabContentUI>().Initialise(character, itemDestroyer);
						SetupTab(tabContentUI, character);
					}
				}
			}
			tabContentParent.gameObject.SetActive(true);
		}
		else
		{
			Debug.LogWarning("No player found for UI to display.");
		}
	}

	private void OnDisable()
	{
		for (int i = 0; i < tabContentParent.childCount; i++)
		{
			Destroy(tabContentParent.gameObject.transform.GetChild(i).gameObject);
			Destroy(tabParent.gameObject.transform.GetChild(i).gameObject);
		}
	}

	private void SetupTab(GameObject tabContentUI, Character character)
	{
		GameObject tab = Instantiate(tabPrefab, tabParent);
		Toggle toggle = tab.GetComponent<Toggle>();
		toggle.group = tabParent.GetComponent<ToggleGroup>();
		tab.GetComponent<TabUI>().SetTabText(character.Name);
		toggle.onValueChanged.AddListener(delegate
		{
			ToggleValueChanged(toggle, tabContentUI);
		}
		);
	}

	private void ToggleValueChanged(Toggle toggle, GameObject tab)
	{
		tab.SetActive(toggle.isOn);
	}
}
