public class BattleLostState : BattleSubState
{
	public BattleLostState(BattleManager battleManager)
	{
		base.BattleManager = battleManager;
	}

	public override void EnterState()
	{
		base.EnterState();
	}

	public override void ExitState()
	{
		base.ExitState();
	}

	public override void HandleInput() {}
}