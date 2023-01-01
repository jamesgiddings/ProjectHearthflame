using GramophoneUtils.Characters;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.tvOS;
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

        Character[] arrayCache = new Character[_numberOfSlots - 1];
        if (characters.Length != _numberOfSlots)
        {
            int charactersIndex = 0;
            for (int arrayCacheIndex = 0; arrayCacheIndex < arrayCache.Length; arrayCacheIndex++)
            {
                if ((characters.Length - 1) < arrayCacheIndex)
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

    public void MoveCharactersForwardIntoSpaces()
    {
        for (int i = 0; i < NumberOfSlots; i++)
        {
            if (_slots[i] == null)
            {
                Character characterToMove = null;
                for (int j = i + 1; j < NumberOfSlots; j++)
                {
                    if (_slots[j] != null)
                    {
                        characterToMove = _slots[j];
                        MoveCharacterToSlot(characterToMove, i, j);
                        Debug.Log("0:" + (_slots[0] == null ? "null" : _slots[0].Name));
                        Debug.Log("1:" + (_slots[1] == null ? "null" : _slots[1].Name));
                        Debug.Log("2:" + (_slots[2] == null ? "null" : _slots[2].Name));
                        Debug.Log("3:" + (_slots[3] == null ? "null" : _slots[3].Name));
                        Debug.LogError("This appears to work, but beacuse the characters are still in the battler dictionary, they are not redrawn. We need a function that moves them when it is appropriate.");
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

    public HashSet<Character> GetCharacters()
    {
        HashSet<Character> characters = new HashSet<Character>();
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
    public void MoveCharacterToSlot(Character characterToMove, int newSlotIndex, int oldSlotIndex)
    {
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
            cachedCharacter1 = _slots[newSlotIndex];
            cachedCharacter2 = _slots[newSlotIndex + 1] == characterToMove ? null : _slots[newSlotIndex + 1];
            _slots[newSlotIndex] = characterToMove;
            _slots[newSlotIndex + 1] = characterToMove;

            if (cachedCharacter2 == null) // we're just moving forward one
            {
                _slots[oldSlotIndex + 1] = cachedCharacter1;
            }
            else
            {
                _slots[oldSlotIndex] = cachedCharacter1;
                _slots[oldSlotIndex + 1] = cachedCharacter2;
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
            throw new ArgumentNullException();
        }

        if (!_slots.ContainsValue(characterToMove))
        {
            Debug.LogWarning("Attempting to move a character that is not in the character order.");
            return false;
        }

        if (characterToMove.CharacterClass.Size == 2 && slotIndex == NumberOfSlots - 1)
        {
            Debug.LogWarning("Attempting to place a size 2 character in a slot with only 1 space. Move aborted.");
            return false;
        }
        return true;
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
