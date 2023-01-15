using GramophoneUtils.Characters;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = System.Object;

[Serializable]
public class CharacterOrder
{
    private static int _numberOfSlots = 4;

    private Dictionary<int, Character> _slots;

    public static int NumberOfSlots => _numberOfSlots;

    public Character Slot1Character
    {
        get { return _slots[0]; }
        set { _slots[0] = value; }
    }

    public Character Slot2Character
    {
        get { return _slots[1]; }
        set { _slots[1] = value; }
    }

    public Character Slot3Character
    {
        get { return _slots[2]; }
        set { _slots[2] = value; }
    }

    public Character Slot4Character
    {
        get { return _slots[3]; }
        set { _slots[3] = value; }
    }

    #region Constructors

    /// <summary>
    /// Creates a character from characters individually passed as parameters. Character slots can be left null, and the character will only create the
    /// characters in the slots specified. If a character is size two, it must be passed twice (i.e. CharacterOrder(size1char, size2char, size2char, size1char)
    /// would create an order for 3 characts with size2char being two slots wide, and occupying slots 2 and 3.
    /// </summary>
    /// <param name="slot1Character"></param>
    /// <param name="slot2Character"></param>
    /// <param name="slot3Character"></param>
    /// <param name="slot4Character"></param>
    public CharacterOrder(Character slot1Character = null, Character slot2Character = null, Character slot3Character = null, Character slot4Character = null)
    {
        Character[] characters = new Character[] { slot1Character, slot2Character, slot3Character, slot4Character };
        
        _slots = CreateDictionaryWithKeysAndNullValues();

        for (int i = 0; i < _numberOfSlots; i++)
        {
            _slots[i] = characters[i];
        }
    }

    /// <summary>
    /// Creates a character order from an array of characters. The characters do not have to be passed in twice if they are size 2, the constructor will handle
    /// this, but the overall size of the array should be no larger than the number of slots, or characters will be cut off. For example, if you passed in 
    /// 4 size 2 characters, the order would ignore the 3 and 4th items in the array and create a character order containing a single size 2 character spanning slots 1 and 2,
    /// and a single size 2 character spanning slots 3 and 4. If the array is smaller, then the constructor will leave the remaining slots blank.
    /// </summary>
    /// <param name="characters"></param>
    public CharacterOrder(Character[] characters)
    {
        _slots = CreateDictionaryWithKeysAndNullValues();

        int numberOfCharacters = 0;
        for (int i = 0; i < characters.Length; i++)
        {
            if (characters[i] != null)
            {
                numberOfCharacters++;
            }
        }

        Character[] arrayCache = new Character[_numberOfSlots];
        if (characters.Length != _numberOfSlots || numberOfCharacters != characters.Length)
        {
            int charactersIndex = 0;
            for (int arrayCacheIndex = 0; arrayCacheIndex < arrayCache.Length; arrayCacheIndex++)
            {
                if ((characters.Length - 1) < charactersIndex)
                {
                    break;
                }
                arrayCache[arrayCacheIndex] = characters[charactersIndex];
                if (characters[charactersIndex].CharacterClass.Size == 2)
                {
                    arrayCache[arrayCacheIndex] = characters[charactersIndex];
                    arrayCache[++arrayCacheIndex] = characters[charactersIndex];
                }
                charactersIndex++;
            }
            Debug.LogWarning("Trying to add character[] of the wrong size. Some characters may not be transferred.");
            characters = arrayCache;
        }
        for (int i = 0; i < characters.Length; i++)
        {
            _slots[i] = characters[i];
            if (characters[i] != null)
            {
                Debug.Log(characters[i].Name);
            }
        }
    }

    #endregion

    #region API
    /// <summary>
    /// Get the character by index (0-indexed), or returns null
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public Character GetCharacterBySlotIndex(int index)
    {
        if (_slots.ContainsKey(index))
        {
            return _slots[index];
        }
        return null;        
    }

    /// <summary>
    /// If there are spaces, moves the characters forwards (to a lower slot index).
    /// Includes characters from the slot index and below
    /// </summary>
    /// <param name="includeFromIndex"></param>
    public void MoveCharactersForwardIntoSpaces(int includeFromIndex = 3)
    {
        if (includeFromIndex > NumberOfSlots - 1) { includeFromIndex = NumberOfSlots - 1; }

        for (int i = 0; i < (includeFromIndex + 1); i++)
        {
            if (_slots[i] == null)
            {
                Character characterToMove = null;
                for (int j = i + 1; j < (includeFromIndex + 1); j++)
                {
                    if (_slots[j] != null)
                    {
                        characterToMove = _slots[j];
                        SwapCharacterIntoSlot(characterToMove, i);
                        break;
                    }
                }
                if (characterToMove == null)
                {
                    return;
                }
            }
        }
    }

    /// <summary>
    /// If there are spaces, moves the characters backwards (to a higher slot index).
    /// Includes characters from the slot index and above
    /// </summary>
    /// <param name="includeFromIndex"></param>
    public void MoveCharactersBackwardIntoSpaces(int includeFromIndex = 0)
    {
        if (includeFromIndex < 0) { includeFromIndex = 0; }
        for (int i = NumberOfSlots - 1; i > (includeFromIndex - 1); i--)
        {
            if (_slots[i] == null)
            {
                Character characterToMove = null;
                for (int j = i - 1; j > (includeFromIndex - 1); j--)
                {
                    if (_slots[j] != null)
                    {
                        characterToMove = _slots[j];

                        if (characterToMove.CharacterClass.Size == 2)
                        {
                            SwapCharacterIntoSlot(characterToMove, i - 1);
                        }
                        else
                        {
                            SwapCharacterIntoSlot(characterToMove, i);
                        }
                        break;
                    }
                }
                if (characterToMove == null)
                {
                    return;
                }
            }
        }
    }

    /// <summary>
    /// Get all the characters in the character order
    /// </summary>
    /// <returns>Returns a unique set of Characters as a HashSet, with no null values</returns>
    public HashSet<Character> GetCharacters()
    {
        HashSet<Character> characters = new HashSet<Character>();
        //Debug.Log(_slots == null); // TODO Optimise, this is being called hundreds of times, possibly during character death
        foreach (var item in _slots)
        {
            if (item.Value != null)
            {
                characters.Add(item.Value);
            }
        }
        return characters;
    }

    /// <summary>
    /// A positive value counts as moving backwards, a negative value counts as forward.
    /// Forward is closer to the centre of the battle (a lower slot index) and backward is away from the centre of the battle (a highger slot index).
    /// </summary>
    /// <param name="characterToMove"></param>
    /// <param name="slotsToMove"></param>
    public void MoveCharacter(Character characterToMove, int slotsToMove)
    {
        if (slotsToMove == 0) { return; } // no move

        int oldSlotIndex = GetSlotIndexByCharacter(characterToMove);

        int newSlotIndex = oldSlotIndex + slotsToMove;

        if (!ValidateMove(characterToMove, newSlotIndex))
        {
            newSlotIndex = GetValidSlotIndexFromInvalidSlotIndex(characterToMove, oldSlotIndex, newSlotIndex);
        }
        
        if (slotsToMove > 0) // move character backwards
        {
            RemoveCharacter(characterToMove);
            MoveCharactersForwardIntoSpaces(newSlotIndex); // but move other characters backwards to balance
        }
        else // move character forwards
        {
            RemoveCharacter(characterToMove);
            MoveCharactersBackwardIntoSpaces(newSlotIndex); // but move other characters backwards to balance
        }
        _slots[newSlotIndex] = characterToMove;
        if (characterToMove.CharacterClass.Size == 2)
        {
            _slots[newSlotIndex + 1] = characterToMove;
        }
    }

    /// <summary>
    /// Moves the character passed into the slotIndex (this is the 0-indexed number
    /// for the slot). If the characterToMove is size 2 and in the last slot, this 
    /// will give a warning, and just not move the character.
    /// The slot index refers to the slot closest to 0 if the character occupies two
    /// slots.
    /// </summary>
    /// <param name="characterToMove"></param>
    /// <param name="newSlotIndex"></param>
    /// <param name="oldSlotIndex"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public void SwapCharacterIntoSlot(Character characterToMove, int newSlotIndex)
    {
        int oldSlotIndex = GetSlotIndexByCharacter(characterToMove);
        if (oldSlotIndex == -1) { return; }

        if (!ValidateMove(characterToMove, newSlotIndex)) { return; }

        Character cachedCharacter1; 
        Character cachedCharacter2; 
        if (characterToMove.CharacterClass.Size == 1)
        {
            cachedCharacter1 = _slots[newSlotIndex];
            _slots[newSlotIndex] = characterToMove;
            _slots[oldSlotIndex] = cachedCharacter1;
        }
        else if (characterToMove.CharacterClass.Size == 2)
        {
            int moveLength = Math.Abs(oldSlotIndex - newSlotIndex);

            if (moveLength < 1 || moveLength > 2)
            {
                throw new ArgumentOutOfRangeException("Maximum move for size 2 character should be 2.");
            }

            if (moveLength == 2) // this can only happen from position index 0 -> 2, or 2 -> 0, so it is a straight swap.
            {
                cachedCharacter1 = _slots[newSlotIndex];
                cachedCharacter2 = _slots[newSlotIndex + 1];

                _slots[newSlotIndex] = characterToMove;
                _slots[newSlotIndex + 1] = characterToMove;

                _slots[oldSlotIndex] = cachedCharacter1;
                _slots[oldSlotIndex + 1] = cachedCharacter2;
            }
            else // a move of length 1 move could be forward or backward and from position index 1, 2 or 3.
            {
                if(oldSlotIndex > newSlotIndex) // moving left
                {
                    cachedCharacter1 = _slots[newSlotIndex];

                    _slots[newSlotIndex] = characterToMove;
                    _slots[newSlotIndex + 1] = characterToMove;

                    _slots[oldSlotIndex + 1] = cachedCharacter1;
                }
                else // moving right
                {
                    cachedCharacter1 = _slots[newSlotIndex + 1];

                    _slots[newSlotIndex] = characterToMove;
                    _slots[newSlotIndex + 1] = characterToMove;

                    _slots[oldSlotIndex] = cachedCharacter1;
                }
            }
        }
    }

    public int GetSlotIndexByCharacter(Character character)
    {
        if (character == null)
        {
            Debug.LogError("Character was null.");
            return -1;
        }
        if (_slots.ContainsValue(character))
        {
            var kvp = _slots.First(kvp => kvp.Value == character);
            return kvp.Key;
        }
        Debug.LogError("Character was not found in a slot.");
        return -1;
    }

    public List<Character> AddCharactersAndReturnRemainder(List<Character> characters)
    {
        int j = 0;
        if (characters.Count == 0) { return characters; } 
        for (int i = 0; i < NumberOfSlots; i++)
        {
            
            if (characters[j].CharacterClass.Size == 1 && _slots[i] == null)
            {
                _slots[i] = characters[j]; 
                characters.RemoveAt(j);
                if (characters.Count - 1 < j) { return characters; } // if we have reached the end of the list of characters, return it
            }

            if (characters[j].CharacterClass.Size == 2 && i < NumberOfSlots - 1 && _slots[i] == null && _slots[i + 1] == null)
            {
                _slots[i] = characters[j];
                _slots[++i] = characters[j];
                characters.RemoveAt(j);
                if (characters.Count - 1 < j) { return characters; } // if we have reached the end of the list of characters, return it
            }
        }
        return characters;
    }

    /// <summary>
    /// Removes all instances of the character. 
    /// Returns false if not found.
    /// </summary>
    /// <param name="character"></param>
    /// <returns></returns>
    public bool RemoveCharacter(Character character)
    {
        bool removed = false;
        foreach (var item in _slots.Where(kvp => kvp.Value == character).ToList())
        {
            removed = true;
            _slots[item.Key] = null;
        }
        return removed;
    }

    public override bool Equals(object obj)
    {
        return AreCharacterOrdersEqual(obj, this);
    }

    #endregion

    #region Utilities

    private bool ValidateMove(Character characterToMove, int slotIndex)
    {
        if (characterToMove == null)
        {
            throw new ArgumentNullException("Character for move was null.");
        }

        if (!_slots.ContainsValue(characterToMove))
        {
            throw new Exception("Attempting to move a character that is not in the character order.");
        }

        if (characterToMove.CharacterClass.Size == 2 && slotIndex == NumberOfSlots - 1)
        {
            return false;
        }

        if (slotIndex < 0 || slotIndex > (NumberOfSlots - 1))
        {
            return false;
        }
        return true;
    }

    private int GetValidSlotIndexFromInvalidSlotIndex(Character characterToMove, int oldSlotIndex, int proposedNewSlotIndex)
    {
        bool moveForwards = oldSlotIndex > proposedNewSlotIndex;
        int validNewSlotIndex;

        if (characterToMove.CharacterClass.Size == 2)
        {
            if (moveForwards)
            {
                validNewSlotIndex = proposedNewSlotIndex < 0 ? 0 : proposedNewSlotIndex > (_numberOfSlots - 2) ? (_numberOfSlots - 2) : proposedNewSlotIndex;
            }
            else
            {
                validNewSlotIndex = (_numberOfSlots - 2) - oldSlotIndex; // can only move to the penultimate place as a size 2
            }
        }
        else
        {
            if (moveForwards)
            {
                validNewSlotIndex = proposedNewSlotIndex < 0 ? 0 : proposedNewSlotIndex > (_numberOfSlots - 1) ? (_numberOfSlots - 1) : proposedNewSlotIndex;
            }
            else
            {
                validNewSlotIndex = (_numberOfSlots - 1) - oldSlotIndex;
            }
        }

        return validNewSlotIndex;
    }

    private Dictionary<int, Character> CreateDictionaryWithKeysAndNullValues()
    {
        _slots = new Dictionary<int, Character>();
        for (int i = 0; i < _numberOfSlots; i++)
        {
            _slots.Add(i, null);
        }
        return _slots;
    }

    private bool AreCharacterOrdersEqual(Object obj, CharacterOrder order2)
    {
        if (obj as CharacterOrder == null) { return false; }
        CharacterOrder order1 = obj as CharacterOrder;
        bool areEqual = true;
        foreach (var key in order1._slots.Keys)
        {
            if (!order2._slots[key].Equals(order1._slots[key]))
            {
                return false;
            }
        }
        return areEqual;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(_slots, Slot1Character, Slot2Character, Slot3Character, Slot4Character);
    }

    #endregion

}
