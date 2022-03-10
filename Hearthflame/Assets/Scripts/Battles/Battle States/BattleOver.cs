public class BattleOver : BattleState
{
	public BattleOver(BattleManager battleManager)
	{
		base.battleManager = battleManager;
	}

	public override void EnterState()
	{
		base.EnterState();
		// End the game
		battleManager.EndBattle();
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
