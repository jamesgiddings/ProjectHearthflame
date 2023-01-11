using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BattleStateCommand : ScriptableObject, ICommand
{
    #region Attributes/Fields/Properties

    [SerializeField] protected BattleDataModel BattleDataModel;

    #endregion

    #region Constructors
    #endregion

    #region Callbacks
    #endregion

    #region Public Functions

    public abstract void Execute();

    #endregion

    #region Protected Functions
    #endregion

    #region Private Functions
    #endregion

    #region Inner Classes
    #endregion

}
