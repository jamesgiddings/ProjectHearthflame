using System;
using UnityEngine;

public class GameStateManager
{
	private GameState gameState;

	private GameBattleState gameBattleState;
	private GameExplorationState gameExplorationState;

	public GameBattleState GameBattleState => gameBattleState;

	public GameState GameState
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

	public GameStateManager()
	{
		InitialiseGameStates();
		SetStartingState();
	}

    private void SetStartingState()
    {
		 ChangeState(gameExplorationState);
    }

    public void Update()
    {
		gameState.Update();
	}

	private void InitialiseGameStates()
	{
		gameBattleState = new GameBattleState();
		gameExplorationState = new GameExplorationState();
	}

	public void ChangeState(GameState newGameState)
	{
		if (gameState != null)
		{
			gameState.ExitState();
		}
		newGameState.EnterState();
		gameState = newGameState;
	}

	public void HandleInput()
	{
		gameState.HandleInput();
	}
}