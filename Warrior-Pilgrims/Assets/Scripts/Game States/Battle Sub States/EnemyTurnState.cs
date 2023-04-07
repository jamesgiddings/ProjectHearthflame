using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy Turn State", menuName = "States/Battle Sub States/Enemy Turn State")]
public class EnemyTurnState : CharacterTurnState
{
	private IEnumerator EnemyAction()
	{
        yield return new WaitForSeconds(ServiceLocatorObject.Instance.Constants.BattleShortDelay);

		ISkill enemySkill = BattleManager.BattleDataModel.CurrentActor.Brain.ChooseSkill(BattleManager.BattleDataModel.CurrentActor);

		if (enemySkill != null)
		{
			BattleManager.TargetManager.GetCurrentlyTargeted(enemySkill, BattleManager.BattleDataModel.CurrentActor);
			if (BattleManager.TargetManager.CurrentTargetsCache.Count > 0)
				BattleManager.BattleDataModel.OnSkillUsed?.Invoke(BattleManager.BattleDataModel.CurrentActor);
		}

		ServiceLocator.Instance.CharacterGameObjectManager.CharacterBattlerDictionary[StateActor].OnTurnComplete += EndOfEnemyAction;
	}

	private void EndOfEnemyAction()
	{
        ServiceLocator.Instance.CharacterGameObjectManager.CharacterBattlerDictionary[StateActor].OnTurnComplete -= EndOfEnemyAction;
        ServiceLocator.Instance.CharacterGameObjectManager.UpdatePlayerBattlers(); // TODO, this should be an event
        ServiceLocator.Instance.CharacterGameObjectManager.UpdateEnemyBattlers(); // TODO, this should be an event
        ServiceLocator.Instance.CharacterGameObjectManager.MovePlayerBattlersForward(); // TODO, this should be an event
        ServiceLocator.Instance.CharacterGameObjectManager.MoveEnemyBattlersForward(); // TODO, this should be an event
        ServiceLocator.Instance.BattleStateManager.ChangeState(ServiceLocator.Instance.ServiceLocatorObject.PostCharacterTurnState);
    }

	public override void EnterState()
	{
		base.EnterState();
        if (CharacterNotInControl)
        {
            return;
        }
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