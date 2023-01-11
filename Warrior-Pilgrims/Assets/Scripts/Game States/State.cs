using AYellowpaper;
using GramophoneUtils.Events.CustomEvents;
using GramophoneUtils.Utilities;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "State", menuName = "States/State")]
public class State : ScriptableObjectThatCanRunCoroutines {

    #region Attributes/Fields/Properties

#if UNITY_EDITOR
    [ShowInInspector, TextArea] private string _stateFunctionDescription;
#endif


    [SerializeField] private StateEvent enterStateEvent;
    [SerializeField] private StateEvent exitStateEvent;

    [SerializeField] private StateManager _subStateManager = null;
    public StateManager SubStateManager => _subStateManager;

    [BoxGroup("Exit State After Delay")]
    [SerializeField] private UnityEvent<float, State, StateManager> _stateExitTimerEvent;

    [BoxGroup("Invoke Event After Delay")]
    [SerializeField] private UnityEvent<float> _delayedCommandsEvent;

    [BoxGroup("Default Exit State On Condition"), Tooltip("The state to exit to.")]
    [SerializeField] private State _exitState;
    [BoxGroup("Default Exit State On Condition"), Tooltip("The state manager of the state to exit to.")]
    [SerializeField] private StateManager _exitStateStateManager;
    [BoxGroup("Default Exit State On Condition"), Tooltip("The time in seconds before exiting the state.")]
    [SerializeField] private FloatReference _delay;

    [BoxGroup("Delayed ICommands"), Tooltip("The time in seconds before executing the ICommand.")]
    [SerializeField] private float _eventDelay;
    [BoxGroup("Delayed ICommands"), Tooltip("The commands to execute after the delay.")]
    [SerializeField] private List<InterfaceReference<ICommand>> _delayedCommands;

    [SerializeField] private List<UnityEvent> _onAfterEnterUnityEvents;

    [SerializeField] private List<UnityEvent> _onBeforeExitUnityEvents;

    private bool _active = false;

    #endregion

    #region Callbacks

    private void OnEnable()
    {
        SetActive(false);
    }

    #endregion

    #region API

    public virtual void HandleInput() {} 

    public virtual void EnterState()
    {
        EnterSubState();

        SetActive(true);

        RaiseEnterStateEvent();

        InvokeAfterEnterEvents();

        StartTimerIfDelayedEventSet();
        StartTimerIfDelayedExitEventSet();
    }


    public virtual void ExitState()
    {
        InvokeBeforeExitEvents();

        RaiseExitStateEvent();

        SetActive(false);

        ExitSubState();
    }

    public bool IsActive()
    {
        return _active;
    }

    public void SetActive(bool value)
    {
        _active = value;
    }

    public void ChangeStateAfterDelay(float delay, State state, StateManager stateManager)
    {
        if (IsActive())
        {
            StartCoroutine(ChangeStateAfterDelayCoroutine(delay, state, stateManager));
        }
    }

    public void ChangeStateAfterDelay()
    {
        if (IsActive())
        {
            StartCoroutine(ChangeStateAfterDelayCoroutine(_delay, _exitState, _exitStateStateManager));
        }
    }

    public void ExecuteCommandsAfterDelay()
    {
        if (IsActive())
        {
            StartCoroutine(ExecuteCommandsAfterDelayCoroutine(_eventDelay));
        }
    }

    public void Wait(float seconds)
    {
        if (IsActive())
        {
            StartCoroutine(WaitCoroutine(seconds));
        }
    }

    public void SetCharacterMovement(bool value)
    {
        FindObjectsOfType<Battler>().ForEach((b) => b.gameObject.GetComponent<CharacterMovement>().enabled = value);
    }

    #endregion

    #region Private Functions

    private IEnumerator WaitCoroutine(float seconds)
    {
        yield return new WaitForSeconds(seconds);
    }

    private void StartTimerIfDelayedExitEventSet()
    {
        if (_stateExitTimerEvent != null)
        {
            _stateExitTimerEvent?.Invoke(_delay, _exitState, _exitStateStateManager);
        }
    }

    private void StartTimerIfDelayedEventSet()
    {
        if (_delayedCommandsEvent != null)
        {
            _delayedCommandsEvent?.Invoke(_eventDelay);
        }
    }

    private void RaiseEnterStateEvent()
    {
        if (enterStateEvent != null)
        {
            enterStateEvent.Raise(this);
        }
        else
        {
            Debug.LogError("enterStateEvent not set");
        }
    }

    private void EnterSubState()
    {
        if (_subStateManager != null)
        {
            if (_subStateManager.StartingState != null)
            {
                _subStateManager.ChangeState(_subStateManager.StartingState);
            }
            else
            {
                Debug.LogError("SubStateManager.StartingState is null, so cannot enter.");
            }
        }
    }

    private void InvokeAfterEnterEvents()
    {
        if (IsActive())
        {
            for (int i = 0; i < _onAfterEnterUnityEvents.Count; i++)
            {
                _onAfterEnterUnityEvents[i]?.Invoke();
            }
        }
    }

    private void ExitSubState()
    {
        if (_subStateManager != null)
        {
            if (_subStateManager.State != null)
            {
                _subStateManager.State.ExitState();
            }
            else
            {
                Debug.LogError("SubStateManager.State is null, so cannot exit.");
            }
        }
    }

    private void RaiseExitStateEvent()
    {
        if (exitStateEvent != null)
        {
            exitStateEvent.Raise(this);
        }
        else
        {
            Debug.LogError("enterStateEvent not set");
        }
    }

    private void InvokeBeforeExitEvents()
    {
        if (IsActive())
        {
            for (int i = 0; i < _onBeforeExitUnityEvents.Count; i++)
            {
                _onBeforeExitUnityEvents[i]?.Invoke();
            }
        }
    }

    private IEnumerator ChangeStateAfterDelayCoroutine(float delay, State state, StateManager stateManager)
    {
        yield return new WaitForSeconds(delay);
        stateManager.ChangeState(state);
    }

    private IEnumerator ExecuteCommandsAfterDelayCoroutine(float delay)
    {
        yield return new WaitForSeconds(delay);
        foreach (InterfaceReference<ICommand> interfaceReference in _delayedCommands)
        {
            if(interfaceReference.Value != null)
            {
                interfaceReference.Value.Execute();
            }
        }
    }

    #endregion

}
