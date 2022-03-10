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

		Skill enemySkill = battleManager.BattleDataModel.CurrentActor.Brain.ChooseSkill();

		if (enemySkill != null)
		{
			battleManager.TargetManager.GetCurrentlyTargeted(enemySkill, battleManager.BattleDataModel.CurrentActor);
			if (battleManager.TargetManager.CurrentTargetsCache.Count > 0)
				battleManager.BattleDataModel.OnSkillUsed?.Invoke(battleManager.BattleDataModel.CurrentActor);
		}

		// choose skill
		// get target
		// use skill

		battleManager.BattleDataModel.NextTurn();
		battleManager.BattleDataModel.OnCurrentActorChanged?.Invoke();
	}

	public override void EnterState()
	{
		base.EnterState();
		StateActor = battleManager.BattleDataModel.CurrentActor;
		Debug.Log("We're in the enemy state, and the current actor is: " + StateActor.Name);
		Debug.Log("StateActor.IsCurrentActor. Should be false: " + StateActor.IsCurrentActor);
		StateActor.SetIsCurrentActor(true);
		Debug.Log("We're in the enemy state, and the current actor is: " + StateActor.Name);
		Debug.Log("StateActor.IsCurrentActor.  Should be true: " + StateActor.IsCurrentActor);
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
		Debug.Log("We're leaving the enemy state, and the old actor is: " + StateActor.Name);
		Debug.Log("The old Actor state should still be true. Is it? :- " + StateActor.IsCurrentActor);
		StateActor.SetIsCurrentActor(false);
		Debug.Log("The old Actor state should now be false. Is it? :- " + StateActor.IsCurrentActor);
	}

	public override void HandleInput()
	{

	}
}