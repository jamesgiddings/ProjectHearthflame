using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Update Radial Menu Task", menuName = "Tasks/Battle/Update Radial Menu Task")]
public class UpdateRadialMenuTask : BattleStateTask
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
        ServiceLocator.Instance.ServiceLocatorObject.BattleManager.UpdateRadialMenu();
        await Task.Yield();
    }

    #endregion

    #region Protected Functions
    #endregion

    #region Private Functions
    #endregion

    #region Inner Classes
    #endregion
}
