using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Raise Update Actions Task", menuName = "Tasks/Battle/Raise Update Actions Task")]
public class RaiseUpdateActionsTask : BattleStateTask
{
    #region Attributes/Fields/Properties
    #endregion

    #region Constructors
    #endregion

    #region Callbacks
    #endregion

    #region Public Functions

    public override Task Execute()
    {
        BattleDataModel.RaiseActionsAtEndOfState();
        return Task.CompletedTask;
    }

    #endregion

    #region Protected Functions
    #endregion

    #region Private Functions
    #endregion

    #region Inner Classes
    #endregion

}
