using AYellowpaper;
using GramophoneUtils.Events.CustomEvents;
using GramophoneUtils.Events.Listeners;
using GramophoneUtils.Utilities;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEditor;
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

    [BoxGroup("Event Tasks")]
    [SerializeField] private List<InterfaceReference<ITask>> _tasks;

    [BoxGroup("Default Exit State On Condition"), Tooltip("The state to exit to.")]
    [SerializeField] private State _exitState;
    [BoxGroup("Default Exit State On Condition"), Tooltip("The state manager of the state to exit to.")]
    [SerializeField] private StateManager _exitStateStateManager;
    [BoxGroup("Default Exit State On Condition"), Tooltip("The time in seconds before exiting the state.")]
    [SerializeField] private FloatReference _delay;

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

        OnEnterTasksComplete();

        StartTimerIfDelayedExitEventSet();
    }

    public virtual void ExitState()
    {
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

#if UNITY_EDITOR

    [Button("Create State Listeners")]
    public void CreateStateListeners(string stateName)
    {
        ScriptableObjectStateListener enterStateListener = CreateInstance(typeof(ScriptableObjectStateListener)) as ScriptableObjectStateListener;
        ScriptableObjectStateListener exitStateListener = CreateInstance(typeof(ScriptableObjectStateListener)) as ScriptableObjectStateListener;
        string path = "Assets/Resources/GameEvents/States/Listeners/";
        string exitAssetPath = AssetDatabase.GenerateUniqueAssetPath(path + "/Exit " + stateName + " State Listener.asset");
        string enterAssetPath = AssetDatabase.GenerateUniqueAssetPath(path + "/Enter " + stateName + " State Listener.asset");
        Debug.Log(exitAssetPath);
        Debug.Log(enterAssetPath);
        AssetDatabase.CreateAsset(enterStateListener, enterAssetPath);
        AssetDatabase.CreateAsset(exitStateListener, exitAssetPath);
    }

#endif

    #endregion

    #region Private Functions

    private async void OnEnterTasksComplete()
    {
        //var tasks = new Task[_tasks.Count];

        for (int i = 0; i < _tasks.Count; i++)
        {
            //tasks[i] = _tasks[i].Value.Execute();
            await _tasks[i].Value.Execute();
        }

        //await Task.WhenAll(tasks);
    }

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

    private IEnumerator ChangeStateAfterDelayCoroutine(float delay, State state, StateManager stateManager)
    {
        yield return new WaitForSeconds(delay);
        stateManager.ChangeState(state);
    }

    #endregion

}
