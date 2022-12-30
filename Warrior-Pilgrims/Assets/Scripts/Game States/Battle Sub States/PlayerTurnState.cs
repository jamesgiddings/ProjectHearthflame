using GramophoneUtils.Characters;
using GramophoneUtils.Stats;
using System;
using UnityEngine;

public class PlayerTurnState : BattleSubState
{
	private Character StateActor;
	
	public PlayerTurnState(BattleManager battleManager)
	{
		base.BattleManager = battleManager;
	}

	public override void EnterState()
	{
		base.EnterState();
		StateActor = BattleManager.BattleDataModel.CurrentActor;
        BattleManager.InitialiseRadialMenu();
		if (StateActor.HealthSystem.IsDead)
		{
			BattleManager.BattleDataModel.NextTurn();
		}
	}

	public override void ExitState()
	{
        base.ExitState();
	}

	public override void HandleInput()
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{
			PlayerAction();
		}

		if (BattleManager.TargetManager.IsTargeting)
		{
			if (Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.DownArrow) || Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.RightArrow))
			{
				if (Math.Abs(Input.GetAxis("Vertical")) > 0 || (Math.Abs(Input.GetAxis("Horizontal")) > 0))
				{
					BattleManager.ChangeTargets();
				}
			}
		}

		if (Input.GetKeyUp(KeyCode.Return))
		{
			if (BattleManager.TargetManager.IsTargeting && BattleManager.TargetManager.CurrentTargetsCache.Count > 0)
			{
				BattleManager.BattleDataModel.OnSkillUsed?.Invoke(StateActor);
                ServiceLocator.Instance.CharacterGameObjectManager.CharacterBattlerDictionary[StateActor].OnTurnComplete += PlayerAction;
			}
		}
	}

	private void PlayerAction()
	{
        ServiceLocator.Instance.CharacterGameObjectManager.CharacterBattlerDictionary[StateActor].OnTurnComplete -= PlayerAction;
		BattleManager.BattleDataModel.NextTurn();
	}
}