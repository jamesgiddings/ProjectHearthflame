using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Target Manager Subscribe To Battle Data Model Task", menuName = "Tasks/Battle/Target Manager Subscribe To Battle Data Model Task")]
public class TargetManagerSubscribeToBattleDataModelTask : BattleStateTask
{
    #region Attributes/Fields/Properties

    [SerializeField] private TargetManager _targetManager;
    [SerializeField] private bool _toggle;

    #endregion

    #region Constructors
    #endregion

    #region Callbacks
    #endregion

    #region Public Functions

    public override Task Execute()
    {
        if (_toggle)
        {
            _targetManager.SubscribeToBattleDataModelOnSkillUsed();
            return Task.CompletedTask;
        }
        _targetManager.UnsubscribeFromBattleDataModelOnSkillUsed();
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
