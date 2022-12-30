using GramophoneUtils.Characters;
using System;
using System.Collections.Generic;
using UnityEngine;
using Object = System.Object;

[Serializable]
public class CharacterOrder
{
    private static int _numberOfFrontCharacters = 3;
    private static int _numberOfRearCharacters = 3;

    private Character[] _frontCharacters;
    private Character[] _rearCharacters;

    public static int NumberOfFrontCharacters => _numberOfFrontCharacters;
    public static int NumberOfRearCharacters => _numberOfRearCharacters;

    public Character[] FrontCharacters
    {
        get { return _frontCharacters; }
        set { _frontCharacters = value; }
    }

    public Character[] RearCharacters
    {
        get { return _rearCharacters; }
        set { _rearCharacters = value; }
    }

    public CharacterOrder(List<Character> frontCharacters, List<Character> rearCharacters)
    {
        if(frontCharacters.Count > _numberOfFrontCharacters || rearCharacters.Count > _numberOfRearCharacters)
        {
            Debug.LogError("Trying to add too many characters by list in CharacterOrder constructor.");
            return;
        }

        Character[] frontCharactersArray = new Character[_numberOfFrontCharacters];
        
        for (int i = 0; i < frontCharacters.Count; i++)
        {
            frontCharactersArray[i] = frontCharacters[i];
        }

        this._frontCharacters = frontCharactersArray;

        Character[] rearCharactersArray = new Character[_numberOfRearCharacters];
        
        for (int i = 0; i < rearCharacters.Count; i++)
        {
            rearCharactersArray[i] = rearCharacters[i];
        }

        this._rearCharacters = rearCharactersArray;
    }

    public CharacterOrder(Character[] frontCharacters, Character[] rearCharacters)
    {
        if (frontCharacters.Length != _numberOfFrontCharacters || rearCharacters.Length != _numberOfRearCharacters)
        {
            Debug.LogError("Trying to add character[]s of the wrong size.");
            return;
        }
        _frontCharacters = frontCharacters;
        _rearCharacters = rearCharacters;
    }

    public CharacterOrder(Character[] characters)
    {
        Character[] arrayCache = new Character[_numberOfFrontCharacters + _numberOfRearCharacters];
        if (characters.Length != _numberOfFrontCharacters + _numberOfRearCharacters)
        {
            for (int i = 0; i < arrayCache.Length; i++)
            {
                if ((characters.Length - 1) < i)
                {
                    break;
                }
                arrayCache[i] = characters[i];
            }
            Debug.LogWarning("Trying to add character[] of the wrong size. Some characters may not be transferred.");
            characters = arrayCache;
        }

        _frontCharacters = new Character[_numberOfFrontCharacters];
        _rearCharacters = new Character[_numberOfRearCharacters];

        for (int i = 0; i < characters.Length; i++)
        {
            if (characters[i] == null) { continue; }
            if (i >= _numberOfFrontCharacters)
            {
                _rearCharacters[i] = characters[i];
            }
            else
            {
                _frontCharacters[i] = characters[i];
            }
        }
    }

    #region API

    public List<Character> AddCharactersToFrontAndReturnRemainder(List<Character> characters)
    {
        int j = 0;
        for (int i = 0; i < _frontCharacters.Length; i++)
        {
            
            if (characters.Count - 1 < j) { return characters; }
            if (_frontCharacters[i] == null)
            {
                _frontCharacters[i] = characters[j];
                characters.RemoveAt(j);
                j--;
            }
            j++;
        }
        return characters;
    }

    /// <summary>
    /// Removes the first instance of that character and returns true. 
    /// Returns false if not found.
    /// </summary>
    /// <param name="character"></param>
    /// <returns></returns>
    public bool RemoveCharacter(Character character)
    {
        for (int i = 0; i < _frontCharacters.Length; i++)
        {
            if (_frontCharacters[i].Equals(character))   
            {
                _frontCharacters[i] = null;
                return true;
            }
        }
        for (int i = 0; i < _rearCharacters.Length; i++)
        {
            if (_rearCharacters[i].Equals(character))
            {
                _rearCharacters[i] = null;
                return true;
            }
        }
        return false;
    }

    public override bool Equals(object obj)
    {
        return AreFrontsEqual(obj, this) && AreRearsEqual(obj, this);
    }

    #endregion

    #region Utilities

    private bool AreFrontsEqual(Object obj, CharacterOrder order2)
    {
        if (obj as CharacterOrder == null) { return false; }
        CharacterOrder order1 = obj as CharacterOrder;
        bool areEqual = true;
        for (int i = 0; i < order1.FrontCharacters.Length; i++)
        {
            if (order1.FrontCharacters[i] != order2.FrontCharacters[i])
            {
                return false;
            }
        }
        return areEqual;
    }
    
    private bool AreRearsEqual(Object obj, CharacterOrder order2)
    {
        if (obj as CharacterOrder == null) { return false; }
        CharacterOrder order1 = obj as CharacterOrder;
        bool areEqual = true;
        for (int i = 0; i < order1.RearCharacters.Length; i++)
        {
            if (order1.RearCharacters[i] != order2.RearCharacters[i])
            {
                return false;
            }
        }
        return areEqual;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(_frontCharacters, _rearCharacters, FrontCharacters, RearCharacters);
    }

    #endregion

}
