using GramophoneUtils.Stats;
using System;
using UnityEngine;

public class PlayerTurn : BattleState
{
	private Character StateActor;

	public PlayerTurn(BattleManager battleManager)
	{
		base.battleManager = battleManager;
	}

	public override void EnterState()
	{
		base.EnterState();
		StateActor = battleManager.BattleDataModel.CurrentActor;
		Debug.Log("We're in the player state, and the current actor is: " + StateActor.Name);
		Debug.Log("StateActor.IsCurrentActor. Should be false: " + StateActor.IsCurrentActor);
		StateActor.SetIsCurrentActor(true);
		Debug.Log("We're in the player state, and the current actor is: " + StateActor.Name);
		Debug.Log("StateActor.IsCurrentActor.  Should be true: " + StateActor.IsCurrentActor);
		battleManager.InitialiseRadialMenu();
		if (battleManager.BattleDataModel.CurrentActor.HealthSystem.IsDead)
		{
			battleManager.BattleDataModel.NextTurn();
		}
	}

	public override void ExitState()
	{
		base.ExitState();
		Debug.Log("We're leaving the player state, and the old actor is: " + StateActor.Name);
		Debug.Log("The old Actor state should still be true. Is it? :- " + StateActor.IsCurrentActor);
		StateActor.SetIsCurrentActor(false);
		Debug.Log("The old Actor state should now be false. Is it? :- " + StateActor.IsCurrentActor);
	}

	public override void HandleInput()
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{
			PlayerAction();
		}

		if (battleManager.TargetManager.IsTargeting)
		{
			if (Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.DownArrow) || Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.RightArrow))
			{
				if (Math.Abs(Input.GetAxis("Vertical")) > 0 || (Math.Abs(Input.GetAxis("Horizontal")) > 0))
				{
					battleManager.ChangeTargets();
				}
			}
		}

		if (Input.GetKeyUp(KeyCode.Return))
		{
			if (battleManager.TargetManager.IsTargeting && battleManager.TargetManager.CurrentTargetsCache.Count > 0)
			{
				battleManager.BattleDataModel.OnSkillUsed?.Invoke(StateActor);
				PlayerAction();
			}
		}
	}

	private void PlayerAction()
	{
		battleManager.BattleDataModel.NextTurn();
		battleManager.BattleDataModel.OnCurrentActorChanged?.Invoke();
	}

}