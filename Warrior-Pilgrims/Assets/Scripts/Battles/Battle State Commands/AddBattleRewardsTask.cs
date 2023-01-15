using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "AddBattleRewardsTask", menuName = "Tasks/Battle/AddBattleRewardsTask")]
public class AddBattleRewardsTask : BattleStateTask
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
        _battleManager.AddBattleReward();
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
