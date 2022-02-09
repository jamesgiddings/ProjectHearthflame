using GramophoneUtils.Stats;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "New Party Character Template", menuName = "Characters/Party Character Template")]
public class PartyCharacterTemplate : Resource
{
	[SerializeField] private CharacterTemplate template;
	[SerializeField] private bool isUnlocked;
	[SerializeField] private bool isRear;

	private PartyCharacter partyCharacter;

	public readonly int MaxStack = 1; // this is so we can reuse the ItemContainer code in CharacterContainer.
	public bool IsUnlocked { get => isUnlocked; set => isUnlocked = value; }
	public bool IsRear { get => isRear; set => isRear = value; }

	public PartyCharacter PartyCharacter { get => partyCharacter; set => partyCharacter = value; }

	public CharacterTemplate Template => template;

	private void OnEnable()
	{
		icon = template.Icon;
	}
}


