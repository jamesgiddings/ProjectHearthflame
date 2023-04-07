using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Player Turn State", menuName = "States/Battle Sub States/Player Turn State")]
public class PlayerTurnState : CharacterTurnState
{
    #region Attributes/Fields/Properties

    #endregion

    #region Public Functions

    public override void EnterState()
	{
        base.EnterState();
    }

	public override void ExitState()
	{
        base.ExitState();
	}

	public override void HandleInput()
	{
        base.HandleInput();
        if (CharacterNotInControl)
        {
            return;
        }

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

#if UNITY_EDITOR

    public void SimulatePlayerAction(ISkill skill, List<ICharacter> targets, ICharacter originator)
    {
        BattleManager.TargetManager.SimulatePlayerTargeting(skill, targets, originator);
        ServiceLocator.Instance.BattleStateManager.ChangeState(ServiceLocator.Instance.ServiceLocatorObject.PostCharacterTurnState);
    }

#endif


#endregion

    #region Private Functions

    private void PlayerAction()
	{
        ServiceLocator.Instance.CharacterGameObjectManager.CharacterBattlerDictionary[StateActor].OnTurnComplete -= PlayerAction;
        ServiceLocator.Instance.BattleStateManager.ChangeState(ServiceLocator.Instance.ServiceLocatorObject.PostCharacterTurnState);
	}

    #endregion 
}