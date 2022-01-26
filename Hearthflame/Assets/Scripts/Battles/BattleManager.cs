using GramophoneUtils.Stats;
using UnityEngine;
using CodeMonkey.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

public class BattleManager
{

	[SerializeField] private Transform pfCharacterBattle;
	[SerializeField] private Texture2D playerSpriteSheet;
	[SerializeField] private Texture2D enemySpriteSheet;
	[SerializeField] private Battle battle;
	[SerializeField] private PlayerBehaviour playerBehaviour;    

	private int turn;
	private BattleState battleState;
	private List<Character> battlersList;

	public enum BattleState
	{
		Busy,
		WaitingForPlayer,
	}

	public BattleManager(Battle battle, PlayerBehaviour player)
	{
		this.battle = battle;
		this.playerBehaviour = player;
		InitialiseBattle();
		FunctionUpdater.Create(() => Update());
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{
			PlayerAction();
		}
	}

	private void PlayerAction()
	{
		Debug.Log("Attack");
		NextTurn();
	}

	private void NextTurn()
	{
		CalculateNextActor();
	}

	private void CalculateNextActor()
	{
		foreach (Character battler in battlersList)
		{
			Debug.Log("Order before: " + battler.Name);
			Debug.Log("Order before: " + battler.StatSystem.GetStatValue(battler.StatTypeStringRefDictionary["Dexterity"]));
		}
		IEnumerable<Character> query = battlersList.OrderBy(character => character.StatSystem.GetStatValue(character.StatTypeStringRefDictionary["Dexterity"]) * - 1); // * - 1 reverses the order of the list
		foreach (Character battler in query)
		{
			Debug.Log("Order after: " + battler.Name);
			Debug.Log("Order after: " + battler.StatSystem.GetStatValue(battler.StatTypeStringRefDictionary["Dexterity"]));
		}
	}

	private void InitialiseBattle()
	{
		InitialiseBattlersList();
		
		SpawnCharacter(true);
		SpawnCharacter(false);

		turn = 0;
		CalculateNextActor();
		EndBattle(true);
	}

	private void InitialiseBattlersList()
	{
		battlersList = new List<Character>();
		foreach (PartyCharacter partyCharacter in playerBehaviour.PartyCharacters)
		{
			if (partyCharacter.IsUnlocked) // && IsFront
			{
				battlersList.Add(partyCharacter.Character);
			}
		}
		foreach (CharacterTemplate characterTemplate in battle.EnemyBattlers)
		{
			battlersList.Add(new Character(characterTemplate, playerBehaviour)); 
		}
	}

	private void EndBattle(bool victorious)
	{
		if (victorious)
		{
			battle.BattleReward.AddBattleReward(playerBehaviour);
		}
		else
		{
			Debug.LogError("Not implemented.");
		}
	}

	private void SpawnCharacter(bool isPlayerTeam)
	{
		//Vector3 position;
		//if (isPlayerTeam)
		//{
		//	position = new Vector3(-5, 0);
		//}
		//else
		//{
		//	position = new Vector3(5, 0);
		//}
		//Instantiate(pfCharacterBattle, position, Quaternion.identity);
	}
}
