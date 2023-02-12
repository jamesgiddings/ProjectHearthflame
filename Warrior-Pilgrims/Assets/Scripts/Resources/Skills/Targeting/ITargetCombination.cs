using GramophoneUtils.Characters;
using System.Collections.Generic;

public interface ITargetCombination
{
    #region Attributes/Fields/Properties
    #endregion

    #region Constructors
    #endregion

    #region Callbacks
    #endregion

    #region Public Functions

    List<Character> GetCombination(CharacterOrder _allyCharacterOrder, CharacterOrder _enemyCharacterOrder); 

    #endregion

    #region Protected Functions
    #endregion

    #region Private Functions
    #endregion

    #region Inner Classes
    #endregion
}
