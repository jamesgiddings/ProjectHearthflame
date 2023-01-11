using GramophoneUtils.Characters;
using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A scriptable object to add to skills which can target multiple characters in various arrays of slots. 
/// </summary>
[CreateAssetMenu(fileName = "TargetToSlots_Multiple_Various", menuName = "Character Classes/Skills/Targeting/TargetToSlots_Multiple_Various")]
public class TargetToSlots : ScriptableObject, ITargetToSlots
{
    #region Attributes/Fields/Properties

    [SerializeField, SerializeReference] private List<TargetCombination> _targetCombinations;

    #endregion

    #region Public Functions

    public ITargets GetTargetsObject(Character originator, CharacterOrder playerCharacterOrder, CharacterOrder enemyCharacterOrder)
    {
        return new TargetsObject(this, originator, playerCharacterOrder, enemyCharacterOrder);
    }

    public List<TargetCombination> GetTargetCombinations()
    {        
        return _targetCombinations;
    }

    #endregion

    #region Private Functions
    #endregion

    #region Inner Classes
    #endregion
}
