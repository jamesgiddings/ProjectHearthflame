using GramophoneUtils.Stats;
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System;
using Cinemachine;
using GramophoneUtils.Characters;
using System.Linq;

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
        battleDataModel = ServiceLocator.Instance.BattleDataModel;
        targetManager = ServiceLocator.Instance.TargetManager;
        ServiceLocator.Instance.CharacterModel.AddEnemyCharacters(battle.BattleCharacters);
        battleDataModel.InitialiseBattleModel();
		//InitialiseBattlerDisplay();

		

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

	

	private void InitialiseTurnOrderUI()
	{
		if (battleBehaviour != null)
		{
			turnOrderPrefab = battleBehaviour.TurnOrderPrefab;
			turnOrderUI = UnityEngine.Object.Instantiate(turnOrderPrefab, new Vector3(0, 0, 0), Quaternion.identity).GetComponent<TurnOrderUI>();
			turnOrderUI.Initialise();
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


    #endregion

    #region EndBattle

    public void EndBattle()
	{
        Destroy(turnOrderUI.gameObject);
        battleBehaviour.RearBattlePrefab.SetActive(false);
        ServiceLocator.Instance.GameStateManager.ChangeState(ServiceLocator.Instance.ExplorationState);
		//UninitialiseBattlers();
	}

	private void UninitialiseBattlers()
	{
		Battler[] battlers = ServiceLocator.Instance.CharacterGameObjectManager.CharacterBattlerDictionary.Values.ToArray();

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
