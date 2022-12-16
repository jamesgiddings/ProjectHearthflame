using GramophoneUtils.Battles;
using GramophoneUtils.Stats;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BattleDataModel : MonoBehaviour
{
	private BattleManager battleManager;
	private StateManager battleStateManager;
	private Battle battle;
	private BattleRear _battleRear;
	private BattleBehaviour battleBehaviour;
	private PlayerModel playerModel;

	private List<Character> playerCharacters;
	private List<Character> enemyCharacters;

	private List<Character> battlersList;
	private List<Character> playerBattlersList;
	private List<Character> rearPlayerBattlersList;
	private List<Character> enemyBattlersList;
	private List<Character> orderedBattlersList;
	private Queue<Character> unresolvedQueue = new Queue<Character>();
	private Queue<Character> resolvedQueue = new Queue<Character>();

    private Character currentActor;

	public Action OnCurrentActorChanged;
	public Action<BattleReward> OnBattleRewardsEarned;

	public Action<Character> OnSkillUsed;

    private int _turn;
    private int _round;

    public BattleRear BattleRear => _battleRear;

    public List<Character> EnemyBattlersList => enemyBattlersList;
	public List<Character> PlayerBattlersList => playerBattlersList;
	public List<Character> BattlersList => battlersList;
	public List<Character> OrderedBattlersList => orderedBattlersList;

	public List<Character> PlayerCharacters => playerCharacters;
	public List<Character> EnemyCharacters => enemyCharacters;

    [ShowInInspector] public int CurrentTurn => _turn;
    [ShowInInspector] public int CurrentRound => _round;

	public Character CurrentActor
	{
		get { return currentActor; }
		set { currentActor = value; }
	}
		
	public void InitialiseBattleModel()
	{
		OnCurrentActorChanged = null; // TODO, hack, because there was a memory leak from battler
		OnSkillUsed = null; // TODO, hack as above

        battleManager = ServiceLocator.Instance.BattleManager;
        battleBehaviour = battleManager.BattleBehaviour;
		battleStateManager = ServiceLocator.Instance.BattleStateManager;
        playerModel =  ServiceLocator.Instance.PlayerModel;
        battle = ServiceLocator.Instance.BattleManager.Battle;
		_battleRear = ServiceLocator.Instance.BattleManager.Battle.BattleRear;
        this.playerCharacters = playerModel.PlayerCharacters;
		this.rearPlayerBattlersList = playerModel.RearCharacters;
        this.enemyCharacters = battle.BattleCharacters;
        InitialiseBattlersLists();

		_turn = 0;
		_round = 0;

		CreateNewRoundQueues();
		unresolvedQueue = OrderQueue();

		battleManager.InitialiseBattlersInventory();
		battleManager.InitialisePlayerBattlersInventory();
		battleManager.InitialiseEnemyBattlersInventory();
		CalculateOrderedBattlersList();
		battleManager.CalculateAndInitialiseOrderedBattlersInventory();

		UpdateCurrentActor();
	}

	private void UpdateCurrentActor()
	{
		if (currentActor != null)
		{
			currentActor.SetIsCurrentActor(false);
		}
        currentActor = unresolvedQueue.Dequeue();
		resolvedQueue.Enqueue(currentActor);
		currentActor.SetIsCurrentActor(true);
		OnCurrentActorChanged?.Invoke();
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

		foreach (Character character in playerCharacters)
		{
			if (character != null)
			{
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
		_round++;
		CreateNewRoundQueues();
		unresolvedQueue = OrderQueue();

		CalculateOrderedBattlersList();
		battleManager.CalculateAndInitialiseOrderedBattlersInventory();
		_turn = 0;
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
		return battle.BattleWinConditions.IsPlayerVictory(this);
	}

    private bool IsEnemyVictory()
    {
        return battle.BattleWinConditions.IsEnemyVictory(this);
    }



    public void NextTurn()
	{

		if (IsPlayerVictory())
		{
			battleStateManager.ChangeState(ServiceLocator.Instance.BattleWonState);
			return;
		}
		else if (IsEnemyVictory())
		{
			battleStateManager.ChangeState(ServiceLocator.Instance.BattleLostState);
			return;
		}

		_turn++;
		Turn.AdvanceTurn(); // this sends off events that tick forward stat modifiers and effects

		unresolvedQueue = OrderQueue();

		if (_turn >= orderedBattlersList.Count)
		{
			NextRound();
		}

		_battleRear.MakeChecks(rearPlayerBattlersList); // this is the rear battle step. Todo: We should move this to a separate battlestate

		UpdateCurrentActor();
		UpdateState();
		
	}

	public void UpdateState()
	{
		if (currentActor.IsPlayer)
		{
			battleStateManager.ChangeState(ServiceLocator.Instance.PlayerTurnState);
		}
		else if (!currentActor.IsPlayer)
		{
			battleStateManager.ChangeState(ServiceLocator.Instance.EnemyTurnState);
		}
	}
}
