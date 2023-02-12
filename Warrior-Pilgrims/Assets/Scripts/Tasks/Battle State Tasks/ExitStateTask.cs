using NSubstitute.Core;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Exit State Task", menuName = "Tasks/Exit State Task")]
public class ExitStateTask : BattleStateTask
{
    #region Attributes/Fields/Properties

    [SerializeField] StateManager _exitStateManager;
    [SerializeField] State _exitState;
    [SerializeField] int _delayInMilliseconds = 0;

    #endregion

    #region Constructors
    #endregion

    #region Callbacks
    #endregion

    #region Public Functions

    public override async Task Execute()
    {
        await Task.Delay(_delayInMilliseconds);
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
