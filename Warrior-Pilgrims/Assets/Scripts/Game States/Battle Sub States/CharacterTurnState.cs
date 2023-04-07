public class CharacterTurnState : BattleSubState
{
    #region Attributes/Fields/Properties

    protected ICharacter StateActor;
    protected bool CharacterNotInControl;

    #endregion

    #region Public Functions

    public override void EnterState()
    {
        base.EnterState();
        StateActor = BattleManager.BattleDataModel.CurrentActor;
        CharacterNotInControl = SkipTurnIfCharacterNotInControl();
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void HandleInput()
    {
        base.HandleInput();
    }

    #endregion

    #region Private Functions

    private bool SkipTurnIfCharacterNotInControl()
    {
        if (StateActor.StatSystem.CharacterIsNotInControl()) 
        {
            ChangeStateAfterDelay(ServiceLocatorObject.Instance.Constants.BattleShortDelay, ServiceLocatorObject.Instance.PostCharacterTurnState, ServiceLocatorObject.Instance.BattleStateManager);
            return true;
        }
        return false;
    }

    #endregion
}
