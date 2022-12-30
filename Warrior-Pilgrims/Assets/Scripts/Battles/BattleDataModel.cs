using GramophoneUtils.Battles;
using GramophoneUtils.Characters;
using GramophoneUtils.Events.CustomEvents;
using GramophoneUtils.Events.UnityEvents;
using GramophoneUtils.Stats;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BattleDataModel : MonoBehaviour
{
    #region Attributes/Properties

    private StateManager battleStateManager;
	private Battle _battle;
	private BattleRear _battleRear;
	private CharacterModel _characterModel;

	private List<Character> _orderedBattlersList;
	private Queue<Character> unresolvedQueue = new Queue<Character>();
	private Queue<Character> resolvedQueue = new Queue<Character>();

    private Character currentActor;

	public Action OnCurrentActorChanged;
	public Action<BattleReward> OnBattleRewardsEarned;

	[SerializeField] public VoidEvent OnBattlerCollectionsUpdated;

    public Action<Character> OnSkillUsed;

    private int _turn;
    private int _round;

	public List<Character> OrderedBattlersList => _orderedBattlersList;
    
    public BattleRear BattleRear => _battleRear;

    [ShowInInspector] public int CurrentTurn => _turn;
    [ShowInInspector] public int CurrentRound => _round;

	public Character CurrentActor
	{
		get { return currentActor; }
		set { currentActor = value; }
	}

    #endregion

    #region API

    public void InitialiseBattleModel()
	{
		OnCurrentActorChanged = null; // TODO, hack, because there was a memory leak from battler
		OnSkillUsed = null; // TODO, hack as above

		battleStateManager = ServiceLocator.Instance.BattleStateManager;
        _characterModel =  ServiceLocator.Instance.CharacterModel;
        _battle = ServiceLocator.Instance.BattleManager.Battle;
		_battleRear = ServiceLocator.Instance.BattleManager.Battle.BattleRear;

		_turn = 0;
		_round = 0;

		CreateNewRoundQueues();
		unresolvedQueue = OrderQueue();
		CalculateOrderedBattlersList();

		UpdateCurrentActor();
	}

    public void NextRound()
    {
        _round++;
        CreateNewRoundQueues();
        unresolvedQueue = OrderQueue();

        CalculateOrderedBattlersList();
        _turn = 0;
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

        unresolvedQueue = OrderQueue();
        OnBattlerCollectionsUpdated?.Raise();

        _turn++;
        Turn.AdvanceTurn(); // this sends off events that tick forward stat modifiers and effects

        if (_turn >= _orderedBattlersList.Count)
        {
            NextRound();
        }

        _battleRear.MakeChecks(_characterModel.RearPlayerCharactersList); // this is the rear battle step. Todo: We should move this to a separate battlestate

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

    #endregion

    #region Utilities

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
		foreach (Character character in _characterModel.AllFrontCharactersList)
		{
			unresolvedQueue.Enqueue(character);
		}
		resolvedQueue = new Queue<Character>();
	}



	private Queue<Character> OrderQueue()
	{
		List<Character> frontAndEnemyCharacters = new List<Character>();
		frontAndEnemyCharacters.AddRange(_characterModel.FrontPlayerCharactersList);
		frontAndEnemyCharacters.AddRange(_characterModel.FrontEnemyCharactersList);
		IEnumerable<Character> unresolvedQuery = from unresolved in frontAndEnemyCharacters.Except(resolvedQueue) select unresolved;

		unresolvedQueue.Clear();
		foreach (Character battler in unresolvedQuery)
		{
			unresolvedQueue.Enqueue(battler);
		}

        IEnumerable<Character> ordereredUnresolvedQuery = unresolvedQueue.OrderBy(character => character.StatSystem.GetStatValue(character.StatTypeStringRefDictionary["Speed"]) * -1); // * - 1 reverses the order of the list
		Queue<Character> orderedRoundQueue = new Queue<Character>();
		foreach (Character battler in ordereredUnresolvedQuery)
		{
			orderedRoundQueue.Enqueue(battler);
		}
		CalculateOrderedBattlersList();
		return orderedRoundQueue;
	}

	private List<Character> CalculateOrderedBattlersList()
	{
		_orderedBattlersList = new List<Character>();
		foreach (Character battler in resolvedQueue)
		{
			_orderedBattlersList.Add(battler);
		}
		foreach (Character battler in unresolvedQueue)
		{
			_orderedBattlersList.Add(battler);
		}
		return _orderedBattlersList;
	}

	private bool IsPlayerVictory()
	{
		return _battle.BattleWinConditions.IsPlayerVictory(this);
	}

    private bool IsEnemyVictory()
    {
        return _battle.BattleWinConditions.IsEnemyVictory(this);
    }

    #endregion
}
