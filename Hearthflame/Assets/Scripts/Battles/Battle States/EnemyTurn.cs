using GramophoneUtils.Stats;
using System.Collections;
using UnityEngine;

public class EnemyTurn : BattleState
{
	private Character StateActor;

	public EnemyTurn(BattleManager battleManager)
	{
		base.battleManager = battleManager;
	}

	private IEnumerator EnemyAction()
	{
		yield return new WaitForSeconds(1f);

		Skill enemySkill = battleManager.BattleDataModel.CurrentActor.Brain.ChooseSkill(battleManager.BattleDataModel.CurrentActor);

		if (enemySkill != null)
		{
			battleManager.TargetManager.GetCurrentlyTargeted(enemySkill, battleManager.BattleDataModel.CurrentActor);
			if (battleManager.TargetManager.CurrentTargetsCache.Count > 0)
				battleManager.BattleDataModel.OnSkillUsed?.Invoke(battleManager.BattleDataModel.CurrentActor);
		}

		// choose skill
		// get target
		// use skill
		battleManager.BattleBehaviour.BattlerDisplayUI.CharacterBattlerDictionary[StateActor].OnTurnComplete += EndOfEnemyAction;
	}

	private void EndOfEnemyAction()
	{
		battleManager.BattleBehaviour.BattlerDisplayUI.CharacterBattlerDictionary[StateActor].OnTurnComplete -= EndOfEnemyAction;
		battleManager.BattleDataModel.NextTurn();
		battleManager.BattleDataModel.OnCurrentActorChanged?.Invoke();
	}

	public override void EnterState()
	{
		base.EnterState();
		StateActor = battleManager.BattleDataModel.CurrentActor;
		StateActor.SetIsCurrentActor(true);
		if (battleManager.BattleDataModel.CurrentActor.HealthSystem.IsDead)
		{
			battleManager.BattleDataModel.NextTurn();
		}
		else
		{
			GameManager.Instance.StartCoroutine(EnemyAction());
		}
	}

	public override void ExitState()
	{
		base.ExitState();
		StateActor.SetIsCurrentActor(false);
	}

	public override void HandleInput()
	{

	}
}