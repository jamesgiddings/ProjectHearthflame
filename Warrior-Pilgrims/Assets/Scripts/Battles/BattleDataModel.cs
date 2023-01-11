using GramophoneUtils.Battles;
using GramophoneUtils.Characters;
using GramophoneUtils.Events.CustomEvents;
using GramophoneUtils.Stats;
using GramophoneUtils.Utilities;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "Battle Data Model", menuName = "Battles/Systems/Battle Data Model")]
public class BattleDataModel : ScriptableObjectThatCanRunCoroutines
{
    #region Attributes/Properties

    private StateManager _battleStateManager;

	private Battle _battle;

	private BattleRear _battleRear;
    public BattleRear BattleRear => _battleRear;

    private CharacterModel _characterModel;

	private List<Character> _orderedBattlersList;
    public List<Character> OrderedBattlersList => _orderedBattlersList;

	private Queue<Character> _unresolvedQueue = new Queue<Character>();

	private Queue<Character> _resolvedQueue = new Queue<Character>();

    private Character _currentActor;
    public Character CurrentActor
    {
        get { return _currentActor; }
        set { _currentActor = value; }
    }

    public Action OnCurrentActorChanged;

	public Action<BattleReward> OnBattleRewardsEarned;

	[SerializeField] public VoidEvent OnBattlerCollectionsUpdated;

    public Action<Character> OnSkillUsed;

    private int _turn;
    [ShowInInspector] public int CurrentTurn => _turn;

    private int _round;
    [ShowInInspector] public int CurrentRound => _round;

    #endregion

    #region Public Functions

    public void InitialiseBattleModel()
	{
		OnCurrentActorChanged = null; // TODO, hack, because there was a memory leak from battler
		OnSkillUsed = null; // TODO, hack as above

		_battleStateManager = ServiceLocator.Instance.BattleStateManager;
        _characterModel =  ServiceLocator.Instance.CharacterModel;
        _battle = ServiceLocator.Instance.BattleManager.Battle;
		_battleRear = ServiceLocator.Instance.BattleManager.Battle.BattleRear;

		_turn = 0;
		_round = -1;

		CreateNewRoundQueues();
		_unresolvedQueue = OrderQueue();
		CalculateOrderedBattlersList();

		/*UpdateCurrentActor();*/
	}

    public void NextRound()
    {
        _round++;
        CreateNewRoundQueues();
        _unresolvedQueue = OrderQueue();

        CalculateOrderedBattlersList();
        _turn = 0;
    }

    public void NextTurn()
    {
/*        if (IsPlayerVictory())
        {
            _battleStateManager.ChangeState(ServiceLocator.Instance.BattleWonState);
            return;
        }
        else if (IsEnemyVictory())
        {
            _battleStateManager.ChangeState(ServiceLocator.Instance.BattleLostState);
            return;
        }

        UpdateUnresolvedQueue();

        AdvanceTurn();

        UpdateCurrentActor();
        UpdateState();*/
    }

    public void UpdateUnresolvedQueue()
    {
        _unresolvedQueue = OrderQueue();
        OnBattlerCollectionsUpdated?.Raise();
        OnCurrentActorChanged?.Invoke();
    }

    public void RespondToBattleLossOrVictory()
	{
        if (IsPlayerVictory())
        {
            _battleStateManager.ChangeState(ServiceLocator.Instance.BattleWonState);
            return;
        }
        else if (IsEnemyVictory())
        {
            _battleStateManager.ChangeState(ServiceLocator.Instance.BattleLostState);
            return;
        }
    }

	public void UpdateState()
    {
        if (_currentActor.IsPlayer)
        {
            _battleStateManager.ChangeState(ServiceLocator.Instance.PlayerTurnState);
        }
        else if (!_currentActor.IsPlayer)
        {
            _battleStateManager.ChangeState(ServiceLocator.Instance.EnemyTurnState);
        }
    }

    public void UpdateCurrentActor()
	{
		if (_currentActor != null)
		{
			_currentActor.SetIsCurrentActor(false);
		}

        _currentActor = _unresolvedQueue.Dequeue();
        _resolvedQueue.Enqueue(_currentActor);
        _currentActor.SetIsCurrentActor(true);
        OnCurrentActorChanged?.Invoke();
	}

    public bool IsEndOFRound()
    {
        return (_turn >= _orderedBattlersList.Count);
    }

    public void AdvanceTurn()
    {
        _turn++;
        Turn.AdvanceTurn(); // this sends off events that tick forward stat modifiers and effects
    }

    public void UpdateState(float delay)
    {
		StartCoroutine(DelayForSecondsThenUpdateState(delay));
    }

    public void CreateNewRoundQueues()
    {
        _unresolvedQueue = new Queue<Character>();
        foreach (Character character in _characterModel.AllCharacters)
        {
            _unresolvedQueue.Enqueue(character);
        }
        _resolvedQueue = new Queue<Character>();
        OnBattlerCollectionsUpdated?.Raise();
    }

    public void RaiseActionsAtEndOfState()
    {
        OnBattlerCollectionsUpdated?.Raise();
        OnCurrentActorChanged?.Invoke();
    }

    #endregion

    #region Private Functions
    private IEnumerator DelayForSecondsThenUpdateState(float delay)
	{
		yield return new WaitForSeconds(delay);
		UpdateState();
    }

	private Queue<Character> OrderQueue()
	{
		List<Character> frontAndEnemyCharacters = new List<Character>();
		frontAndEnemyCharacters.AddRange(_characterModel.PlayerCharacters);
		frontAndEnemyCharacters.AddRange(_characterModel.EnemyCharacters);
		IEnumerable<Character> unresolvedQuery = from unresolved in frontAndEnemyCharacters.Except(_resolvedQueue) select unresolved;

		_unresolvedQueue.Clear();
		foreach (Character battler in unresolvedQuery)
		{
			_unresolvedQueue.Enqueue(battler);
		}

        IEnumerable<Character> ordereredUnresolvedQuery = _unresolvedQueue.OrderBy(character => character.StatSystem.GetStatValue(character.StatTypeStringRefDictionary["Speed"]) * -1); // * - 1 reverses the order of the list
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
		foreach (Character battler in _resolvedQueue)
		{
			_orderedBattlersList.Add(battler);
		}
		foreach (Character battler in _unresolvedQueue)
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
