using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitForFade : State
{
    [SerializeField] private float timeInState = 0.3f;

    #region API
    public override void EnterState()
    {
        base.EnterState();
        StartCoroutine(ChangeStateAfterDelay(timeInState));
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

    #region Utilities

    private IEnumerator ChangeStateAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        ServiceLocator.Instance.LoadingStateManager.ChangeState(ServiceLocator.Instance.UnloadingOldScene);
    }

    #endregion
}
