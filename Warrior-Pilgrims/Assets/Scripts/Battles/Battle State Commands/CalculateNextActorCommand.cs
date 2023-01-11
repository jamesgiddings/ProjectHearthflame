using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Calculate Next Actor Command", menuName = "Commands/Battle/Calculate Next Actor Command")]
public class CalculateNextActorCommand : BattleStateCommand
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
        BattleDataModel.UpdateCurrentActor();
        BattleDataModel.UpdateState();
    }

    #endregion

    #region Protected Functions
    #endregion

    #region Private Functions
    #endregion

    #region Inner Classes
    #endregion

}
