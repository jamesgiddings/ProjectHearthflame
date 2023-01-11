using GramophoneUtils.Characters;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBrain
{
    #region Attributes/Fields/Properties
    #endregion

    #region Constructors
    #endregion

    #region Callbacks
    #endregion

    #region Public Functions

    ISkill ChooseSkill(Character character);

    List<Character> ChooseTargets(Character originator, ISkill skill);

    #endregion

    #region Protected Functions
    #endregion

    #region Private Functions
    #endregion

    #region Inner Classes
    #endregion
}
