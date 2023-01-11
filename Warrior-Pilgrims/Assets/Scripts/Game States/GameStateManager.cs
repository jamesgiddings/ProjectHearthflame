using System;
using UnityEngine;

public class GameStateManager : StateManager
{
	[SerializeField] private GameBattleState _gameBattleState;
    [SerializeField] private GameExplorationState gameExplorationState;
    [SerializeField] private GameMenuState menuState;

	private State gameState;

    public GameBattleState GameBattleState => _gameBattleState;
    public GameExplorationState GameExplorationState => gameExplorationState;

	public GameMenuState MenuState => menuState;

    public State GameState
	{
		get 
		{
			if (gameState == null)
			{
				Debug.LogError("gameState is null.");
			}
			return gameState;
		}
		set 
		{
			gameState = value; 
		}
	}

	public void ChangeState(State newGameState)
	{

		if (gameState != null)
		{
			gameState.ExitState();
			gameState.SetActive(false);
		}
        newGameState.EnterState();
        newGameState.SetActive(true);
        gameState = newGameState;
	}

	public void HandleInput()
	{
		gameState.HandleInput();
	}
}