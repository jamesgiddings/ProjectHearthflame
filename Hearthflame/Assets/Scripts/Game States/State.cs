using GramophoneUtils.Events.CustomEvents;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State : MonoBehaviour {

    [SerializeField] private StateEvent enterStateEvent;
    [SerializeField] private StateEvent exitStateEvent;

    public virtual void HandleInput() {}

    public virtual void EnterState() 
    {
        this.gameObject.SetActive(true);
        if (enterStateEvent != null)
        {
            enterStateEvent.Raise(this);
        } else
        {
            Debug.LogError("enterStateEvent not set");
        }
    }

	public virtual void ExitState()
    {
        this.gameObject.SetActive(false);
        if (exitStateEvent != null)
        {
            exitStateEvent.Raise(this);
        }
        else
        {
            Debug.LogError("enterStateEvent not set");
        }
    }

    public void Update()
    {
        HandleInput();
    }
}
