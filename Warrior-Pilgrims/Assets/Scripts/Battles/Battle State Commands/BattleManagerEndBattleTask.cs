using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Battle Manager End Battle Task", menuName = "Tasks/Battle/Battle Manager End Battle Task")]
public class BattleManagerEndBattleTask : BattleStateTask
{
    #region Attributes/Fields/Properties

    [SerializeField] private BattleManager _battleManager;

    #endregion

    #region Constructors
    #endregion

    #region Callbacks
    #endregion

    #region Public Functions

    public override Task Execute()
    {
        _battleManager.EndBattle();
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
