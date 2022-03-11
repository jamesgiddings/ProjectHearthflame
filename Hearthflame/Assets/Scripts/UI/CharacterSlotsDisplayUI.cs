using GramophoneUtils.Stats;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CharacterSlotsDisplayUI : MonoBehaviour
{
	[SerializeField] private PlayerBehaviour playerBehaviour;

	private Character[] frontCharacters;
	private Character[] rearCharacters;

	public PlayerBehaviour PlayerBehaviour { get { return playerBehaviour; } set { playerBehaviour = value; } }

	[SerializeField] Transform FrontCharacterSlotsHolder; 
	[SerializeField] Transform RearCharacterSlotsHolder; 

	private void OnEnable()
	{
		frontCharacters = new Character[4];
		rearCharacters = new Character[4];

		int frontIndex = 0;
		int rearIndex = 0;

		IEnumerable<Character> query = playerBehaviour.PlayerCharacters.Where(character => character.IsUnlocked == true);
		List<Character> unlockedCharacters = new List<Character>();
		foreach (Character character in query)
		{
			unlockedCharacters.Add(character);
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
				CharacterTemplate frontCharacter = UnityEngine.Object.Instantiate(frontCharacters[i].CharacterTemplate) as CharacterTemplate;
				//frontCharacter.PartyCharacter = frontCharacters[i];
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
				CharacterTemplate rearCharacter = UnityEngine.Object.Instantiate(rearCharacters[i].CharacterTemplate) as CharacterTemplate;
				//rearCharacter.PartyCharacter = rearCharacters[i];
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
		Debug.LogWarning("Broken");
		for (int i = 0; i < FrontCharacterSlotsHolder.childCount; i++)
		{
			if (FrontCharacterSlotsHolder.GetChild(i).gameObject.GetComponent<CharacterSlotUI>().SlotResource != null)
			{
				CharacterTemplate characterTemplate = FrontCharacterSlotsHolder.GetChild(i).gameObject.GetComponent<CharacterSlotUI>().SlotResource as CharacterTemplate;
				//characterTemplate.IsRear = false;
				//characterTemplate.PartyCharacter.Character.IsRear = false;
			}
		}

		for (int i = 0; i < RearCharacterSlotsHolder.childCount; i++)
		{
			if (RearCharacterSlotsHolder.GetChild(i).gameObject.GetComponent<CharacterSlotUI>().SlotResource != null)
			{
				CharacterTemplate characterTemplate = RearCharacterSlotsHolder.GetChild(i).gameObject.GetComponent<CharacterSlotUI>().SlotResource as CharacterTemplate;
				//characterTemplate.PartyCharacter.IsRear = true;
				//characterTemplate.PartyCharacter.Character.IsRear = true;
			}
		}
	}
}
