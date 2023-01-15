using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Initialise Battle Manager And Battle Data Model Task", menuName = "Tasks/Battle/Initialise Battle Manager And Battle Data Model Task")]
public class InitialiseBattleManagerAndBattleDataModelTask : BattleStateTask
{
    #region Attributes/Fields/Properties

    [SerializeField] BattleManager _battleManager;

    #endregion

    #region Constructors
    #endregion

    #region Callbacks
    #endregion

    #region Public Functions

    public override Task Execute()
    {
        _battleManager.InitialiseBattleManager();

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
