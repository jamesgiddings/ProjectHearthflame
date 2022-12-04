using Sirenix.OdinInspector;
using UnityEngine;

public class StateManager : MonoBehaviour
{
    [SerializeField]
    [ChildGameObjectsOnly(IncludeSelf = false)]
    State startingState;
    private State state;

    [SerializeField]
    [ChildGameObjectsOnly(IncludeSelf = false)]
    [InfoBox("States can only be selected from children of the manager")]
    private State[] states;

    public State[] States => states;

    public State State
    {
        get
        {
            if (state == null)
            {
                Debug.LogError("state is null.");
            }
            return state;
        }
        set { state = value; }
    }

    public void OnEnable()
    {
        InitialiseStates();
        SetStartingState();
    }

    private void SetStartingState()
    {
        ChangeState(startingState);
    }

    private void InitialiseStates()
    {
        
    }

    public void ChangeState(State newState)
    {
        if (state != null)
        {
            state.ExitState();
            state.gameObject.SetActive(false);
        }
        newState.gameObject.SetActive(true);
        newState.EnterState();
        state = newState;
    }

    public void HandleInput()
    {
        state.HandleInput();
    }
}
