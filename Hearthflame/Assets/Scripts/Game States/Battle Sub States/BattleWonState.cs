using UnityEngine;

public class BattleWonState : BattleSubState
{
	public BattleWonState(BattleManager battleManager)
	{
		base.BattleManager = battleManager;
	}

	public override void EnterState()
	{
		base.EnterState();
		// get the BattleReward class to show its popup 
		BattleManager.Battle.BattleReward.AddBattleReward(ServiceLocator.Instance.CharacterModel);
		BattleManager.BattleDataModel.OnBattleRewardsEarned?.Invoke(BattleManager.Battle.BattleReward);

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
			ServiceLocator.Instance.BattleStateManager.ChangeState(ServiceLocator.Instance.BattleOverState);
		}
	}
}