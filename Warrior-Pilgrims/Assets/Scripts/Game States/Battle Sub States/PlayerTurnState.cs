using GramophoneUtils.Characters;
using GramophoneUtils.Stats;
using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Player Turn State", menuName = "States/Battle Sub States/Player Turn State")]
public class PlayerTurnState : BattleSubState
{
	private Character StateActor;

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
        if (ServiceLocator.Instance.PlayerInputBehaviour.CancelAction.triggered)
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

        if (ServiceLocator.Instance.PlayerInputBehaviour.AcceptAction.triggered)
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
        //BattleManager.BattleDataModel.NextTurn();
        ServiceLocator.Instance.BattleStateManager.ChangeState(ServiceLocator.Instance.ServiceLocatorObject.PostCharacterTurnState);

	}
}