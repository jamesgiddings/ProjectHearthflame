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
	private bool testBool = false;

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

	private List<Character> battlersList;
	private List<Character> orderedBattlersList;
	private Queue<Character> unresolvedQueue;
	private Queue<Character> resolvedQueue;

	private CharacterInventory battlersCharacterInventory;
	private CharacterInventory orderedBattlersCharacterInventory;
	private CharacterInventory enemyBattlersCharacterInventory;
	private CharacterInventory playerBattlersCharacterInventory;

	private Character currentActor;

	public Action OnCurrentActorChanged;

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

	private void InitialiseBattleStates()
	{
		playerTurn = new PlayerTurn(this);
		enemyTurn = new EnemyTurn(this);
		battleStart = new BattleStart(this);
		battleWon = new BattleWon(this);
		battleLost = new BattleLost(this);
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

	private void Update()
	{
		battleState.HandleInput();
	}

	private void PlayerAction()
	{
		Debug.Log("Player Attack"); /////// Just a test
		
		
		
		if (testBool == false)
		{
			Debug.Log(CurrentActor.Name + " Speed is: " + CurrentActor.StatSystem.GetStatValue(CurrentActor.StatTypeStringRefDictionary["Speed"]));
			StatModifier statModifier = new StatModifier(CurrentActor.StatTypeStringRefDictionary["Speed"], StatModifierTypes.Flat, 2000, 6);
			CurrentActor.StatSystem.AddModifier(statModifier);
			Debug.Log(CurrentActor.Name + " Speed is: " + CurrentActor.StatSystem.GetStatValue(CurrentActor.StatTypeStringRefDictionary["Speed"]));
			testBool = true;
		}
		if (CurrentActor.Name == "Snoo" && testBool == true)
			Debug.Log(CurrentActor.Name + " Speed should be 17 again, is it?: " + CurrentActor.StatSystem.GetStatValue(CurrentActor.StatTypeStringRefDictionary["Speed"]));
		NextTurn();
		OnCurrentActorChanged?.Invoke();
	}

	private IEnumerator EnemyAction()
	{
		yield return new WaitForSeconds(2f);
		Debug.Log("Enemy Attack");
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
		if (HasPlayerTeamLost())
		{
			ChangeState(battleLost);
		}
		if (HasPlayerTeamWon())
		{
			ChangeState(battleWon);
		}
		if (CurrentActor.IsPlayer)
		{
			ChangeState(playerTurn);
		}
		else if (!CurrentActor.IsPlayer)
		{
			ChangeState(enemyTurn);
		}
	}

	private bool HasPlayerTeamWon()
	{
		Debug.LogWarning("Not fully implemented.");
		return false;
	}

	private bool HasPlayerTeamLost()
	{
		Debug.LogWarning("Not fully implemented.");
		return false;
	}

	private void NextTurn()
	{
		turn++;
		Turn.AdvanceTurn(); // this sends off events that tick forward stat modifiers and effects

		unresolvedQueue = OrderQueue();

		if (turn >= battlersList.Count)
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

	private void NextRound()
	{
		round++;
		CreateNewRoundQueues();
		unresolvedQueue = OrderQueue();
		CalculateOrderedBattlersList();
		CalculateAndInitializeOrderedBattlersInventory();
		turn = 0;
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

	private void InitialiseBattle()
	{
		InitialiseBattlersList();
		
		turn = 0;
		round = 0;
		CreateNewRoundQueues();
		unresolvedQueue = OrderQueue();

		InitialiseBattlersInventory();
		InitialisePlayerBattlersInventory();
		InitialiseEnemyBattlersInventory();
		CalculateOrderedBattlersList();
		CalculateAndInitializeOrderedBattlersInventory();

		CurrentActor = unresolvedQueue.Dequeue();
		CurrentActor.SetIsCurrentActor(true);
		resolvedQueue.Enqueue(CurrentActor);
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
		CalculateAndInitializeOrderedBattlersInventory();
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

	private void InitialiseBattlersList()
	{
		battlersList = new List<Character>();

		foreach (PartyCharacter partyCharacter in party.PartyCharacters)
		{
			if (partyCharacter.IsUnlocked) // && IsFront
			{
				battlersList.Add(partyCharacter.Character);
			}
		}

		foreach (PartyCharacter partyCharacter in battle.EnemyParty.PartyCharacters)
		{
			battlersList.Add(partyCharacter.Character);
		}
	}

	private CharacterInventory CalculateAndInitializeOrderedBattlersInventory()
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
		int totalNumberOfEnemyBattlers = battle.EnemyParty.PartyCharacters.Length;
		enemyBattlersCharacterInventory = new CharacterInventory(battleBehaviour.OnCharactersUpdated, totalNumberOfEnemyBattlers);

		foreach (PartyCharacter partyCharacter in battle.EnemyParty.PartyCharacters)
		{
			enemyBattlersCharacterInventory.AddCharacter(new CharacterSlot(partyCharacter.Character));
			
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

		foreach (PartyCharacter partyCharacter in battle.EnemyParty.PartyCharacters)
		{
			battlersCharacterInventory.AddCharacter(new CharacterSlot(partyCharacter.Character));
		}
	}

	IEnumerator WaitUntilBattleSceneFullyLoadedThenInitialiseBattle()
	{
		yield return new WaitUntil(() => isLoaded = true);
		InitialiseBattleBehaviour();
		InitialiseBattle();
		InitialiseBattlerDisplay();
		InitialiseTurnOrderUI();
		OnCurrentActorChanged?.Invoke();
		// Start:
		UpdateState();
	}

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
			//_outer.OnCurrentActorChanged?.Invoke();
		}
		public virtual void ExitState()
		{
			//_outer.OnCurrentActorChanged?.Invoke();
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

			if (Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.DownArrow) || Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.RightArrow))
			{
				if (Math.Abs(Input.GetAxis("Vertical")) > 0 || (Math.Abs(Input.GetAxis("Horizontal")) > 0))
				{
					_outer.ChangeTargets();
				}
			}

			if (Input.GetKeyDown(KeyCode.Return))
			{
				_outer.OnSkillUsed?.Invoke(StateActor);
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
			GameManager.Instance.StartCoroutine(_outer.EnemyAction());

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
			// End the game
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
