using GramophoneUtils.Characters;
using System.Collections.Generic;
using UnityEngine;

public interface ITargets
{
    #region Attributes/Fields/Properties
    #endregion

    #region Constructors
    #endregion

    #region Callbacks
    #endregion

    #region Public Functions

    List<Character> GetCurrentlyTargeted();
    List<Character> ChangeCurrentlyTargeted(Vector2 direction); 

    #endregion

    #region Protected Functions
    #endregion

    #region Private Functions
    #endregion

    #region Inner Classes
    #endregion
}
