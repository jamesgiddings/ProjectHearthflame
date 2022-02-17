using GramophoneUtils.Stats;
using UnityEngine;
using CodeMonkey.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using UnityEngine.SceneManagement;

public class BattleManager
{
	[SerializeField] private Battle battle;
	[SerializeField] private Party party;

	private bool isLoaded;
	private int turn;
	private int round;
	private BattleState battleState;

	private BattleBehaviour[] battleBehaviours;
	private BattleBehaviour battleBehaviour;
	private GameObject turnOrderPrefab;

	private TargetManager targetManager;

	private PlayerTurn playerTurn;
	private EnemyTurn enemyTurn;
	private BattleStart battleStart;
	private BattleWon battleWon;
	private BattleLost battleLost;
	private BattleOver battleOver;

	private List<Character> battlersList;
	private List<Character> playerBattlersList;
	private List<Character> enemyBattlersList;
	private List<Character> orderedBattlersList;
	private Queue<Character> unresolvedQueue;
	private Queue<Character> resolvedQueue;

	private CharacterInventory battlersCharacterInventory;
	private CharacterInventory orderedBattlersCharacterInventory;
	private CharacterInventory enemyBattlersCharacterInventory;
	private CharacterInventory playerBattlersCharacterInventory;

	private Character currentActor;

	public Action OnCurrentActorChanged;
	public Action<BattleReward> OnBattleRewardsEarned;

	public Action<Character> OnSkillUsed;

	public CharacterInventory BattlersCharacterInventory => battlersCharacterInventory;
	public CharacterInventory OrderedBattlersCharacterInventory => orderedBattlersCharacterInventory;
	public CharacterInventory EnemyBattlersCharacterInventory => enemyBattlersCharacterInventory;
	public CharacterInventory PlayerBattlersCharacterInventory => playerBattlersCharacterInventory;

	public List<Character> BattlersList => battlersList;
	public List<Character> OrderedBattlersList => orderedBattlersList;

	public TargetManager TargetManager => targetManager; // getter

	public Character CurrentActor
	{
		get { return currentActor; } set { currentActor = value; }
	}

public BattleManager(Battle battle, Party party)
	{
		this.battle = battle;
		this.party = party;
		FunctionUpdater.Create(() => Update());
		isLoaded = false;
		GameManager.Instance.BattleSceneLoaded += SetIsBattleFullyLoaded;

		targetManager = new TargetManager(this);

		InitialiseBattleStates();

		ChangeState(battleStart);
	}

	private void Update()
	{
		battleState.HandleInput();
	}

	private void PlayerAction()
	{
		NextTurn();
		OnCurrentActorChanged?.Invoke();
	}

	private IEnumerator EnemyAction()
	{
		yield return new WaitForSeconds(1f);

		Skill enemySkill = CurrentActor.Brain.ChooseSkill();

		if (enemySkill != null)
		{
			TargetManager.GetCurrentlyTargeted(enemySkill, CurrentActor);
			OnSkillUsed?.Invoke(CurrentActor);
		}
		
		

		// choose skill
		// get target
		// use skill

		NextTurn();
		OnCurrentActorChanged?.Invoke();
	}

	public void GetTargets(Skill skill)
	{
		targetManager.GetCurrentlyTargeted(skill, currentActor);
	}

	private void ChangeTargets()
	{
		targetManager.ChangeTargeted(new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")));
	}

	private void UpdateState()
	{
		if (CurrentActor.IsPlayer)
		{
			ChangeState(playerTurn);
		}
		else if (!CurrentActor.IsPlayer)
		{
			ChangeState(enemyTurn);
		}
	}

	private void NextTurn()
	{
		if (IsPlayerVictory())
		{
			ChangeState(battleWon);
			return;
		}
		else if (IsEnemyVictory())
		{
			ChangeState(battleLost);
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

		Debug.Log("Current Turn: " + turn);
		Debug.Log("CurrentActor: " + CurrentActor.Name);

		OnCurrentActorChanged?.Invoke();
		UpdateState();
	}

	private bool IsPlayerVictory()
	{
		bool allDead = true;
		foreach (CharacterSlot characterSlot in enemyBattlersCharacterInventory.CharacterSlots)
		{
			if (!characterSlot.Character.HealthSystem.IsDead)
			{
				allDead = false;
			}
		}
		return allDead;
	}

	private bool IsEnemyVictory()
	{
		bool allDead = true;
		foreach (CharacterSlot characterSlot in playerBattlersCharacterInventory.CharacterSlots)
		{
			if (!characterSlot.Character.HealthSystem.IsDead)
			{
				allDead = false;
			}
		}
		return allDead;
	}

	private void NextRound()
	{
		round++;
		CreateNewRoundQueues();
		unresolvedQueue = OrderQueue();

		CalculateOrderedBattlersList();
		CalculateAndInitialiseOrderedBattlersInventory();
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
		CalculateAndInitialiseOrderedBattlersInventory();
		return orderedRoundQueue;
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

	#region Initialisation
	private void InitialiseBattle()
	{
		InitialiseBattlersLists();
		
		turn = 0;
		round = 0;

		CreateNewRoundQueues();
		unresolvedQueue = OrderQueue();

		InitialiseBattlersInventory();
		InitialisePlayerBattlersInventory();
		InitialiseEnemyBattlersInventory();
		CalculateOrderedBattlersList();
		CalculateAndInitialiseOrderedBattlersInventory();

		CurrentActor = unresolvedQueue.Dequeue();
		CurrentActor.SetIsCurrentActor(true);
		resolvedQueue.Enqueue(CurrentActor);
	}

	private void InitialiseBattleStates()
	{
		playerTurn = new PlayerTurn(this);
		enemyTurn = new EnemyTurn(this);
		battleStart = new BattleStart(this);
		battleWon = new BattleWon(this);
		battleLost = new BattleLost(this);
		battleOver = new BattleOver(this);
	}

	private void SetIsBattleFullyLoaded(Scene scene)
	{
		if (scene.name == "BattleScene")
		{
			isLoaded = true;
		}
	}

	private void InitialiseTurnOrderUI()
	{
		if (battleBehaviour != null)
		{
			turnOrderPrefab = battleBehaviour.TurnOrderPrefab;
			TurnOrderUI turnOrderUI = UnityEngine.Object.Instantiate(turnOrderPrefab, new Vector3(0, 0, 0), Quaternion.identity).GetComponent<TurnOrderUI>();
			turnOrderUI.Initialise(this);
		}
	}

	private void InitialiseBattleBehaviour()
	{
		battleBehaviours = GameObject.FindObjectsOfType<BattleBehaviour>();
		if (battleBehaviours.Length > 0)
		{
			battleBehaviour = battleBehaviours[0];
		}
	}

	private void InitialiseBattlersLists()
	{
		battlersList = new List<Character>();
		playerBattlersList = new List<Character>();
		enemyBattlersList = new List<Character>();

		foreach (PartyCharacter partyCharacter in party.PartyCharacters)
		{
			if (partyCharacter.IsUnlocked) // && IsFront
			{
				battlersList.Add(partyCharacter.Character);
				playerBattlersList.Add(partyCharacter.Character);
			}
		}
		
		Party enemyPartyClone = battle.EnemyParty.InstantiateClone();

		foreach (PartyCharacter partyCharacter in enemyPartyClone.PartyCharacters)
		{
			battlersList.Add(partyCharacter.Character);
			enemyBattlersList.Add(partyCharacter.Character);
		}
	}

	private CharacterInventory CalculateAndInitialiseOrderedBattlersInventory()
	{
		InitializeOrderedBattlersInventory();
		foreach (Character battler in orderedBattlersList)
		{
			orderedBattlersCharacterInventory.AddCharacter(new CharacterSlot(battler));
		}
		return orderedBattlersCharacterInventory;
	}

	private CharacterInventory InitializeOrderedBattlersInventory()
	{
		int totalNumberOfBattlers = party.PartyCharacters.Length + battle.EnemyParty.PartyCharacters.Length;
		orderedBattlersCharacterInventory = new CharacterInventory(battleBehaviour.OnCharactersUpdated, totalNumberOfBattlers);
		return orderedBattlersCharacterInventory;
	}

	private void InitialiseEnemyBattlersInventory()
	{
		int totalNumberOfEnemyBattlers = enemyBattlersList.Count;
		enemyBattlersCharacterInventory = new CharacterInventory(battleBehaviour.OnCharactersUpdated, totalNumberOfEnemyBattlers);

		foreach (Character character in enemyBattlersList)
		{
			enemyBattlersCharacterInventory.AddCharacter(new CharacterSlot(character));
			
		}
	}

	private void InitialisePlayerBattlersInventory()
	{
		int totalNumberOfPlayerBattlers = party.PartyCharacters.Length;
		playerBattlersCharacterInventory = new CharacterInventory(battleBehaviour.OnCharactersUpdated, totalNumberOfPlayerBattlers);

		foreach (PartyCharacter partyCharacter in party.PartyCharacters)
		{
			if (partyCharacter.IsUnlocked) // && IsFront
			{
				playerBattlersCharacterInventory.AddCharacter(new CharacterSlot(partyCharacter.Character));
			}
		}
	}

	private void InitialiseBattlersInventory()
	{
		int totalNumberOfBattlers = party.PartyCharacters.Length + battle.EnemyParty.PartyCharacters.Length;
		battlersCharacterInventory = new CharacterInventory(battleBehaviour.OnCharactersUpdated, totalNumberOfBattlers);

		foreach (PartyCharacter partyCharacter in party.PartyCharacters)
		{
			if (partyCharacter.IsUnlocked) // && IsFront
			{
				battlersCharacterInventory.AddCharacter(new CharacterSlot(partyCharacter.Character));
			}
		}

		foreach (Character character in enemyBattlersList)
		{
			battlersCharacterInventory.AddCharacter(new CharacterSlot(character));
		}
	}

	IEnumerator WaitUntilBattleSceneFullyLoadedThenInitialiseBattle()
	{
		yield return new WaitUntil(() => isLoaded = true);
		InitialiseBattleBehaviour();
		InitialiseBattle();
		InitialiseBattlerDisplay();
		InitialiseTurnOrderUI();
		InitialiseBattleRewardsDisplayUI();
		OnCurrentActorChanged?.Invoke();
		// Start:
		UpdateState();
	}

	private void InitialiseBattleRewardsDisplayUI()
	{
		battleBehaviour.BattleRewardsDisplayUI.Initialise(this);
	}

	#endregion

	#region UI

	private void InitialiseRadialMenu()
	{
		if (battleBehaviour != null)
		{
			battleBehaviour.RadialMenu.InitialiseRadialMenu(this);
		}
	}

	private void InitialiseBattlerDisplay()
	{
		if (battleBehaviour != null)
		{
			battleBehaviour.BattlerDisplayUI.Initialise(this);
		}
	}

	#endregion

	#region EndBattle

	public void EndBattle()
	{
		SceneController.UnloadScene("BattleScene");
	}

	#endregion

	#region BattleStates

	public void ChangeState(BattleState newBattleState)
	{
		if (battleState != null)
		{
			battleState.ExitState();
		}
		newBattleState.EnterState();
	}

	public abstract class BattleState
	{
		protected BattleManager _outer;

		public abstract void HandleInput();

		public virtual void EnterState()
		{
			_outer.battleState = this;
		}

		public virtual void ExitState()
		{
			_outer.TargetManager.ClearTargets();
		}
	}

	public class BattleStart : BattleState
	{
		public BattleStart(BattleManager outer)
		{
			_outer = outer;
		}
		public override void EnterState()
		{
			base.EnterState();

			GameManager.Instance.StartCoroutine(_outer.WaitUntilBattleSceneFullyLoadedThenInitialiseBattle());
		}

		public override void ExitState()
		{
			base.ExitState();
		}

		public override void HandleInput()
		{
			//throw new NotImplementedException();
		}
	}

	public class PlayerTurn : BattleState
	{
		private Character StateActor;

		public PlayerTurn(BattleManager outer)
		{
			_outer = outer;
		}

		public override void EnterState()
		{
			base.EnterState();
			StateActor = _outer.CurrentActor;
			Debug.Log("We're in the player state, and the current actor is: " + StateActor.Name);
			Debug.Log("StateActor.IsCurrentActor. Should be false: " + StateActor.IsCurrentActor);
			StateActor.SetIsCurrentActor(true);
			Debug.Log("We're in the player state, and the current actor is: " + StateActor.Name);
			Debug.Log("StateActor.IsCurrentActor.  Should be true: " + StateActor.IsCurrentActor);
			_outer.InitialiseRadialMenu();
			if (_outer.CurrentActor.HealthSystem.IsDead)
			{
				_outer.NextTurn();
			}
		}

		public override void ExitState()
		{
			base.ExitState();
			Debug.Log("We're leaving the player state, and the old actor is: " + StateActor.Name);
			Debug.Log("The old Actor state should still be true. Is it? :- " + StateActor.IsCurrentActor);
			StateActor.SetIsCurrentActor(false);			
			Debug.Log("The old Actor state should now be false. Is it? :- " + StateActor.IsCurrentActor);
		}

		public override void HandleInput()
		{
			if (Input.GetKeyDown(KeyCode.Space))
			{
				_outer.PlayerAction();
			}

			if (_outer.TargetManager.IsTargeting)
			{
				if (Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.DownArrow) || Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.RightArrow))
				{
					if (Math.Abs(Input.GetAxis("Vertical")) > 0 || (Math.Abs(Input.GetAxis("Horizontal")) > 0))
					{
						_outer.ChangeTargets();
					}
				}
			}
				
			if (Input.GetKeyUp(KeyCode.Return))
			{
				if (_outer.TargetManager.IsTargeting)
				{
					_outer.OnSkillUsed?.Invoke(StateActor);
					_outer.PlayerAction();
				}
			}
		}
	}

	public class EnemyTurn : BattleState
	{
		private Character StateActor;

		public EnemyTurn(BattleManager outer)
		{
			_outer = outer;
		}

		public override void EnterState()
		{
			base.EnterState();
			StateActor = _outer.CurrentActor;
			Debug.Log("We're in the enemy state, and the current actor is: " + StateActor.Name);
			Debug.Log("StateActor.IsCurrentActor. Should be false: " + StateActor.IsCurrentActor);
			StateActor.SetIsCurrentActor(true);
			Debug.Log("We're in the enemy state, and the current actor is: " + StateActor.Name);
			Debug.Log("StateActor.IsCurrentActor.  Should be true: " + StateActor.IsCurrentActor);
			if (_outer.CurrentActor.HealthSystem.IsDead)
			{
				_outer.NextTurn();
			}
			else
			{
				GameManager.Instance.StartCoroutine(_outer.EnemyAction());
			}
		}

		public override void ExitState()
		{
			base.ExitState();
			Debug.Log("We're leaving the enemy state, and the old actor is: " + StateActor.Name);
			Debug.Log("The old Actor state should still be true. Is it? :- " + StateActor.IsCurrentActor);
			StateActor.SetIsCurrentActor(false);
			Debug.Log("The old Actor state should now be false. Is it? :- " + StateActor.IsCurrentActor);
		}

		public override void HandleInput()
		{
			
		}
	}

	public class BattleWon : BattleState
	{
		public BattleWon(BattleManager outer)
		{
			_outer = outer;
		}

		public override void EnterState()
		{
			base.EnterState();
			// get the BattleReward class to show its popup 
			_outer.battle.BattleReward.AddBattleReward(_outer.party);
			_outer.OnBattleRewardsEarned?.Invoke(_outer.battle.BattleReward);
			//_outer.battleBehaviour.BattleRewardsDisplayUI.text = "You won some nice things";
			//_outer.battleBehaviour.BattleRewards.SetActive(true);

		}

		public override void ExitState()
		{
			base.ExitState();
			//throw new NotImplementedException();
			// close the battle and change the scene back to map
			GameManager.Instance.BattleSceneLoaded -= _outer.SetIsBattleFullyLoaded;
		}

		public override void HandleInput()
		{
			//throw new NotImplementedException();
			// allow the player to accept the reward from the battle popup but do nothing else
			if (Input.GetKeyDown(KeyCode.Space))
			{
				_outer.ChangeState(_outer.battleOver);
			}
		}
	}

	public class BattleLost : BattleState
	{
		public BattleLost(BattleManager outer)
		{
			_outer = outer;
		}

		public override void EnterState()
		{
			base.EnterState();
		}

		public override void ExitState()
		{
			base.ExitState();
			GameManager.Instance.BattleSceneLoaded -= _outer.SetIsBattleFullyLoaded;
		}

		public override void HandleInput()
		{
			//throw new NotImplementedException();
		}
	}

	public class BattleOver : BattleState
	{
		public BattleOver(BattleManager outer)
		{
			_outer = outer;
		}

		public override void EnterState()
		{
			base.EnterState();
			// End the game
			_outer.EndBattle();
			ExitState();
		}

		public override void ExitState()
		{
			base.ExitState();
			GameManager.Instance.BattleSceneLoaded -= _outer.SetIsBattleFullyLoaded;
		}

		public override void HandleInput()
		{
			//throw new NotImplementedException();
		}
	}

	#endregion
}
