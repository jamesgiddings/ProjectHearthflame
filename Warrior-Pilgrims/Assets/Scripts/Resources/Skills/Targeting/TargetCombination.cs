using GramophoneUtils.Characters;
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

    #endregion

    #region Constructors
    #endregion

    #region Callbacks

    #endregion

    #region Public Functions

    public List<Character> GetCombination(List<Character> _allyCharacters, List<Character> _enemyCharacters)
    {
        List<Character> combination = new List<Character>();
        if (_allySlot4)
        {
            if (_allyCharacters.Count > 3 && _allyCharacters[3] != null)
            {
                combination.Add(_allyCharacters[3]);
            }
        }
        if (_allySlot3)
        {
            if (_allyCharacters.Count > 2 && _allyCharacters[2] != null)
            {
                combination.Add(_allyCharacters[2]);
            }
        }
        if (_allySlot2)
        {
            if (_allyCharacters.Count > 1 && _allyCharacters[1] != null)
            {
                combination.Add(_allyCharacters[1]);
            }
        }
        if (_allySlot1)
        {
            if (_allyCharacters.Count > 0 && _allyCharacters[0] != null)
            {
                combination.Add(_allyCharacters[0]);
            }
        }

        if (_enemySlot1)
        {
            if (_enemyCharacters.Count > 0 && _enemyCharacters[0] != null)
            {
                combination.Add(_enemyCharacters[0]);
            }
        }
        if (_enemySlot2)
        {
            if (_enemyCharacters.Count > 1 && _enemyCharacters[1] != null)
            {
                combination.Add(_enemyCharacters[1]);
            };
        }
        if (_enemySlot3)
        {
            if (_enemyCharacters.Count > 2 && _enemyCharacters[2] != null)
            {
                combination.Add(_enemyCharacters[2]);
            }
        }
        if (_enemySlot4)
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
