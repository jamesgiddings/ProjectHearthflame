public class BattleOverState : BattleSubState
{
	public BattleOverState(BattleManager battleManager)
	{
		base.BattleManager = battleManager;
	}

	public override void EnterState()
	{
		base.EnterState();
		// End the game
		BattleManager.EndBattle();
		ExitState();
	}

	public override void ExitState()
	{
		base.ExitState();
		//GameManager.Instance.BattleSceneLoaded -= battleManager.SetIsBattleFullyLoaded;
	}

	public override void HandleInput()
	{
		//throw new NotImplementedException();
	}
}
