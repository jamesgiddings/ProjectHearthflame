﻿using GramophoneUtils.Items.Containers;
using UnityEngine;
using UnityEngine.Events;

namespace GramophoneUtils.Stats
{
	public class Party
	{
	//	[SerializeField] public UnityEvent onStatsChanged;
	//	[SerializeField] public UnityEvent onInventoryItemsUpdated;
	
	//	private Inventory partyInventory = new Inventory(20, 10000);
	//	private int partyInventorySize;
	//	private int startingScrip;

	//	[SerializeField] private PartyCharactersTemplateObject partyCharactersTemplateObject;

	//	private PartyCharacter[] partyCharacters;

	//	public Inventory PartyInventory => partyInventory; //getter

	//	public PartyCharacter[] PartyCharacters
	//	{
	//		get
	//		{
	//			if (partyCharacters != null) { return partyCharacters; }
	//			partyCharacters = new PartyCharacter[partyCharactersTemplateObject.PartyCharacterTemplates.Length];
	//			for (int i = 0; i < partyCharactersTemplateObject.PartyCharacterTemplates.Length; i++)
	//			{
	//				PartyCharacterTemplate partyCharacterTemplate = partyCharactersTemplateObject.PartyCharacterTemplates[i];
	//				partyCharacters[i] = new PartyCharacter(partyCharacterTemplate, this);
	//			}
	//			return partyCharacters;
	//		}
	//	}

	//	public Party(
	//		PartyCharactersTemplateObject partyCharactersTemplateObject,
	//		UnityEvent onStatsChanged,
	//		UnityEvent onInventoryItemsUpdated,
	//		int partyInventorySize,
	//		int startingScrip
	//		) //constructor 
	//	{
	//		this.partyCharactersTemplateObject = partyCharactersTemplateObject;
	//		this.onStatsChanged = onStatsChanged;
	//		this.onInventoryItemsUpdated = onInventoryItemsUpdated;
	//		this.startingScrip = startingScrip;
	//		this.partyInventorySize = partyInventorySize;

	//		partyInventory = new Inventory(partyInventorySize, startingScrip);
	//	}

	//	public Party InstantiateClone()
	//	{
	//		return new Party(
	//			this.partyCharactersTemplateObject, 
	//			this.onStatsChanged, 
	//			this.onInventoryItemsUpdated, 
	//			this.partyInventorySize, 
	//			this.startingScrip);
	//	}
	}
}