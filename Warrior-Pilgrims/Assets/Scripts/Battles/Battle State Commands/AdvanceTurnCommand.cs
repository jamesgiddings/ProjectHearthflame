using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Advance Turn Command", menuName = "Commands/Battle/Advance Turn Command")]
public class AdvanceTurnCommand : BattleStateCommand
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
        BattleDataModel.AdvanceTurn();
    }

    #endregion

    #region Protected Functions
    #endregion

    #region Private Functions
    #endregion

    #region Inner Classes
    #endregion

}
