using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Update Unresolved Queue Command", menuName = "Commands/Battle/Update Unresolved Queue Command")]
public class UpdateUnresolvedQueueCommand : BattleStateCommand
{
    #region Attributes/Fields/Properties
    #endregion

    #region Constructors
    #endregion

    #region Callbacks
    #endregion

    #region Public Functions

    public override void Execute()
    {
        BattleDataModel.UpdateUnresolvedQueue();
    }

    #endregion

    #region Protected Functions
    #endregion

    #region Private Functions
    #endregion

    #region Inner Classes
    #endregion

}
