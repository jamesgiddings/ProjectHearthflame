using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleStateManager
{
	private BattleState battleState;

	private PlayerTurn playerTurn;
	private EnemyTurn enemyTurn;
	private BattleStart battleStart;
	private BattleWon battleWon;
	private BattleLost battleLost;
	private BattleOver battleOver;

	public PlayerTurn PlayerTurn => playerTurn;
	public EnemyTurn EnemyTurn => enemyTurn;
	public BattleStart BattleStart => battleStart;
	public BattleWon BattleWon => battleWon;
	public BattleLost BattleLost => battleLost;
	public BattleOver BattleOver => battleOver;

	public BattleState BattleState
	{
		get { return battleState; }
		set { battleState = value; }
	}

	public BattleStateManager(BattleManager battleManager)
	{
		InitialiseBattleStates(battleManager);
	}
	
	private void InitialiseBattleStates(BattleManager battleManager)
	{
		playerTurn = new PlayerTurn(battleManager);
		enemyTurn = new EnemyTurn(battleManager);
		battleStart = new BattleStart(battleManager);
		battleWon = new BattleWon(battleManager);
		battleLost = new BattleLost(battleManager);
		battleOver = new BattleOver(battleManager);
	}

	public void ChangeState(BattleState newBattleState)
	{
		if (battleState != null)
		{
			battleState.ExitState();
		}
		newBattleState.EnterState();
	}

	public void HandleInput()
	{
		battleState.HandleInput();
	}

}
