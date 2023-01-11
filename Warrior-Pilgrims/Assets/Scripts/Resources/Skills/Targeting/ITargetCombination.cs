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

    List<Character> GetCombination(List<Character> _allyCharacters, List<Character> _enemyCharacters); 

    #endregion

    #region Protected Functions
    #endregion

    #region Private Functions
    #endregion

    #region Inner Classes
    #endregion
}
