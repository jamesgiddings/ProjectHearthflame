using UnityEngine;

[CreateAssetMenu(fileName = "Exit To Pre Round Or Pre Character Turn Command", menuName = "Commands/Battle/Exit To Pre Round Or Pre Character Turn Command")]
public class ExitToPreRoundOrPreCharacterTurnCommand : BattleStateCommand
{
    #region Attributes/Fields/Properties

    [SerializeField] StateManager _exitStateManager;
    [SerializeField] State _exitToPreTurnState;
    [SerializeField] State _exitToPreRoundState;

    #endregion

    #region Constructors
    #endregion

    #region Callbacks
    #endregion

    #region Public Functions

    public override void Execute()
    {
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
