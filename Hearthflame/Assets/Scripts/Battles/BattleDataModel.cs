using GramophoneUtils.Stats;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BattleDataModel
{
	private BattleManager battleManager;
	private BattleStateManager battleStateManager;
	private Battle battle;
	private BattleBehaviour battleBehaviour;
	private PlayerBehaviour playerBehaviour;

	private List<Character> playerCharacters;
	private List<Character> enemyCharacters;

	private List<Character> battlersList;
	private List<Character> playerBattlersList;
	private List<Character> enemyBattlersList;
	private List<Character> orderedBattlersList;
	private Queue<Character> unresolvedQueue;
	private Queue<Character> resolvedQueue;

	private Character currentActor;

	public Action OnCurrentActorChanged;
	public Action<BattleReward> OnBattleRewardsEarned;

	public Action<Character> OnSkillUsed;

	private int turn;
	private int round;
	public List<Character> EnemyBattlersList => enemyBattlersList;
	public List<Character> PlayerBattlersList => playerBattlersList;
	public List<Character> BattlersList => battlersList;
	public List<Character> OrderedBattlersList => orderedBattlersList;

	public List<Character> PlayerCharacters => playerCharacters;
	public List<Character> EnemyCharacters => enemyCharacters;

	public Character CurrentActor
	{
		get { return currentActor; }
		set { currentActor = value; }
	}

	public BattleDataModel(BattleManager battleManager, Battle battle, BattleBehaviour battleBehaviour)
	{
		this.battleManager = battleManager;
		this.battle = battle;
		this.battleBehaviour = battleBehaviour;
		this.battleStateManager = battleManager.BattleStateManager;

		this.battle = battle;
		this.playerBehaviour = battleManager.PlayerBehaviour;
		this.playerCharacters = playerBehaviour.PlayerCharacters;
		this.enemyCharacters = battle.BattleCharacters;
	}
	
	public void InitialiseBattleModel()
	{
		InitialiseBattlersLists();

		turn = 0;
		round = 0;

		CreateNewRoundQueues();
		unresolvedQueue = OrderQueue();

		battleManager.InitialiseBattlersInventory();
		battleManager.InitialisePlayerBattlersInventory();
		battleManager.InitialiseEnemyBattlersInventory();
		CalculateOrderedBattlersList();
		battleManager.CalculateAndInitialiseOrderedBattlersInventory();

		CurrentActor = unresolvedQueue.Dequeue();
		CurrentActor.SetIsCurrentActor(true);
		resolvedQueue.Enqueue(CurrentActor);
	}

	private void CreateNewRoundQueues()
	{
		unresolvedQueue = new Queue<Character>();
		foreach (Character character in battlersList)
		{
			unresolvedQueue.Enqueue(character);
		}
		resolvedQueue = new Queue<Character>();
	}

	private void InitialiseBattlersLists()
	{
		battlersList = new List<Character>();
		playerBattlersList = new List<Character>();
		enemyBattlersList = new List<Character>();

		Debug.Log(playerCharacters.Count);
		foreach (Character character in playerCharacters)
		{
			if (character != null)
			{
				Debug.Log(playerCharacters.Count);
				if (character.IsUnlocked) // && IsFront
				{
					battlersList.Add(character);
					playerBattlersList.Add(character);
				}
			}
		}

		List<Character> enemyCharacters = battle.BattleCharacters;

		foreach (Character character in enemyCharacters)
		{
			battlersList.Add(character);
			enemyBattlersList.Add(character);
		}
	}

	public void NextRound()
	{
		round++;
		CreateNewRoundQueues();
		unresolvedQueue = OrderQueue();

		CalculateOrderedBattlersList();
		battleManager.CalculateAndInitialiseOrderedBattlersInventory();
		turn = 0;
	}
		private Queue<Character> OrderQueue()
	{
		IEnumerable<Character> query = unresolvedQueue.OrderBy(character => character.StatSystem.GetStatValue(character.StatTypeStringRefDictionary["Speed"]) * -1); // * - 1 reverses the order of the list
		Queue<Character> orderedRoundQueue = new Queue<Character>();
		foreach (Character battler in query)
		{
			orderedRoundQueue.Enqueue(battler);
		}
		CalculateOrderedBattlersList();
		battleManager.CalculateAndInitialiseOrderedBattlersInventory();
		return orderedRoundQueue;
	}

	private List<Character> CalculateOrderedBattlersList()
	{
		orderedBattlersList = new List<Character>();
		foreach (Character battler in resolvedQueue)
		{
			orderedBattlersList.Add(battler);
		}
		foreach (Character battler in unresolvedQueue)
		{
			orderedBattlersList.Add(battler);
		}
		return orderedBattlersList;
	}

	private bool IsPlayerVictory()
	{
		bool allDead = true;
		foreach(Character character in enemyBattlersList)
		{
			if (!character.HealthSystem.IsDead)
			{
				allDead = false;
			}
		}

		return allDead;
	}

	private bool IsEnemyVictory()
	{
		bool allDead = true;
		foreach (Character character in playerBattlersList)
		{
			if (!character.HealthSystem.IsDead)
			{
				allDead = false;
			}
		}
		return allDead;
	}

	public void NextTurn()
	{
		if (IsPlayerVictory())
		{
			battleStateManager.ChangeState(battleManager.BattleStateManager.BattleWon);
			return;
		}
		else if (IsEnemyVictory())
		{
			battleStateManager.ChangeState(battleManager.BattleStateManager.BattleLost);
			return;
		}

		turn++;
		Turn.AdvanceTurn(); // this sends off events that tick forward stat modifiers and effects

		unresolvedQueue = OrderQueue();

		if (turn >= orderedBattlersList.Count)
		{
			NextRound();
		}

		CurrentActor = unresolvedQueue.Dequeue();
		resolvedQueue.Enqueue(CurrentActor);
		OnCurrentActorChanged?.Invoke();
		UpdateState();
	}

	public void UpdateState()
	{
		if (CurrentActor.IsPlayer)
		{
			battleStateManager.ChangeState(battleStateManager.PlayerTurn);
		}
		else if (!CurrentActor.IsPlayer)
		{
			battleStateManager.ChangeState(battleStateManager.EnemyTurn);
		}
	}
}
