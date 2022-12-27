using GramophoneUtils.Characters;
using GramophoneUtils.Events.CustomEvents;
using GramophoneUtils.Stats;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
            if (characterSlots[i].Character != null)
            {
                if (characterSlots[i].Character == characterSlot.Character)
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
            if (characterSlots[i].Character == null)
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
            if (characterSlots[i].Character != null)
            {
                if (characterSlots[i].Character == characterSlot.Character)
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

    public List<Character> GetAllUniqueCharacters()
    {
        List<Character> characters = new List<Character>();

        for (int i = 0; i < characterSlots.Length; i++)
        {
            if (characterSlots[i].Character == null) { continue; }

            if (characters.Contains(characterSlots[i].Character)) { continue; }

            characters.Add(characterSlots[i].Character);
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

        if (secondSlot.Character != null && firstSlot.Character != null)
        {
            if (firstSlot.Character == secondSlot.Character)
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

    public virtual bool HasCharacter(Character character)
    {
        foreach (CharacterSlot characterSlot in characterSlots)
        {
            if (characterSlot.Character == null) { continue; }
            if (characterSlot.Character != character) { continue; }

            return true;
        }

        return false;
    }

    public virtual int GetTotalQuantity(Character character)
    {
        int totalCount = 0;

        foreach (CharacterSlot characterSlot in characterSlots)
        {
            if (characterSlot.Character == null) { continue; }
            if (characterSlot.Character != character) { continue; }

            totalCount += characterSlot.Quantity;
        }

        return totalCount;
    }
}
