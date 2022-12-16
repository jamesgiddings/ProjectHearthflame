using GramophoneUtils.Stats;
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System;
using Cinemachine;

public class BattleManager : MonoBehaviour
{
	private Battle battle;
	
	[SerializeField] private BattleBehaviour battleBehaviour;
	private GameObject turnOrderPrefab;

	private BattleDataModel battleDataModel;
	private TargetManager targetManager;

	private CharacterInventory battlersCharacterInventory;
	private CharacterInventory orderedBattlersCharacterInventory;
	private CharacterInventory enemyBattlersCharacterInventory;
	private CharacterInventory playerBattlersCharacterInventory;

	private CinemachineVirtualCamera cinemachineVirtualCamera;

	public CharacterInventory BattlersCharacterInventory => battlersCharacterInventory;
	public CharacterInventory OrderedBattlersCharacterInventory => orderedBattlersCharacterInventory;
	public CharacterInventory EnemyBattlersCharacterInventory => enemyBattlersCharacterInventory;
	public CharacterInventory PlayerBattlersCharacterInventory => playerBattlersCharacterInventory;

	private TurnOrderUI turnOrderUI;
	public TargetManager TargetManager => targetManager; // getter
	public BattleDataModel BattleDataModel => battleDataModel; // getter

	public BattleBehaviour BattleBehaviour => battleBehaviour;

	public Battle Battle => battle;

	private void OnEnable()
	{
        battle.InstanceCharacters();
        battleDataModel = ServiceLocator.Instance.BattleDataModel;
        targetManager = ServiceLocator.Instance.TargetManager;
        battleDataModel.InitialiseBattleModel();
        InitialiseBattlerDisplay();
        InitialiseTurnOrderUI();
		InitialiseRearBattleUI();
        InitialiseBattleRewardsDisplayUI();
        battleDataModel.OnCurrentActorChanged?.Invoke();

        // Move to next state:
        battleDataModel.Invoke("UpdateState", 0.5f);
    }

        public void SetBattle(Battle battle)
    {
		this.battle = battle;
    }

	public void GetTargets(Skill skill)
	{
		targetManager.GetCurrentlyTargeted(skill, BattleDataModel.CurrentActor);
        Debug.Log("GetCurrentlyTargeted(skill, BattleDataModel.CurrentActor).Count: " + targetManager.GetCurrentlyTargeted(skill, BattleDataModel.CurrentActor).Count);
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
		Debug.Log("totalNumberOfEnemyBattlers: " + totalNumberOfEnemyBattlers);
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

	private void InitialiseRearBattleUI()
	{
		battleBehaviour.RearBattlePrefab.SetActive(true);
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
        ServiceLocator.Instance.BattlerDisplayUI.Initialise(this);
	}

    #endregion

    #region EndBattle

    public void EndBattle()
	{
        Destroy(turnOrderUI.gameObject);
        battleBehaviour.RearBattlePrefab.SetActive(false);
        ServiceLocator.Instance.GameStateManager.ChangeState(ServiceLocator.Instance.ExplorationState);
		UninitialiseBattlers();
	}

	private void UninitialiseBattlers()
	{
		Battler[] battlers = ServiceLocator.Instance.BattlerDisplayUI.BattlerGameObjects;

        foreach (Battler battler in battlers)
		{
			if (battler != null)
			{
                battler.Uninitialise();
            }
		}
	}

	#endregion

}
