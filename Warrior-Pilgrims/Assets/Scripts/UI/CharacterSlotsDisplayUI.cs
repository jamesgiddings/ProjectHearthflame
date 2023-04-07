using GramophoneUtils.Events.CustomEvents;
using GramophoneUtils.Stats;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CharacterSlotsDisplayUI : MonoBehaviour
{
	private ICharacterModel _characterModel;

	private ICharacter[] frontCharacters;
	private ICharacter[] rearCharacters;

	public ICharacterModel CharacterModel { get { return _characterModel; } set { _characterModel = value; } }

	[SerializeField] public CharacterOrderEvent OnCharacterOrderChanged;
	[SerializeField] Transform FrontCharacterSlotsHolder; 
	[SerializeField] Transform RearCharacterSlotsHolder;

	private void OnEnable()
	{
        _characterModel = ServiceLocator.Instance.CharacterModel;
        frontCharacters = new ICharacter[3];
		rearCharacters = new ICharacter[3];

		int frontIndex = 0;
		int rearIndex = 0;

		IEnumerable<ICharacter> query = _characterModel.PlayerCharacters.Where(character => character.IsUnlocked == true);
		List<ICharacter> unlockedCharacters = new List<ICharacter>();
		foreach (ICharacter character in query)
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
				ICharacter frontCharacter = frontCharacters[i];
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
				ICharacter rearCharacter = rearCharacters[i];
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
				ICharacter character = FrontCharacterSlotsHolder.GetChild(i).gameObject.GetComponent<CharacterSlotUI>().SlotResource as ICharacter;
                character.IsRear = false;
			}
		}

		for (int i = 0; i < RearCharacterSlotsHolder.childCount; i++)
		{
			if (RearCharacterSlotsHolder.GetChild(i).gameObject.GetComponent<CharacterSlotUI>().SlotResource != null)
			{
				ICharacter character = RearCharacterSlotsHolder.GetChild(i).gameObject.GetComponent<CharacterSlotUI>().SlotResource as ICharacter;
				character.IsRear = true;
			}
		}

		OnCharacterOrderChanged.Raise(new CharacterOrder(frontCharacters));

    }
}
