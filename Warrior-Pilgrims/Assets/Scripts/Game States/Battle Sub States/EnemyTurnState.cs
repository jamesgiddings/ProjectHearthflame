using GramophoneUtils.Characters;
using GramophoneUtils.Stats;
using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy Turn State", menuName = "States/Battle Sub States/Enemy Turn State")]
public class EnemyTurnState : BattleSubState
{
	private Character StateActor;

	private IEnumerator EnemyAction()
	{
		yield return new WaitForSeconds(0.1f); // TODO hack - this is because the change state function called the code below,
		// which meant that it would change to the enemyturnstate, but if the enemy was dead, it would change the turn to player,
		// but the original change state call from playerturnstate was not popped off the stack, so it would then change back to
		// the previous state.

/*        if (BattleManager.BattleDataModel.CurrentActor.HealthSystem.IsDead)
        {
            BattleManager.BattleDataModel.NextTurn(); // Problem? 
        }*/

        yield return new WaitForSeconds(1f);

		ISkill enemySkill = BattleManager.BattleDataModel.CurrentActor.Brain.ChooseSkill(BattleManager.BattleDataModel.CurrentActor);

		if (enemySkill != null)
		{
			BattleManager.TargetManager.GetCurrentlyTargeted(enemySkill, BattleManager.BattleDataModel.CurrentActor);
			if (BattleManager.TargetManager.CurrentTargetsCache.Count > 0)
				BattleManager.BattleDataModel.OnSkillUsed?.Invoke(BattleManager.BattleDataModel.CurrentActor);
		}

		// choose skill
		// get target
		// use skill
		ServiceLocator.Instance.CharacterGameObjectManager.CharacterBattlerDictionary[StateActor].OnTurnComplete += EndOfEnemyAction;
	}

	private void EndOfEnemyAction()
	{
		Debug.Log("EndOfEnemyAction()");
        ServiceLocator.Instance.CharacterGameObjectManager.CharacterBattlerDictionary[StateActor].OnTurnComplete -= EndOfEnemyAction;
        //BattleManager.BattleDataModel.NextTurn();
        ServiceLocator.Instance.BattleStateManager.ChangeState(ServiceLocator.Instance.ServiceLocatorObject.PostCharacterTurnState);
    }

	public override void EnterState()
	{
		base.EnterState();
		StateActor = BattleManager.BattleDataModel.CurrentActor;
		StateActor.SetIsCurrentActor(true);
		StartCoroutine(EnemyAction());
	}

	public override void ExitState()
	{
        StateActor.SetIsCurrentActor(false);
        base.ExitState();
	}

	public override void HandleInput() {}
}