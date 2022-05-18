using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameExplorationState : GameState
{
    public override void EnterState()
    {
        
    }

    public override void ExitState()
    {

    }

    public override void HandleInput()
    {

    }

    public override void Update()
    {
        GameManager.Instance.Movement = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        GameManager.Instance.MovementNormalized = GameManager.Instance.Movement.normalized;
    }
}
