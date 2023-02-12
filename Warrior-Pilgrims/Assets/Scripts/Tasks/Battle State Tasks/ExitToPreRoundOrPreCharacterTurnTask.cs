using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Exit To Pre Round Or Pre Character Or Battle Won Or Loss Turn Task", menuName = "Tasks/Battle/Exit To Pre Round Or Pre Character Or Battle Won Or Loss Turn Task")]
public class ExitToPreRoundOrPreCharacterTurnTask : BattleStateTask
{
    #region Attributes/Fields/Properties

    [SerializeField] StateManager _exitStateManager;
    [SerializeField] State _exitToPreTurnState;
    [SerializeField] State _exitToPreRoundState;

    [SerializeField] int _delayInMilliseconds;

    #endregion

    #region Constructors
    #endregion

    #region Callbacks
    #endregion

    #region Public Functions

    public override async Task Execute()
    {
        await Task.Delay(_delayInMilliseconds);

        if (BattleDataModel.RespondToBattleLossOrVictory())
        {
            return;
        }

        if (BattleDataModel.IsEndOFRound())
        {
            _exitStateManager.ChangeState(_exitToPreRoundState);
            return;
        }
        _exitStateManager.ChangeState(_exitToPreTurnState);
    }

    #endregion

    #region Protected Functions
    #endregion

    #region Private Functions
    #endregion

    #region Inner Classes
    #endregion

}
