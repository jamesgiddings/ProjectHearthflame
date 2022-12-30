using GramophoneUtils.Events.CustomEvents;
using Sirenix.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State : MonoBehaviour {

    [SerializeField] private StateEvent enterStateEvent;
    [SerializeField] private StateEvent exitStateEvent;

    #region Callbacks

    public void Update()
    {
        HandleInput();
    }

    #endregion

    #region API

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

    #endregion

    #region Utilities

    protected void SetCharacterMovement(bool value)
    {
        FindObjectsOfType<Battler>().ForEach((b) => b.gameObject.GetComponent<CharacterMovement>().enabled = value);
    }

    #endregion

}
