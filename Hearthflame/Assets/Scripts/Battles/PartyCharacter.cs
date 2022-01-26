using GramophoneUtils.Stats;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyCharacter
{
	private bool isUnlocked;
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

	public Character Character => character; // getter

	public PartyCharacter(PartyCharacterTemplate partyCharacterTemplate, PlayerBehaviour playerBehaviour)
		{
		this.character = new Character(partyCharacterTemplate.Template, playerBehaviour);
		this.isUnlocked = partyCharacterTemplate.IsUnlocked;
		}
}
