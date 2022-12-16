using GramophoneUtils.Events.CustomEvents;
using Sirenix.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBattleState : State
{
    #region API

    public override void EnterState()
    {
        base.EnterState();
        SetCharacterMovement(false);
    }
        
    public override void ExitState() 
    {
        SetCharacterMovement(true);
        base.ExitState();
    }

    public override void HandleInput() {}

    #endregion

    #region Utilities

    private void SetCharacterMovement(bool value)
    {
        FindObjectsOfType<Battler>().ForEach((b) => b.gameObject.GetComponent<CharacterMovement>().enabled = value);
    }
    #endregion
}
