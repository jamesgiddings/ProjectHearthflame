using GramophoneUtils.Utilities;
using UnityEngine;

[CreateAssetMenu(fileName = "State Manager", menuName = "States/State Manager")]
public class StateManager : ScriptableObjectThatCanRunCoroutines
{
    #region Attributes/Fields/Properties

    [SerializeField]
    private State _startingState;
    public State StartingState => _startingState;

    private State _state;
    public State State
    {
        get
        {
            if (_state == null)
            {
                Debug.LogError("state is null.");
            }
            return _state;
        }
        set { _state = value; }
    }

    [SerializeField]
    private State[] _states;

    public State[] States => _states;

    #endregion

    #region Callbacks

    private void OnDisable()
    {
        _state = null;
    }

    #endregion

    #region Public Functions

    public void SetStartingState()
    {
        ChangeState(_startingState);
    }

    public void ChangeState(State newState)
    {
        if (_state != null)
        {
            _state.ExitState();
        }
        newState.EnterState();
        _state = newState;
    }

    public void HandleInput()
    {
        _state.HandleInput();
        if (_state.SubStateManager != null)
        {
            _state.SubStateManager.State.HandleInput();
        }
    }


    #endregion
}
