using GramophoneUtils.Items.Containers;
using GramophoneUtils.Stats;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New PartyCharactersTemplateObject", menuName = "Characters/Party Characters Template Object")]
public class PartyCharactersTemplateObject : ScriptableObject
{
	[SerializeField] PartyCharacterTemplate[] partyCharacterTemplates;
	public PartyCharacterTemplate[] PartyCharacterTemplates => partyCharacterTemplates; //getter	
}


