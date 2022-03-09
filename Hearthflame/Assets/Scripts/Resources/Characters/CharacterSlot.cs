using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using GramophoneUtils.Stats;

[Serializable]
public struct CharacterSlot
{
    public CharacterTemplate CharacterTemplate;
    public Character Character;
    public int Quantity;
    public int MaxStack;

    public CharacterSlot(Character character, int quantity = 1, int maxStack = 1)
    {
        this.CharacterTemplate = character.CharacterTemplate;
        this.Character = character;
        this.Quantity = quantity;
        this.MaxStack = maxStack;
    }
}
