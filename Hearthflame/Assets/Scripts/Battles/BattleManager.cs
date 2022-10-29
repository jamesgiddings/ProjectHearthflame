using GramophoneUtils.Stats;
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System;

public class BattleManager : MonoBehaviour
{
	private Battle battle;
	private PlayerModel playerModel;
		
	private BattleBehaviour[] battleBehaviours;
	private BattleBehaviour battleBehaviour;
	private GameObject turnOrderPrefab;

	private BattleStateManager battleStateManager;
	private BattleDataModel battleDataModel;
	private TargetManager targetManager;

	private CharacterInventory battlersCharacterInventory;
	private CharacterInventory orderedBattlersCharacterInventory;
	private CharacterInventory enemyBattlersCharacterInventory;
	private CharacterInventory playerBattlersCharacterInventory;

	private GameObject battleEnvironment;

	public CharacterInventory BattlersCharacterInventory => battlersCharacterInventory;
	public CharacterInventory OrderedBattlersCharacterInventory => orderedBattlersCharacterInventory;
	public CharacterInventory EnemyBattlersCharacterInventory => enemyBattlersCharacterInventory;
	public CharacterInventory PlayerBattlersCharacterInventory => playerBattlersCharacterInventory;

	public GameObject BattleEnvironment => battleEnvironment;

	private TurnOrderUI turnOrderUI;

	public BattleStateManager BattleStateManager => battleStateManager; // getter
	public TargetManager TargetManager => targetManager; // getter
	public BattleDataModel BattleDataModel => battleDataModel; // getter

	public PlayerModel PlayerModel => playerModel;

	public BattleBehaviour BattleBehaviour => battleBehaviour;

	public Battle Battle => battle;

	public void Initialise(Battle battle, PlayerModel player)
	{
		this.battle = battle;
		this.playerModel = player;

		battleStateManager = new BattleStateManager(this);
		battleDataModel = new BattleDataModel(this, battle, battleBehaviour);
		targetManager = new TargetManager(this);

		battleStateManager.ChangeState(battleStateManager.BattleStart);
	}

	public void InitialiseBattle()
	{
		InitialiseBattleBehaviour();
		InitialiseBattleEnvironment();
		battleDataModel.InitialiseBattleModel();
		InitialiseBattlerDisplay();
		InitialiseTurnOrderUI();
		InitialiseBattleRewardsDisplayUI();
		battleDataModel.OnCurrentActorChanged?.Invoke();
		// Start:
		battleDataModel.UpdateState();
	}

    private void InitialiseBattleEnvironment()
    {
		battleEnvironment = Instantiate(battle.BattleBackground, battleBehaviour.BattleTilemapGrid.transform);
    }

    private void Update()
	{
		battleStateManager.HandleInput();
	}

	public void GetTargets(Skill skill)
	{
		targetManager.GetCurrentlyTargeted(skill, BattleDataModel.CurrentActor);
	}

	public void ChangeTargets()
	{
		targetManager.ChangeTargeted(new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")));
	}

	#region UI

	private void InitialiseBattleRewardsDisplayUI()
	{
		battleBehaviour.BattleRewardsDisplayUI.Initialise(this);
	}

	public void InitialiseBattlersInventory()
	{
		int totalNumberOfBattlers = BattleDataModel.PlayerCharacters.Count + battle.BattleCharacters.Count;
		battlersCharacterInventory = new CharacterInventory(battleBehaviour.OnCharactersUpdated, totalNumberOfBattlers);

		foreach (Character character in BattleDataModel.PlayerCharacters)
		{
			if (character != null)
			{
				if (character.IsUnlocked) // && IsFront
				{
					battlersCharacterInventory.AddCharacter(new CharacterSlot(character));
				}
			}
		}

		foreach (Character character in BattleDataModel.EnemyBattlersList)
		{
			battlersCharacterInventory.AddCharacter(new CharacterSlot(character));
		}
	}

	public CharacterInventory CalculateAndInitialiseOrderedBattlersInventory()
	{
		InitializeOrderedBattlersInventory();
		foreach (Character battler in BattleDataModel.OrderedBattlersList)
		{
			orderedBattlersCharacterInventory.AddCharacter(new CharacterSlot(battler));
		}
		return orderedBattlersCharacterInventory;
	}

	private CharacterInventory InitializeOrderedBattlersInventory()
	{
		int totalNumberOfBattlers = BattleDataModel.PlayerCharacters.Count + battle.BattleCharacters.Count;
		orderedBattlersCharacterInventory = new CharacterInventory(battleBehaviour.OnCharactersUpdated, totalNumberOfBattlers);
		return orderedBattlersCharacterInventory;
	}

	public void InitialiseEnemyBattlersInventory()
	{
		int totalNumberOfEnemyBattlers = BattleDataModel.EnemyCharacters.Count;
		enemyBattlersCharacterInventory = new CharacterInventory(battleBehaviour.OnCharactersUpdated, totalNumberOfEnemyBattlers);

		foreach (Character character in BattleDataModel.EnemyCharacters)
		{
			enemyBattlersCharacterInventory.AddCharacter(new CharacterSlot(character));

		}
	}

	public void InitialisePlayerBattlersInventory()
	{
		int totalNumberOfPlayerBattlers = BattleDataModel.PlayerCharacters.Count;
		playerBattlersCharacterInventory = new CharacterInventory(battleBehaviour.OnCharactersUpdated, totalNumberOfPlayerBattlers);

		foreach (Character character in BattleDataModel.PlayerCharacters)
		{
			if (character != null)
			{
				if (character.IsUnlocked) // && IsFront
				{
					playerBattlersCharacterInventory.AddCharacter(new CharacterSlot(character));
				}
			}
		}
	}

	private void InitialiseTurnOrderUI()
	{
		if (battleBehaviour != null)
		{
			turnOrderPrefab = battleBehaviour.TurnOrderPrefab;
			turnOrderUI = UnityEngine.Object.Instantiate(turnOrderPrefab, new Vector3(0, 0, 0), Quaternion.identity).GetComponent<TurnOrderUI>();
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

	public void InitialiseRadialMenu()
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
		GameManager.Destroy(turnOrderUI.gameObject);

		SceneController.UnloadScene("Battle Scene");
	}

	#endregion

}
