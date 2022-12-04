using System;
using UnityEngine;

public class GameStateManager : MonoBehaviour
{
	private State gameState;

	private GameBattleState gameBattleState;
	private GameExplorationState gameExplorationState;
	private GameMenuState menuState;

    public GameBattleState GameBattleState => gameBattleState;
    public GameExplorationState GameExplorationState => gameExplorationState;

	public GameMenuState MenuState => menuState;

    public State GameState
	{
		get {
				if (gameState == null)
				{
					Debug.LogError("gameState is null.");
				}
				return gameState;
		}
		set { gameState = value; }
	}

	public void Awake()
	{
        InitialiseGameStates();
        SetStartingState();
    }

	private void SetStartingState()
    {
		 ChangeState(gameExplorationState);
    }

/*    public void Update() // TODO, now states are monobehaviours, this is probably not needed
    {
		gameState.Update();
	}*/

	private void InitialiseGameStates()
	{
		gameBattleState = GetComponentInChildren<GameBattleState>(true);
		gameExplorationState = GetComponentInChildren<GameExplorationState>(true);
		menuState = GetComponentInChildren<GameMenuState>(true);
    }

	public void ChangeState(State newGameState)
	{
		if (gameState != null)
		{
			gameState.ExitState();
			gameState.gameObject.SetActive(false);
		}
        newGameState.EnterState();
        newGameState.gameObject.SetActive(true);
        gameState = newGameState;
	}

	public void HandleInput()
	{
		gameState.HandleInput();
	}
}