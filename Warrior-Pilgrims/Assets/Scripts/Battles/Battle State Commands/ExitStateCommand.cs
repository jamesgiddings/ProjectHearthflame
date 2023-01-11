using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Exit State Command", menuName = "Commands/Exit State Command")]
public class ExitStateCommand : BattleStateCommand
{
    #region Attributes/Fields/Properties

    [SerializeField] StateManager _exitStateManager;
    [SerializeField] State _exitState;

    #endregion

    #region Constructors
    #endregion

    #region Callbacks
    #endregion

    #region Public Functions

    public override void Execute()
    {
        _exitStateManager.ChangeState(_exitState);
    }

    #endregion

    #region Protected Functions
    #endregion

    #region Private Functions
    #endregion

    #region Inner Classes
    #endregion

}
