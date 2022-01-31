using GramophoneUtils.Stats;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CharacterSlotsDisplayUI : MonoBehaviour
{
	[SerializeField] private PlayerBehaviour playerBehaviour;

	private Party party;

	private PartyCharacter[] frontCharacters;
	private PartyCharacter[] rearCharacters;

	public PlayerBehaviour PlayerBehaviour { get { return playerBehaviour; } set { playerBehaviour = value; } }

	[SerializeField] Transform FrontCharacterSlotsHolder; 
	[SerializeField] Transform RearCharacterSlotsHolder; 

	private void OnEnable()
	{
		frontCharacters = new PartyCharacter[4];
		rearCharacters = new PartyCharacter[4];

		party = playerBehaviour.Party;

		int frontIndex = 0;
		int rearIndex = 0;

		IEnumerable<PartyCharacter> query = party.PartyCharacters.Where(character => character.IsUnlocked == true);
		List<PartyCharacter> unlockedCharacters = new List<PartyCharacter>();
		foreach (PartyCharacter partyCharacter in query)
		{
			unlockedCharacters.Add(partyCharacter);
		}

		for (int i = 0; i < unlockedCharacters.Count; i++)
		{
			if (unlockedCharacters[i].IsRear || frontIndex > 3)
			{
				rearCharacters[rearIndex] = unlockedCharacters[i];
				rearIndex++;
			}
			else if (!unlockedCharacters[i].IsRear || rearIndex > 3)
			{
				frontCharacters[frontIndex] = unlockedCharacters[i];
				frontIndex++;
			}
		}
		

		RegisterOnCharacterSlotChangedEvents();
		

		for (int i = 0; i < FrontCharacterSlotsHolder.childCount; i++)
		{
			if (frontCharacters[i] != null)
			{
				PartyCharacterTemplate frontCharacter = UnityEngine.Object.Instantiate(frontCharacters[i].Character.PartyCharacterTemplate) as PartyCharacterTemplate;
				frontCharacter.PartyCharacter = frontCharacters[i];
				FrontCharacterSlotsHolder.GetChild(i).gameObject.GetComponent<CharacterSlotUI>().SlotResource = frontCharacter;
				FrontCharacterSlotsHolder.GetChild(i).gameObject.GetComponent<CharacterSlotUI>().UpdateSlotUI();
			}
			else
			{
				FrontCharacterSlotsHolder.GetChild(i).gameObject.GetComponent<CharacterSlotUI>().SlotResource = null;
				FrontCharacterSlotsHolder.GetChild(i).gameObject.GetComponent<CharacterSlotUI>().UpdateSlotUI();
			}
		}

		for (int i = 0; i < RearCharacterSlotsHolder.childCount; i++)
		{
			if (rearCharacters[i] != null)
			{
				PartyCharacterTemplate rearCharacter = UnityEngine.Object.Instantiate(rearCharacters[i].Character.PartyCharacterTemplate) as PartyCharacterTemplate;
				rearCharacter.PartyCharacter = rearCharacters[i];
				RearCharacterSlotsHolder.GetChild(i).gameObject.GetComponent<CharacterSlotUI>().SlotResource = rearCharacter;
				RearCharacterSlotsHolder.GetChild(i).gameObject.GetComponent<CharacterSlotUI>().UpdateSlotUI();
			}
			else
			{
				RearCharacterSlotsHolder.GetChild(i).gameObject.GetComponent<CharacterSlotUI>().SlotResource = null;
				RearCharacterSlotsHolder.GetChild(i).gameObject.GetComponent<CharacterSlotUI>().UpdateSlotUI();
			}
		}
	}

	private void OnDisable()
	{
		UnregisterOnCharacterSlotChangedEvents();
	}

	private void RegisterOnCharacterSlotChangedEvents()
	{
		for (int i = 0; i < FrontCharacterSlotsHolder.childCount; i++)
		{
			FrontCharacterSlotsHolder.GetChild(i).gameObject.GetComponent<CharacterSlotUI>().onCharacterSlotChanged += UpdateCharacterSlots;
		}

		for (int i = 0; i < RearCharacterSlotsHolder.childCount; i++)
		{
			RearCharacterSlotsHolder.GetChild(i).gameObject.GetComponent<CharacterSlotUI>().onCharacterSlotChanged += UpdateCharacterSlots;
		}
	}	
	
	private void UnregisterOnCharacterSlotChangedEvents()
	{
		for (int i = 0; i < FrontCharacterSlotsHolder.childCount; i++)
		{
			FrontCharacterSlotsHolder.GetChild(i).gameObject.GetComponent<CharacterSlotUI>().onCharacterSlotChanged -= UpdateCharacterSlots;
		}

		for (int i = 0; i < RearCharacterSlotsHolder.childCount; i++)
		{
			RearCharacterSlotsHolder.GetChild(i).gameObject.GetComponent<CharacterSlotUI>().onCharacterSlotChanged -= UpdateCharacterSlots;
		}
	}

	private void UpdateCharacterSlots()
	{
		for (int i = 0; i < FrontCharacterSlotsHolder.childCount; i++)
		{
			if (FrontCharacterSlotsHolder.GetChild(i).gameObject.GetComponent<CharacterSlotUI>().SlotResource != null)
			{
				Debug.Log("WERE DOING THE THING");
				PartyCharacterTemplate partyCharacterTemplate = FrontCharacterSlotsHolder.GetChild(i).gameObject.GetComponent<CharacterSlotUI>().SlotResource as PartyCharacterTemplate;
				partyCharacterTemplate.PartyCharacter.IsRear = false;
			}
		}

		for (int i = 0; i < RearCharacterSlotsHolder.childCount; i++)
		{
			if (RearCharacterSlotsHolder.GetChild(i).gameObject.GetComponent<CharacterSlotUI>().SlotResource != null)
			{
				PartyCharacterTemplate partyCharacterTemplate = RearCharacterSlotsHolder.GetChild(i).gameObject.GetComponent<CharacterSlotUI>().SlotResource as PartyCharacterTemplate;
				partyCharacterTemplate.PartyCharacter.IsRear = true;
			}
		}
	}
}
