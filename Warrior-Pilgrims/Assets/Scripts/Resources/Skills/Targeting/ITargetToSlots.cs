using System.Collections.Generic;
public interface ITargetToSlots
{
    ITargets GetTargetsObject(ICharacter originator, CharacterOrder playerCharacterOrder, CharacterOrder enemyCharacterOrder);

    List<TargetCombination> GetTargetCombinations();
}
