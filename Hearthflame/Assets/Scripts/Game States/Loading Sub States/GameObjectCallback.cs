using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectCallback : State
{
    #region API

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
    }

    public void AfterAllAwakeCallbacks()
    {
        Debug.Log("2");
    }

    public void AfterAllStartCallbacks()
    {
        Debug.Log("4");
        StartCoroutine(ChangeStateAfterDelay(0.1f));
    }

    #endregion

    #region Utilities

    private IEnumerator ChangeStateAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        ServiceLocator.Instance.LoadingStateManager.ChangeState(ServiceLocator.Instance.InstantiatingCharactersState);
    }

    #endregion
}
