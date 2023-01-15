using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Advance Turn Task", menuName = "Tasks/Battle/Advance Turn Task")]
public class AdvanceTurnTask : BattleStateTask
{
    #region Attributes/Fields/Properties
    #endregion

    #region Constructors
    #endregion

    #region Callbacks
    #endregion

    #region Public Functions

    public override async Task Execute()
    {
        BattleDataModel.AdvanceTurn();
        await Task.Delay(10);
    }

    #endregion

    #region Protected Functions
    #endregion

    #region Private Functions
    #endregion

    #region Inner Classes
    #endregion

}
