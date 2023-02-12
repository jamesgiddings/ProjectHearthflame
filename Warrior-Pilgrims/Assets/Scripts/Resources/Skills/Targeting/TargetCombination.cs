using GramophoneUtils.Characters;
using GramophoneUtils.Stats;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class TargetCombination : ITargetCombination
{
    #region Attributes/Fields/Properties

    [SerializeField] private bool _allySlot1;
    [SerializeField] private bool _allySlot2;
    [SerializeField] private bool _allySlot3;
    [SerializeField] private bool _allySlot4;

    [SerializeField] private bool _enemySlot1;
    [SerializeField] private bool _enemySlot2;
    [SerializeField] private bool _enemySlot3;
    [SerializeField] private bool _enemySlot4;

    public bool AllySlot1 { get => _allySlot1; set => _allySlot1 = value; }
    public bool AllySlot2 { get => _allySlot2; set => _allySlot2 = value; }
    public bool AllySlot3 { get => _allySlot3; set => _allySlot3 = value; }
    public bool AllySlot4 { get => _allySlot4; set => _allySlot4 = value; }
    public bool EnemySlot1 { get => _enemySlot1; set => _enemySlot1 = value; }
    public bool EnemySlot2 { get => _enemySlot2; set => _enemySlot2 = value; }
    public bool EnemySlot3 { get => _enemySlot3; set => _enemySlot3 = value; }
    public bool EnemySlot4 { get => _enemySlot4; set => _enemySlot4 = value; }

    #endregion

    #region Constructors

    public TargetCombination(List<Character> characters, CharacterOrder playerCharacterOrder, CharacterOrder enemyCharacterOrder)
    {
        foreach (var character in characters)
        {
            if (playerCharacterOrder.GetCharacters().Contains(character))
            {
                int slotIndex = playerCharacterOrder.GetSlotIndexByCharacter(character);
                switch (slotIndex)
                {
                    case 0:
                        AllySlot1 = true;
                        break;
                    case 1:
                        AllySlot2 = true;
                        break;
                    case 2:
                        AllySlot3 = true;
                        break;
                    case 3:
                        AllySlot4 = true;
                        break;
                    default:
                        break;
                }
            }
            if (enemyCharacterOrder.GetCharacters().Contains(character))
            {
                int slotIndex = enemyCharacterOrder.GetSlotIndexByCharacter(character);
                switch (slotIndex)
                {
                    case 0:
                        EnemySlot1 = true;
                        break;
                    case 1:
                        EnemySlot2 = true;
                        break;
                    case 2:
                        EnemySlot3 = true;
                        break;
                    case 3:
                        EnemySlot4 = true;
                        break;
                    default:
                        break;
                }
            }
        }
    }

    #endregion

    #region Callbacks
    #endregion

    #region Public Functions

    public List<Character> GetCombination(CharacterOrder _allyCharacterOrder, CharacterOrder _enemyCharacterOrder)
    {
        List<Character> combination = new List<Character>();
        if (AllySlot4)
        { 
            if (_allyCharacterOrder.Slot4Character != null)
            {
                combination.Add(_allyCharacterOrder.Slot4Character);
            }
        }
        if (AllySlot3)
        {
            if (_allyCharacterOrder.Slot3Character != null)
            {
                combination.Add(_allyCharacterOrder.Slot3Character);
            }
        }
        if (AllySlot2)
        {
            if (_allyCharacterOrder.Slot2Character != null)
            {
                combination.Add(_allyCharacterOrder.Slot2Character); //TODO, the problem here is when there are fewer than 4 ally characters, we're not geeting the allycharacter in slot 2 if slot 1 is empty.
            }
        }
        if (AllySlot1)
        {
            if (_allyCharacterOrder.Slot1Character != null)
            {
                combination.Add(_allyCharacterOrder.Slot1Character);
            }
        }

        if (EnemySlot1)
        {
            if (_enemyCharacterOrder.Slot1Character != null)
            {
                combination.Add(_enemyCharacterOrder.Slot1Character);
            }
        }
        if (EnemySlot2)
        {
            if (_enemyCharacterOrder.Slot2Character != null)
            {
                combination.Add(_enemyCharacterOrder.Slot2Character);
            };
        }
        if (EnemySlot3)
        {
            if (_enemyCharacterOrder.Slot3Character != null)
            {
                combination.Add(_enemyCharacterOrder.Slot3Character);
            }
        }
        if (EnemySlot4)
        {
            if (_enemyCharacterOrder.Slot4Character != null)
            {
                combination.Add(_enemyCharacterOrder.Slot4Character);
            }
        }

        return combination;
    }

    #endregion

    #region Protected Functions
    #endregion

    #region Private Functions
    #endregion

    #region Inner Classes
    #endregion
}
