using GramophoneUtils.Items;
using GramophoneUtils.Stats;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabPanelUI : MonoBehaviour
{
	[SerializeField] Transform tabParent;
	[SerializeField] Transform tabContentParent;
	[SerializeField] GameObject tabPrefab;
	[SerializeField] GameObject tabContentPrefab;
	[SerializeField] ItemDestroyer itemDestroyer;
	
	[SerializeField] PlayerBehaviour playerBehaviour;


	private void OnEnable()
	{
		tabContentParent.gameObject.SetActive(false);
		foreach (PartyCharacter partyCharacter in playerBehaviour.Party.PartyCharacters)
		{
			if (partyCharacter.IsUnlocked)
			{
				GameObject tabContentUI = Instantiate(tabContentPrefab, tabContentParent);
				tabContentUI.GetComponent<TabContentUI>().Initialise(playerBehaviour.Party, partyCharacter.Character, itemDestroyer);
				SetupTab(tabContentUI, partyCharacter.Character);
			}
		}
		tabContentParent.gameObject.SetActive(true);
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
