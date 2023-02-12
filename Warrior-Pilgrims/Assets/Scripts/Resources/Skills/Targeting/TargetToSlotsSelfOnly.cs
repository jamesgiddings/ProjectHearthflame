using GramophoneUtils.Characters;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A scriptable object to add to skills which can only target the caster. 
/// </summary>

[CreateAssetMenu(fileName = "TargetToSlots_Self_Only", menuName = "Skills/Targeting/TargetToSlots_Self_Only")]
public class TargetToSlotsSelfOnly : ScriptableObject, ITargetToSlots
{
    #region Attributes/Fields/Properties

    private List<TargetCombination> _targetCombinations;

    #endregion

    #region Constructors
    #endregion

    #region Callbacks
    #endregion

    #region Public Functions

    public List<TargetCombination> GetTargetCombinations()
    {
        return _targetCombinations;
    }

    public ITargets GetTargetsObject(Character originator, CharacterOrder playerCharacterOrder, CharacterOrder enemyCharacterOrder)
    {
        CharacterOrder opponentCharacterOrder = originator.IsPlayer ? enemyCharacterOrder : playerCharacterOrder;
        CharacterOrder allyCharacterOrder = originator.IsPlayer ? playerCharacterOrder : enemyCharacterOrder;

        _targetCombinations = new List<TargetCombination>() { new TargetCombination(new List<Character>() { originator }, allyCharacterOrder, opponentCharacterOrder) };
        return new TargetsObject(this, originator, playerCharacterOrder, enemyCharacterOrder);
    }

    #endregion

    #region Protected Functions
    #endregion

    #region Private Functions
    #endregion

    #region Inner Classes
    #endregion

}
