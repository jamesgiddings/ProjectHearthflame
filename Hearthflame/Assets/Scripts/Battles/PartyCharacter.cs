using GramophoneUtils.Items.Containers;
using GramophoneUtils.Stats;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyCharacter
{
	[SerializeField] private bool isUnlocked;
	[SerializeField] private bool isRear;

	private readonly Character character;

	public bool IsUnlocked
	{
		get
		{
			return isUnlocked;
		}
		set
		{
			isUnlocked = value;
		}
	}	
	
	public bool IsRear
	{
		get
		{
			return isRear;
		}
		set
		{
			isRear = value;
		}
	}

	public Character Character => character; // getter

	public PartyCharacter(CharacterTemplate characterTemplate, Inventory partyInventory)
		{
			this.character = new Character(characterTemplate, partyInventory);
			this.isUnlocked = characterTemplate.StartsUnlocked;
		}
}
