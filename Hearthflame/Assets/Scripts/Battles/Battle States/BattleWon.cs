using UnityEngine;

public class BattleWon : BattleSubState
{
	public BattleWon(BattleManager battleManager)
	{
		base.battleManager = battleManager;
	}

	public override void EnterState()
	{
		base.EnterState();
		// get the BattleReward class to show its popup 
		battleManager.Battle.BattleReward.AddBattleReward(battleManager.PlayerBehaviour);
		battleManager.BattleDataModel.OnBattleRewardsEarned?.Invoke(battleManager.Battle.BattleReward);

	}

	public override void ExitState()
	{
		base.ExitState();
		//throw new NotImplementedException();
		// close the battle and change the scene back to map
	}

	public override void HandleInput()
	{
		//throw new NotImplementedException();
		// allow the player to accept the reward from the battle popup but do nothing else
		if (Input.GetKeyDown(KeyCode.Space))
		{
			battleManager.BattleStateManager.ChangeState(battleManager.BattleStateManager.BattleOver);
		}
	}
}