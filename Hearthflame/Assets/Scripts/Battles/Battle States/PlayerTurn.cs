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
		StateActor.SetIsCurrentActor(true);
		battleManager.InitialiseRadialMenu();
		if (battleManager.BattleDataModel.CurrentActor.HealthSystem.IsDead)
		{
			battleManager.BattleDataModel.NextTurn();
		}
	}

	public override void ExitState()
	{
		base.ExitState();
		StateActor.SetIsCurrentActor(false);
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
				battleManager.BattleBehaviour.BattlerDisplayUI.CharacterBattlerDictionary[StateActor].OnTurnComplete += PlayerAction;
			}
		}
	}

	private void PlayerAction()
	{
		battleManager.BattleBehaviour.BattlerDisplayUI.CharacterBattlerDictionary[StateActor].OnTurnComplete -= PlayerAction;
		battleManager.BattleDataModel.NextTurn();
		battleManager.BattleDataModel.OnCurrentActorChanged?.Invoke();
	}
}