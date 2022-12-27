using GramophoneUtils.Characters;
using GramophoneUtils.Events.CustomEvents;
using GramophoneUtils.Stats;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class CharacterSlotsDisplayUI : MonoBehaviour
{
	private CharacterModel _characterModel;

	private Character[] frontCharacters;
	private Character[] rearCharacters;

	public CharacterModel CharacterModel { get { return _characterModel; } set { _characterModel = value; } }

	[SerializeField] public CharacterOrderEvent OnCharacterOrderChanged;
	[SerializeField] Transform FrontCharacterSlotsHolder; 
	[SerializeField] Transform RearCharacterSlotsHolder;

	private void OnEnable()
	{
        _characterModel = ServiceLocator.Instance.CharacterModel;
        frontCharacters = new Character[3];
		rearCharacters = new Character[3];

		int frontIndex = 0;
		int rearIndex = 0;

		IEnumerable<Character> query = _characterModel.PlayerCharacters.Where(character => character.IsUnlocked == true);
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
				Character frontCharacter = frontCharacters[i];
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
				Character rearCharacter = rearCharacters[i];
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
		UpdateCharacterSlots();
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
				Character character = FrontCharacterSlotsHolder.GetChild(i).gameObject.GetComponent<CharacterSlotUI>().SlotResource as Character;
                character.IsRear = false;
			}
		}

		for (int i = 0; i < RearCharacterSlotsHolder.childCount; i++)
		{
			if (RearCharacterSlotsHolder.GetChild(i).gameObject.GetComponent<CharacterSlotUI>().SlotResource != null)
			{
				Character character = RearCharacterSlotsHolder.GetChild(i).gameObject.GetComponent<CharacterSlotUI>().SlotResource as Character;
				character.IsRear = true;
			}
		}

		OnCharacterOrderChanged.Raise(new CharacterOrder(frontCharacters, rearCharacters));

    }
}
