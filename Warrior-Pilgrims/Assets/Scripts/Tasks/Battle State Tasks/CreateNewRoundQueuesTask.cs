using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Create New Round Queues Task", menuName = "Tasks/Battle/Create New Round Queues Task")]
public class CreateNewRoundQueuesTask : BattleStateTask
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
        BattleDataModel.CreateNewRoundQueues();
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
