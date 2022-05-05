public class GameStateManager
{
	private GameState gameState;

	private GameBattleState gameBattleState;

	public GameBattleState GameBattleState => gameBattleState;

	public GameState GameState
	{
		get { return gameState; }
		set { gameState = value; }
	}

	public GameStateManager()
	{
		InitialiseGameStates();
	}

	private void InitialiseGameStates()
	{
		gameBattleState = new GameBattleState();
	}

	public void ChangeState(GameState newGameState)
	{
		if (gameState != null)
		{
			gameState.ExitState();
		}
		newGameState.EnterState();
	}

	public void HandleInput()
	{
		gameState.HandleInput();
	}
}