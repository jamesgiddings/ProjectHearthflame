public class BattleStart : BattleSubState
{
	public BattleStart(BattleManager battleManager)
	{
		this.battleManager = battleManager;
	}
	public override void EnterState()
	{
		base.EnterState();

		//GameManager.Instance.StartCoroutine(
		battleManager.InitialiseBattle();
	}

	public override void ExitState()
	{
		base.ExitState();
	}

	public override void HandleInput()
	{
		//throw new NotImplementedException();
	}
}
