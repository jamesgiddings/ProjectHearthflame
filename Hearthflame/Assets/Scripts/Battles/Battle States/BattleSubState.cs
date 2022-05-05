public abstract class BattleSubState : State
{
	protected BattleManager battleManager;

	public override void EnterState()
	{
		battleManager.BattleStateManager.BattleState = this;
	}

	public override void ExitState()
	{
		battleManager.TargetManager.ClearTargets();
	}
}
