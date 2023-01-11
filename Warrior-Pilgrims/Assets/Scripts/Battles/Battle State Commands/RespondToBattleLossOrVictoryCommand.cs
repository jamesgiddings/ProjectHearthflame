using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Respond To Battle Loss Or Victory Command", menuName = "Commands/Battle/Respond To Battle Loss Or Victory Command")]
public class RespondToBattleLossOrVictoryCommand : BattleStateCommand
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
        BattleDataModel.RespondToBattleLossOrVictory();
    }

    #endregion

    #region Protected Functions
    #endregion

    #region Private Functions
    #endregion

    #region Inner Classes
    #endregion

}
