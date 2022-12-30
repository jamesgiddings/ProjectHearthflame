using GramophoneUtils.Events.CustomEvents;
using Sirenix.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameExplorationState : State
{

    #region API

    public override void EnterState()
    {
        base.EnterState();
        SetCharacterMovement(true);
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void HandleInput()
    {
        base.HandleInput();
    }

    #endregion


}
