public abstract class BattleState
{
	protected BattleManager battleManager;

	public abstract void HandleInput();

	public virtual void EnterState()
	{
		battleManager.BattleStateManager.BattleState = this;
	}

	public virtual void ExitState()
	{
		battleManager.TargetManager.ClearTargets();
	}
}
