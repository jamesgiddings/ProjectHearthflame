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
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Battle Data Model", menuName = "Battles/Systems/Battle Data Model")]
public class BattleDataModel : ScriptableObjectThatCanRunCoroutines
{
    #region Attributes/Properties

    private StateManager _battleStateManager;

	private Battle _battle;

	private BattleRear _battleRear;
    public BattleRear BattleRear => _battleRear;

    private ICharacterModel _characterModel;

	private List<ICharacter> _orderedBattlersList;
    public List<ICharacter> OrderedBattlersList => _orderedBattlersList;

	private Queue<ICharacter> _unresolvedQueue = new Queue<ICharacter>();

	private Queue<ICharacter> _resolvedQueue = new Queue<ICharacter>();

    private ICharacter _currentActor;

    [ShowInInspector]
    public ICharacter CurrentActor
    {
        get { return _currentActor; }
        set { _currentActor = value; }
    }

    public Action OnCurrentActorChanged;

	public Action<BattleReward> OnBattleRewardsEarned;

	[SerializeField] public VoidEvent OnBattlerCollectionsUpdated;

    public Action<ICharacter> OnSkillUsed;

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
        OnBattlerCollectionsUpdated?.Raise();
        OnCurrentActorChanged?.Invoke();
    }

    [Button]
    public void UpdateUnresolvedQueue()
    {
        _unresolvedQueue = OrderQueue();
        OnBattlerCollectionsUpdated?.Raise();
        OnCurrentActorChanged?.Invoke();
    }

    [Button]
    public bool RespondToBattleLossOrVictory()
	{
        if (IsPlayerVictory())
        {
            _battleStateManager.ChangeState(ServiceLocator.Instance.BattleWonState);
            return true;
        }
        else if (IsEnemyVictory())
        {
            _battleStateManager.ChangeState(ServiceLocator.Instance.BattleLostState);
            return true;
        }
        return false;
    }

	public async Task UpdateState()
    {
        await Task.Delay(100);
        if (_currentActor.IsPlayer)
        {
            _battleStateManager.ChangeState(ServiceLocator.Instance.PlayerTurnState);
        }
        else if (!_currentActor.IsPlayer)
        {
            _battleStateManager.ChangeState(ServiceLocator.Instance.EnemyTurnState);
        }
    }

    public void UpdateStateWithSmallDelay()
    {
        StartCoroutine(DelayForSecondsThenUpdateState(0.1f));
    }

    [Button]
    public async Task UpdateCurrentActor()
	{
		if (_currentActor != null)
		{
			_currentActor.SetIsCurrentActor(false);
		}

        _currentActor = _unresolvedQueue.Dequeue();
        _resolvedQueue.Enqueue(_currentActor);
        _currentActor.SetIsCurrentActor(true);
        OnCurrentActorChanged?.Invoke();
        await Task.Delay(10);
	}

    public bool IsEndOFRound()
    {
        return (_turn >= _orderedBattlersList.Count);
    }

    [Button]
    public void AdvanceTurn()
    {
        _turn++;
        _currentActor.AdvanceCharacterTurn(); // this sends off events that tick forward stat modifiers and effects
        //Turn.AdvanceTurn(); // this sends off events that tick forward stat modifiers and effects
    }

    public void UpdateState(float delay)
    {
		StartCoroutine(DelayForSecondsThenUpdateState(delay));
    }

    [Button]
    public void CreateNewRoundQueues()
    {
        _unresolvedQueue = new Queue<ICharacter>();
        foreach (ICharacter character in _characterModel.AllCharacters)
        {
            _unresolvedQueue.Enqueue(character);
        }
        _resolvedQueue = new Queue<ICharacter>();
        OnBattlerCollectionsUpdated?.Raise();
    }

    [Button]
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

	private Queue<ICharacter> OrderQueue()
	{

        //Todo, this gets ordered three times at the start of the battle, find out why
		List<ICharacter> frontAndEnemyCharacters = new List<ICharacter>();
		frontAndEnemyCharacters.AddRange(_characterModel.PlayerCharacters);
		frontAndEnemyCharacters.AddRange(_characterModel.EnemyCharacters);
		IEnumerable<ICharacter> unresolvedQuery = from unresolved in frontAndEnemyCharacters.Except(_resolvedQueue) select unresolved;
        _unresolvedQueue.Clear();

        unresolvedQuery = from chara in unresolvedQuery orderby chara.StatSystem.GetStatValue(chara.StatTypeStringRefDictionary["Speed"]) * -1 select chara;

        foreach (ICharacter battler in unresolvedQuery)
		{
			_unresolvedQueue.Enqueue(battler);
		}

		Queue<ICharacter> orderedRoundQueue = new Queue<ICharacter>();
		foreach (ICharacter battler in unresolvedQuery)
		{
			orderedRoundQueue.Enqueue(battler);
		}
		CalculateOrderedBattlersList();
		return orderedRoundQueue;
	}

	private List<ICharacter> CalculateOrderedBattlersList()
	{
		_orderedBattlersList = new List<ICharacter>();
		foreach (ICharacter battler in _resolvedQueue)
		{
			_orderedBattlersList.Add(battler);
		}
		foreach (ICharacter battler in _unresolvedQueue)
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
