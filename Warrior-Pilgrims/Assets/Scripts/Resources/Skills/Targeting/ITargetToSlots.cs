using GramophoneUtils.Characters;
using System.Collections.Generic;
using UnityEngine;

public interface ITargetToSlots
{
    ITargets GetTargetsObject(Character originator, CharacterOrder playerCharacterOrder, CharacterOrder enemyCharacterOrder);

    List<TargetCombination> GetTargetCombinations();
}
