using GramophoneUtils.Characters;
using GramophoneUtils.Stats;
using System.Collections.Generic;

public interface ICharacterContainer
{
    CharacterSlot[] CharacterSlots { get; }
    CharacterSlot GetSlotByIndex(int index);
    CharacterSlot AddCharacter(CharacterSlot characterSlot);
    List<Character> GetAllUniqueCharacters();
    void RemoveCharacter(CharacterSlot itemSlot);
    void RemoveAt(int slotIndex);
    void Swap(ICharacterContainer characterContainerOne, int indexOne, ICharacterContainer characterContainerTwo, int indexTwo);
    bool HasCharacter(Character character);
    int GetTotalQuantity(Character character);
}