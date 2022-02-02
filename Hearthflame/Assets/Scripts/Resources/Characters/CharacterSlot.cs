using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using GramophoneUtils.Stats;

[Serializable]
public struct CharacterSlot
{
    public PartyCharacterTemplate PartyCharacterTemplate;
    public Character Character;
    public int Quantity;

    public CharacterSlot(Character character, int quantity = 1)
    {
        this.PartyCharacterTemplate = character.PartyCharacterTemplate;
        this.Character = character;
        this.Quantity = quantity;
    }
}
