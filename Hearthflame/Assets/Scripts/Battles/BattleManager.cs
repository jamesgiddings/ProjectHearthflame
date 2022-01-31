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

	private static BattleManager _instance;

	private bool isLoaded;
	private int turn;
	private int round;
	private BattleState battleState;

	private BattleBehaviour[] battleBehaviours;
	private BattleBehaviour battleBehaviour;
	private GameObject turnOrderPrefab;

	private PlayerTurn playerTurn = new PlayerTurn();
	private EnemyTurn enemyTurn = new EnemyTurn();
	private BattleStart battleStart = new BattleStart();
	private BattleWon battleWon = new BattleWon();
	private BattleLost battleLost = new BattleLost();

	private List<Character> battlersList;
	private List<Character> orderedBattlersList;

	private CharacterInventory battlersListNew;
	private CharacterInventory orderedBattlersListNew;
	private CharacterInventory enemyBattlersList;
	private CharacterInventory playerBattlersList;

	private Character currentActor;

	public static BattleManager Instance => _instance;

	public CharacterInventory BattlersListNew => battlersListNew;

	public BattleManager(Battle battle, Party party)
	{
		this.battle = battle;
		this.party = party;
		FunctionUpdater.Create(() => Update());
		_instance = this;
		isLoaded = false;
		GameManager.Instance.BattleSceneLoaded += SetIsBattleFullyLoaded;
		ChangeState(battleStart);
	}

	private void SetIsBattleFullyLoaded(Scene scene)
	{
		if (scene.name == "BattleScene")
		{
			isLoaded = true;
		}
	}

	private void InitialiseUIElements()
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
		Debug.Log(battleBehaviour);
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
		Debug.Log("Player Attack");
		NextTurn();
	}

	private IEnumerator EnemyAction()
	{
		yield return new WaitForSeconds(2f);
		Debug.Log("Enemy Attack");
		NextTurn();
	}

	private void NextTurn()
	{
		turn++;
		if (turn >= battlersList.Count)
		{
			NextRound();
		}
		//Debug.Log("turn: " + turn);
		//Debug.Log("ordered battlers length: " + orderedBattlersList.Count);
		currentActor = CalculateNextActor();
		Debug.Log("updated turn: " + turn);
		Debug.Log(currentActor);
		Turn.AdvanceTurn(); // this sends off events that tick forward stat modifiers and effects
		UpdateState(currentActor);
	}

	private void UpdateState(Character currentActor)
	{
		if (HasPlayerTeamLost())
		{
			ChangeState(battleLost);
		}
		if (HasPlayerTeamWon())
		{
			ChangeState(battleWon);
		}
		if (currentActor.IsPlayer)
		{
			ChangeState(playerTurn);
		}
		else if (!currentActor.IsPlayer)
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

	private void NextRound()
	{
		round++;
		turn = 0;
	}

	private List<Character> CalculateOrderedBattlersList()
	{
		IEnumerable<Character> query = battlersList.OrderBy(character => character.StatSystem.GetStatValue(character.StatTypeStringRefDictionary["Speed"]) * -1); // * - 1 reverses the order of the list
		orderedBattlersList = new List<Character>();
		foreach (Character battler in query)
		{
			orderedBattlersList.Add(battler);
		}
		return orderedBattlersList;
	}

	private Character CalculateNextActor()
	{
		return orderedBattlersList[turn];
	}

	private void InitialiseBattle()
	{
		InitialiseBattlersList();
		CalculateOrderedBattlersList();

		turn = 0;
		round = 0;
		currentActor = CalculateNextActor();
		UpdateState(currentActor);
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
		Debug.Log("battlersList.Count: " + battlersList.Count);

		// NEW
		int totalNumberOfBattlers = party.PartyCharacters.Length + battle.EnemyParty.PartyCharacters.Length;
		Debug.Log("battleBehaviour == null: " + (battleBehaviour == null));
		battlersListNew = new CharacterInventory(battleBehaviour.OnCharactersUpdated, totalNumberOfBattlers);

		foreach (PartyCharacter partyCharacter in party.PartyCharacters)
		{
			if (partyCharacter.IsUnlocked) // && IsFront
			{
				battlersListNew.AddCharacter(new CharacterSlot(partyCharacter.Character.PartyCharacterTemplate));
				Debug.Log(partyCharacter.Character.Name);
			}
		}

		foreach (PartyCharacter partyCharacter in battle.EnemyParty.PartyCharacters)
		{
			battlersListNew.AddCharacter(new CharacterSlot(partyCharacter.Character.PartyCharacterTemplate));
			Debug.Log(partyCharacter.Character.Name);

			//if (partyCharacter.IsUnlocked) // && IsFront
			//{
			//	battlersListNew.AddCharacter(new CharacterSlot(partyCharacter.Character.PartyCharacterTemplate));
			//	Debug.Log(partyCharacter.Character.Name);
			//}
		}
	}

	IEnumerator WaitUntilBattleSceneFullyLoaded()
	{
		yield return new WaitUntil(() => isLoaded = true);
		InitialiseBattleBehaviour();
		BattleManager.Instance.InitialiseBattle();
		InitialiseUIElements();
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
		public abstract void HandleInput();

		public abstract void ExitState();

		public virtual void EnterState()
		{
			BattleManager.Instance.battleState = this;
		}
	}

	public class BattleStart : BattleState
	{
		public override void EnterState()
		{
			base.EnterState();

			GameManager.Instance.StartCoroutine(BattleManager.Instance.WaitUntilBattleSceneFullyLoaded());
			Debug.Log("PRPRPRPRPRPRPPR!");
			
		}

		public override void ExitState()
		{
			//throw new NotImplementedException();
		}

		public override void HandleInput()
		{
			//throw new NotImplementedException();
		}
	}

	public class PlayerTurn : BattleState
	{
		public override void EnterState()
		{
			base.EnterState();
			Debug.Log("We're in the player state");
		}

		public override void ExitState()
		{
			//throw new NotImplementedException();
		}

		public override void HandleInput()
		{
			if (Input.GetKeyDown(KeyCode.Space))
			{
				BattleManager.Instance.PlayerAction();
			}
		}
	}

	public class EnemyTurn : BattleState
	{
		public override void EnterState()
		{
			base.EnterState();
			Debug.Log("We're in the enemy state");
			GameManager.Instance.StartCoroutine(BattleManager.Instance.EnemyAction());
		}

		public override void ExitState()
		{
			//throw new NotImplementedException();
		}

		public override void HandleInput()
		{
			return;
		}
	}

	public class BattleWon : BattleState
	{
		public override void EnterState()
		{
			base.EnterState();
			// get the BattleReward class to show its popup 
			BattleManager.Instance.battle.BattleReward.AddBattleReward(BattleManager.Instance.party);
		}

		public override void ExitState()
		{
			//throw new NotImplementedException();
			// close the battle and change the scene back to map
			GameManager.Instance.BattleSceneLoaded -= BattleManager.Instance.SetIsBattleFullyLoaded;
		}

		public override void HandleInput()
		{
			//throw new NotImplementedException();
			// allow the player to accept the reward from the battle popup but do nothing else
		}
	}

	public class BattleLost : BattleState
	{
		public override void EnterState()
		{
			base.EnterState();
			// End the game
		}

		public override void ExitState()
		{
			GameManager.Instance.BattleSceneLoaded -= BattleManager.Instance.SetIsBattleFullyLoaded;
		}

		public override void HandleInput()
		{
			//throw new NotImplementedException();
		}
	}
	#endregion
}
