using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleInitialisationState : BattleSubState
{
    private bool _isCameraInitialised;

    [ShowInInspector] public bool IsCameraInitialised => _isCameraInitialised;

    public override void EnterState()
    {
        base.EnterState();
        StartCoroutine(WaitForInitialisationToComplete());
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void HandleInput() { }

    public void SetIsCameraInitialised(bool value)
    {
        _isCameraInitialised = true;
    }

    private IEnumerator WaitForInitialisationToComplete()
    {
        yield return new WaitUntil(IsInitialisationComplete);
        ServiceLocator.Instance.BattleStateManager.ChangeState(ServiceLocator.Instance.BattleStartState);
    }

    private bool IsInitialisationComplete()
    {
        bool initialisationIsComplete = true;
        if (!_isCameraInitialised)
        {
            initialisationIsComplete = false;
        }
        return initialisationIsComplete;
    }
}
