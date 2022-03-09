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
            if (characterSlots[i].CharacterTemplate != null)
            {
                if (characterSlots[i].CharacterTemplate == characterSlot.CharacterTemplate)
                {
                    int slotRemainingSpace = characterSlots[i].MaxStack - characterSlots[i].Quantity;

                    if (characterSlot.Quantity <= slotRemainingSpace)
                    {
                        characterSlots[i].Quantity += characterSlot.Quantity;

                        characterSlot.Quantity = 0;

                        onCharactersUpdated.Raise();

                        return characterSlot;
                    }
                    else if (slotRemainingSpace > 0)
                    {
                        characterSlots[i].Quantity += slotRemainingSpace;

                        characterSlot.Quantity -= slotRemainingSpace;
                    }
                }
            }
        }

        for (int i = 0; i < characterSlots.Length; i++)
        {
            if (characterSlots[i].CharacterTemplate == null)
            {
                if (characterSlot.Quantity <= characterSlot.MaxStack)
                {
                    characterSlots[i] = characterSlot;

                    characterSlot.Quantity = 0;

                    onCharactersUpdated?.Raise();

                    return characterSlot;
                }
                else
                {
                    characterSlots[i] = new CharacterSlot(characterSlot.Character, characterSlot.MaxStack);

                    characterSlot.Quantity -= characterSlot.MaxStack;
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
            if (characterSlots[i].CharacterTemplate != null)
            {
                if (characterSlots[i].CharacterTemplate == characterSlot.CharacterTemplate)
                {
                    if (characterSlots[i].Quantity < characterSlot.Quantity)
                    {
                        characterSlot.Quantity -= characterSlots[i].Quantity;

                        characterSlots[i] = new CharacterSlot();

                        onCharactersUpdated.Raise();
                    }
                    else
                    {
                        characterSlots[i].Quantity -= characterSlot.Quantity;

                        if (characterSlots[i].Quantity == 0)
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

    public List<CharacterTemplate> GetAllUniqueCharacters()
    {
        List<CharacterTemplate> characters = new List<CharacterTemplate>();

        for (int i = 0; i < characterSlots.Length; i++)
        {
            if (characterSlots[i].CharacterTemplate == null) { continue; }

            if (characters.Contains(characterSlots[i].CharacterTemplate)) { continue; }

            characters.Add(characterSlots[i].CharacterTemplate);
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

        if (secondSlot.CharacterTemplate != null && firstSlot.CharacterTemplate != null)
        {
            if (firstSlot.CharacterTemplate == secondSlot.CharacterTemplate)
            {
                int secondSlotRemainingSpace = secondSlot.MaxStack - secondSlot.Quantity;

                if (firstSlot.Quantity <= secondSlotRemainingSpace)
                {
                    itemContainerTwo.CharacterSlots[indexTwo].Quantity += firstSlot.Quantity;

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

    public virtual bool HasCharacter(CharacterTemplate character)
    {
        foreach (CharacterSlot characterSlot in characterSlots)
        {
            if (characterSlot.CharacterTemplate == null) { continue; }
            if (characterSlot.CharacterTemplate != character) { continue; }

            return true;
        }

        return false;
    }

    public virtual int GetTotalQuantity(CharacterTemplate character)
    {
        int totalCount = 0;

        foreach (CharacterSlot characterSlot in characterSlots)
        {
            if (characterSlot.CharacterTemplate == null) { continue; }
            if (characterSlot.CharacterTemplate != character) { continue; }

            totalCount += characterSlot.Quantity;
        }

        return totalCount;
    }
}
