using GramophoneUtils.Events.CustomEvents;
using GramophoneUtils.Stats;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CharacterContainer : ICharacterContainer
{
    [SerializeField] protected CharacterSlot[] characterSlots = new CharacterSlot[0];

    public VoidEvent onCharactersUpdated;

    public CharacterSlot[] CharacterSlots { get => characterSlots; }

    public CharacterSlot GetSlotByIndex(int index) => characterSlots[index];

    public virtual CharacterSlot AddCharacter(CharacterSlot characterSlot)
    {
        for (int i = 0; i < characterSlots.Length; i++)
        {
            if (characterSlots[i].character != null)
            {
                if (characterSlots[i].character == characterSlot.character)
                {
                    int slotRemainingSpace = characterSlots[i].character.MaxStack - characterSlots[i].quantity;

                    if (characterSlot.quantity <= slotRemainingSpace)
                    {
                        characterSlots[i].quantity += characterSlot.quantity;

                        characterSlot.quantity = 0;

                        onCharactersUpdated.Raise();

                        return characterSlot;
                    }
                    else if (slotRemainingSpace > 0)
                    {
                        characterSlots[i].quantity += slotRemainingSpace;

                        characterSlot.quantity -= slotRemainingSpace;
                    }
                }
            }
        }

        for (int i = 0; i < characterSlots.Length; i++)
        {
            if (characterSlots[i].character == null)
            {
                if (characterSlot.quantity <= characterSlot.character.MaxStack)
                {
                    characterSlots[i] = characterSlot;

                    characterSlot.quantity = 0;

                    onCharactersUpdated?.Raise();

                    return characterSlot;
                }
                else
                {
                    characterSlots[i] = new CharacterSlot(characterSlot.character, characterSlot.character.MaxStack);

                    characterSlot.quantity -= characterSlot.character.MaxStack;
                }
            }
        }
        onCharactersUpdated.Raise();
        return characterSlot;
    }

    public virtual void RemoveCharacter(CharacterSlot characterSlot)
    {
        for (int i = 0; i < characterSlots.Length; i++)
        {
            if (characterSlots[i].character != null)
            {
                if (characterSlots[i].character == characterSlot.character)
                {
                    if (characterSlots[i].quantity < characterSlot.quantity)
                    {
                        characterSlot.quantity -= characterSlots[i].quantity;

                        characterSlots[i] = new CharacterSlot();

                        onCharactersUpdated.Raise();
                    }
                    else
                    {
                        characterSlots[i].quantity -= characterSlot.quantity;

                        if (characterSlots[i].quantity == 0)
                        {
                            characterSlots[i] = new CharacterSlot();

                            onCharactersUpdated.Raise();

                            return;
                        }
                    }
                }
            }
        }
    }

    public List<PartyCharacterTemplate> GetAllUniqueCharacters()
    {
        List<PartyCharacterTemplate> characters = new List<PartyCharacterTemplate>();

        for (int i = 0; i < characterSlots.Length; i++)
        {
            if (characterSlots[i].character == null) { continue; }

            if (characters.Contains(characterSlots[i].character)) { continue; }

            characters.Add(characterSlots[i].character);
        }

        return characters;
    }

    public virtual void RemoveAt(int slotIndex)
    {
        if (slotIndex < 0 || slotIndex > characterSlots.Length - 1) { return; }

        characterSlots[slotIndex] = new CharacterSlot();

        onCharactersUpdated.Raise();
    }

    public virtual void Swap(ICharacterContainer itemContainerOne, int indexOne, ICharacterContainer itemContainerTwo, int indexTwo)
    {
        CharacterSlot firstSlot = itemContainerOne.CharacterSlots[indexOne];
        CharacterSlot secondSlot = itemContainerTwo.CharacterSlots[indexTwo];

        if (firstSlot.Equals(secondSlot)) { return; }

        if (secondSlot.character != null && firstSlot.character != null)
        {
            if (firstSlot.character == secondSlot.character)
            {
                int secondSlotRemainingSpace = secondSlot.character.MaxStack - secondSlot.quantity;

                if (firstSlot.quantity <= secondSlotRemainingSpace)
                {
                    itemContainerTwo.CharacterSlots[indexTwo].quantity += firstSlot.quantity;

                    itemContainerOne.CharacterSlots[indexOne] = new CharacterSlot();

                    onCharactersUpdated.Raise();

                    return;
                }
            }
        }

        itemContainerOne.CharacterSlots[indexOne] = secondSlot;
        itemContainerTwo.CharacterSlots[indexTwo] = firstSlot;

        onCharactersUpdated.Raise();
    }

    public virtual bool HasCharacter(PartyCharacterTemplate character)
    {
        foreach (CharacterSlot characterSlot in characterSlots)
        {
            if (characterSlot.character == null) { continue; }
            if (characterSlot.character != character) { continue; }

            return true;
        }

        return false;
    }

    public virtual int GetTotalQuantity(PartyCharacterTemplate character)
    {
        int totalCount = 0;

        foreach (CharacterSlot characterSlot in characterSlots)
        {
            if (characterSlot.character == null) { continue; }
            if (characterSlot.character != character) { continue; }

            totalCount += characterSlot.quantity;
        }

        return totalCount;
    }
}
