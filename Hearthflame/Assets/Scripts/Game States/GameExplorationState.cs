using GramophoneUtils.Events.CustomEvents;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameExplorationState : State
{

    public override void EnterState()
    {
        base.EnterState();
    }

    public override void ExitState()
    {
        base.ExitState();
        GameManager.Instance.Movement = new Vector2(0, 0);
        GameManager.Instance.MovementNormalized = GameManager.Instance.Movement.normalized;
    }

    public override void HandleInput()
    {
        base.HandleInput();
    }

    public void Update()
    {
        //GameManager.Instance.Movement = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        // TODO Hack
        GameManager.Instance.Movement = new Vector2(Input.GetAxisRaw("Horizontal"), 0);

        GameManager.Instance.MovementNormalized = GameManager.Instance.Movement.normalized;
    }
}
