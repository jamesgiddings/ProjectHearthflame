using GramophoneUtils.Events.CustomEvents;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This is an object which should be placed at the very bottom of the scene hierarchy
/// in order to raise events during the different callbacks, so that the loading states
/// know when they have all been called. 
/// </summary>
public class LoadingStateCallbackReference : MonoBehaviour
{
    [SerializeField] VoidEvent OnAllAwakeCalled;
    [SerializeField] VoidEvent OnAllStartCalled;

    #region Callbacks

    private void Awake()
    {
        OnAllAwakeCalled?.Raise();
    }

    void Start()
    {
        OnAllStartCalled?.Raise();
    }


    void Update()
    {
        
    }

    #endregion
}
