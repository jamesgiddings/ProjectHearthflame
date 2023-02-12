using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Respond To Battle Loss Or Victory Task", menuName = "Tasks/Battle/Respond To Battle Loss Or Victory Task")]
public class RespondToBattleLossOrVictoryTask : BattleStateTask
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
        BattleDataModel.RespondToBattleLossOrVictory();
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
