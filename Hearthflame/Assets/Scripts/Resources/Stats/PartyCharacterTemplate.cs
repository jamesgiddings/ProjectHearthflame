using GramophoneUtils.Stats;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PartyCharacterTemplate
{
	[SerializeField] private CharacterTemplate template;
	[SerializeField] private bool isUnlocked;

	public bool IsUnlocked { get => isUnlocked; set => isUnlocked = value; }

	public CharacterTemplate Template => template;

}


