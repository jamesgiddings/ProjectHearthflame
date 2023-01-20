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

    public List<Character> GetCombination(List<Character> _allyCharacters, List<Character> _enemyCharacters)
    {
        List<Character> combination = new List<Character>();
        if (AllySlot4)
        {
            if (_allyCharacters.Count > 3 && _allyCharacters[3] != null)
            {
                combination.Add(_allyCharacters[3]);
            }
        }
        if (AllySlot3)
        {
            if (_allyCharacters.Count > 2 && _allyCharacters[2] != null)
            {
                combination.Add(_allyCharacters[2]);
            }
        }
        if (AllySlot2)
        {
            if (_allyCharacters.Count > 1 && _allyCharacters[1] != null)
            {
                combination.Add(_allyCharacters[1]);
            }
        }
        if (AllySlot1)
        {
            if (_allyCharacters.Count > 0 && _allyCharacters[0] != null)
            {
                combination.Add(_allyCharacters[0]);
            }
        }

        if (EnemySlot1)
        {
            if (_enemyCharacters.Count > 0 && _enemyCharacters[0] != null)
            {
                combination.Add(_enemyCharacters[0]);
            }
        }
        if (EnemySlot2)
        {
            if (_enemyCharacters.Count > 1 && _enemyCharacters[1] != null)
            {
                combination.Add(_enemyCharacters[1]);
            };
        }
        if (EnemySlot3)
        {
            if (_enemyCharacters.Count > 2 && _enemyCharacters[2] != null)
            {
                combination.Add(_enemyCharacters[2]);
            }
        }
        if (EnemySlot4)
        {
            if (_enemyCharacters.Count > 3 && _enemyCharacters[3] != null)
            {
                combination.Add(_enemyCharacters[3]);
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
