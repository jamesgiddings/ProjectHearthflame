using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using GramophoneUtils.Stats;

[Serializable]
public struct CharacterSlot
{
    public PartyCharacterTemplate character;
    public int quantity;

    public CharacterSlot(PartyCharacterTemplate characterTemplate, int quantity = 1)
    {
        this.character = characterTemplate;
        this.quantity = quantity;
    }
}
