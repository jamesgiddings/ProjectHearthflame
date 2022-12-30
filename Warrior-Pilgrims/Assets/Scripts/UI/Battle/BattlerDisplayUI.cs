using GramophoneUtils.Stats;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using GramophoneUtils.Characters;
using UnityEngine.TextCore.Text;
using Character = GramophoneUtils.Characters.Character;

public class BattlerDisplayUI : MonoBehaviour
{
	[SerializeField] private Transform battlerPrefab;
	
	private BattleManager battleManager;
	private BattleDataModel _battleDataModel;
	private CharacterModel _characterModel;

	private Battler[] playerBattlers;
	private Battler[] enemyBattlers;

	private Battler[] battlerGameObjects;

	private Dictionary<Character, Battler> characterBattlerDictionary;

    public Battler[] BattlerGameObjects => battlerGameObjects;

    public Dictionary<Character, Battler> CharacterBattlerDictionary => characterBattlerDictionary;

	#region Initialising
	public void Initialise(BattleManager battleManager)
	{
		this.battleManager = battleManager;
		_battleDataModel = ServiceLocator.Instance.BattleDataModel;
		_characterModel = ServiceLocator.Instance.CharacterModel;
		
		battlerGameObjects = new Battler[_characterModel.AllFrontCharactersList.Count];
		characterBattlerDictionary = new Dictionary<Character, Battler>();

        playerBattlers = new Battler[_characterModel.FrontPlayerCharacters.Length];
        enemyBattlers = new Battler[_characterModel.FrontEnemyCharacters.Length];
		playerBattlers = InitialisePlayerBattlers(_characterModel.FrontPlayerCharactersList);
        //enemyBattlers = InitialiseEnemyBattlers(_battleDataModel.EnemyBattlersCharacterInventory);

        battlerGameObjects = playerBattlers.Concat(enemyBattlers).ToArray();
	}

    private Battler[] InitialisePlayerBattlers(List<Character> characters)
    {
        int newCharacterStartIndex = playerBattlers.Length;
        Battler[] newPlayerBattlers = new Battler[_characterModel.FrontPlayerCharactersList.Count];
        for (int i = newCharacterStartIndex; i < playerBattlers.Length; i++)
        {
            int j = 0;
            Character character = characters[j];
            j++;
            if (character != null)
            {
                if (character.IsUnlocked && !character.IsRear)
                {
                    newPlayerBattlers[i] = ServiceLocator.Instance.CharacterGameObjectManager.CharacterBattlerDictionary[character];
                    newPlayerBattlers[i].Initialise(battleManager, character);
                    characterBattlerDictionary.Add(character, newPlayerBattlers[i]);
                }
            }
        }
		return newPlayerBattlers;
    }

/*    private Battler[] InitialiseEnemyBattlers(CharacterInventory characterInventory)
	{

        enemyBattlers = new Battler[_battleDataModel.EnemyBattlersCharacterInventory.CharacterSlots.Length];
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
    }*/

	public void OnEnterBattle()
	{

	}

	public void RefreshBattlersOnOrderChange()
	{
		if (!(ServiceLocator.Instance.GameStateManager.State == ServiceLocator.Instance.BattleState))
		{
			return;
		}
		Debug.LogError("For some reason this isn't called when we add a character from the rear");
        battlerGameObjects = new Battler[_characterModel.AllFrontCharactersList.Count];

		for (int i = characterBattlerDictionary.Keys.Count - 1; i >= 0; i--)
		{
			Character character = characterBattlerDictionary.ElementAt(i).Key;
            if (character.IsPlayer && !_characterModel.FrontPlayerCharacters.Contains(character))
			{
				characterBattlerDictionary[character].Uninitialise();
                characterBattlerDictionary.Remove(character);
            }
        }

        /*		foreach (Character character in characterBattlerDictionary.Keys) // Remove all players as they will be readded if they are still in the front.
                {
                    if (character.IsPlayer)
                    {
                        characterBattlerDictionary.Remove(character);
                    }
                }*/

        /*        foreach (Battler battler in playerBattlers)
                {
                    battler.Uninitialise();
                }*/

        IEnumerable<Character> uninitialisedCharacters = from uninitialised in _characterModel.FrontPlayerCharacters.Except(characterBattlerDictionary.Keys) select uninitialised;
        
        playerBattlers = new Battler[_characterModel.FrontPlayerCharactersList.Count];
        playerBattlers.Concat(InitialisePlayerBattlers(_characterModel.FrontPlayerCharactersList));
		Debug.Log("characterBattlerDictionary.Count: " + characterBattlerDictionary.Count);
        battlerGameObjects = playerBattlers.Concat(enemyBattlers).ToArray();
    }



	private Battler InitialiseEnemyBattler(CharacterSlot characterSlot, int index)
	{

        Battler battler = Instantiate(battlerPrefab, ServiceLocator.Instance.CharacterGameObjectManager.transform.position + new Vector3(2 + (2 * index), 0, 0), Quaternion.identity).GetComponent<Battler>();
        
		battler.Initialise(battleManager, characterSlot.Character);

		return battler;
	}

    #endregion


    #region EndOfLife

    public void OnExitBattle()
    {
		for (int i = 0; i < battlerGameObjects.Length; i++)
		{
			if (battlerGameObjects[i] != null)
			{
                battlerGameObjects[i].DisplayBattleUI(false);
            }
		}

        for (int i = 0; i < enemyBattlers.Length; i++)
        {
            Destroy(enemyBattlers[i].gameObject);
        }
    }
    #endregion
}
