using GramophoneUtils.Stats;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

public class BattlerDisplayUI : MonoBehaviour
{
	[SerializeField] private Transform battlerPrefab;
	
	private BattleManager battleManager;

	private CharacterInventory battlersListNew;
	private CharacterInventory orderedBattlersListNew;
	private CharacterInventory enemyBattlersList;
	private CharacterInventory playerBattlersList;

	private Battler[] playerBattlers;
	private Battler[] enemyBattlers;

	private Battler[] battlerGameObjects;

	private Dictionary<Character, Battler> characterBattlerDictionary;

	public CharacterInventory PlayerBattlersList { get { return playerBattlersList; } set { playerBattlersList = value; } }
	public CharacterInventory EnemyBattlersList { get { return enemyBattlersList; } set { enemyBattlersList = value; } }
	public CharacterInventory OrderedBattlersListNew { get { return orderedBattlersListNew; } set { orderedBattlersListNew = value; } }
	public CharacterInventory BattlersListNew { get { return battlersListNew; } set { battlersListNew = value; } }

    public Battler[] BattlerGameObjects => battlerGameObjects;

    public Dictionary<Character, Battler> CharacterBattlerDictionary => characterBattlerDictionary;

	#region Initialising
	public void Initialise(BattleManager battleManager)
	{
		this.battleManager = battleManager;
		
		this.battlersListNew = battleManager.BattlersCharacterInventory;
		this.orderedBattlersListNew = battleManager.OrderedBattlersCharacterInventory;
		this.enemyBattlersList = battleManager.EnemyBattlersCharacterInventory;
		this.playerBattlersList = battleManager.PlayerBattlersCharacterInventory;

		battlerGameObjects = new Battler[battlersListNew.CharacterSlots.Length];
		characterBattlerDictionary = new Dictionary<Character, Battler>();

        playerBattlers = new Battler[playerBattlersList.CharacterSlots.Length];
        enemyBattlers = new Battler[enemyBattlersList.CharacterSlots.Length];
		playerBattlers = InitialisePlayerBattlers(playerBattlersList);
        enemyBattlers = InitialiseEnemyBattlers(enemyBattlersList);

        battlerGameObjects = playerBattlers.Concat(enemyBattlers).ToArray();
	}

    private Battler[] InitialisePlayerBattlers(CharacterInventory characterInventory)
    {
        Battler[] playerBattlers = new Battler[playerBattlersList.CharacterSlots.Length];
        for (int i = 0; i < playerBattlers.Length; i++)
        {
            CharacterSlot characterSlot = characterInventory.CharacterSlots[i];
            Debug.LogWarning("Broken. Needs to react to unlocked and rear status");
            //Debug.Log(!characterSlot.Character.PartyCharacterTemplate.PartyCharacter.IsRear);
            if (characterSlot.Character != null)
            {
                if (true) //characterSlot.CharacterTemplate.IsUnlocked) //&& !characterSlot.Character.PartyCharacterTemplate.PartyCharacter.IsRear)
                {
					Debug.Log("NUMBER OF PLAYER BATTLERS: " + ServiceLocator.Instance.PlayerBehaviour.PlayerBattlers.Count);
					playerBattlers[i] = ServiceLocator.Instance.PlayerBehaviour.PlayerBattlers[characterSlot.Character];
                    playerBattlers[i].Initialise(battleManager, characterSlot.Character);
                    characterBattlerDictionary.Add(characterSlot.Character, playerBattlers[i]);
                }
            }
        }
		return playerBattlers;
    }

    private Battler[] InitialiseEnemyBattlers(CharacterInventory characterInventory)
	{

        enemyBattlers = new Battler[enemyBattlersList.CharacterSlots.Length];
        for (int i = 0; i < characterInventory.CharacterSlots.Length; i++)
		{
			CharacterSlot characterSlot = characterInventory.CharacterSlots[i];
			if (characterSlot.Character != null)
			{
				Debug.Log("AN ENEMY: ------------------------------> " + characterSlot.Character.Name);
                enemyBattlers[i] = InitialiseEnemyBattler(characterSlot, i);
				characterBattlerDictionary.Add(characterSlot.Character, enemyBattlers[i]);
				Debug.Log(characterBattlerDictionary[characterSlot.Character]);
			}
		}
		Debug.Log("The finished CHARACTER BATTLER DICTIONARY size: " + characterBattlerDictionary.Count);
		BattlerDisplayUI[] displayUIs = FindObjectsOfType<BattlerDisplayUI>();
		Debug.Log("DISPLAYUIS.Length: " + displayUIs.Length);
        return enemyBattlers;
    }

	public void OnEnterBattle()
	{

	}



	private Battler InitialiseEnemyBattler(CharacterSlot characterSlot, int index)
	{

        Battler battler = Instantiate(battlerPrefab, ServiceLocator.Instance.PlayerBehaviour.transform.position + new Vector3(2 + (2 * index), 0, 0), Quaternion.identity).GetComponent<Battler>();
        
		battler.Initialise(battleManager, characterSlot.Character);

		return battler;
	}

    #endregion


    #region EndOfLife

    public void OnExitBattle()
    {
		for (int i = 0; i < battlerGameObjects.Length; i++)
		{
			battlerGameObjects[i].DisplayBattleUI(false);
		}

        for (int i = 0; i < enemyBattlers.Length; i++)
        {
            Destroy(enemyBattlers[i].gameObject);
        }
    }
    #endregion
}
