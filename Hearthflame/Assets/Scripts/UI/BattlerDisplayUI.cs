using GramophoneUtils.Stats;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattlerDisplayUI : MonoBehaviour
{
	[SerializeField] private Transform playerTransform;
	[SerializeField] private Transform enemyTransform;
	[SerializeField] private Transform battlerPrefab;
	
	private BattleManager battleManager;

	private CharacterInventory battlersListNew;
	private CharacterInventory orderedBattlersListNew;
	private CharacterInventory enemyBattlersList;
	private CharacterInventory playerBattlersList;

	private BattlerSprite[] battlerGameObjects;

	public CharacterInventory PlayerBattlersList { get { return playerBattlersList; } set { playerBattlersList = value; } }
	public CharacterInventory EnemyBattlersList { get { return enemyBattlersList; } set { enemyBattlersList = value; } }
	public CharacterInventory OrderedBattlersListNew { get { return orderedBattlersListNew; } set { orderedBattlersListNew = value; } }
	public CharacterInventory BattlersListNew { get { return battlersListNew; } set { battlersListNew = value; } }


	#region Initialising
	public void Initialise(BattleManager battleManager)
	{
		this.battleManager = battleManager;
		
		this.battlersListNew = battleManager.BattlersListNew;
		this.orderedBattlersListNew = battleManager.OrderedBattlersListNew;
		this.enemyBattlersList = battleManager.EnemyBattlersList;
		this.playerBattlersList = battleManager.PlayerBattlersList;

		battlerGameObjects = new BattlerSprite[battlersListNew.CharacterSlots.Length];

		InitialiseBattlers(playerBattlersList, true);
		InitialiseBattlers(enemyBattlersList, false);

		battleManager.OnCurrentActorChanged += UpdateDisplay;
	}

	private void InitialiseBattlers(CharacterInventory characterInventory, bool isPlayer)
	{
		for (int i = 0; i < characterInventory.CharacterSlots.Length; i++)
		{
			CharacterSlot characterSlot = characterInventory.CharacterSlots[i];
			Debug.Log(characterSlot.PartyCharacterTemplate.IsUnlocked);
			//Debug.Log(!characterSlot.Character.PartyCharacterTemplate.PartyCharacter.IsRear);

			if (isPlayer)
			{
				if (characterSlot.PartyCharacterTemplate.IsUnlocked) //&& !characterSlot.Character.PartyCharacterTemplate.PartyCharacter.IsRear)
				{
					battlerGameObjects[i] = InitialiseBattler(characterSlot, isPlayer);
				}
			}
			else
			{
				battlerGameObjects[i] = InitialiseBattler(characterSlot, isPlayer);
			}
		}
	}

	private BattlerSprite InitialiseBattler(CharacterSlot characterSlot, bool isPlayer)
	{
		Transform parent;
		parent = (isPlayer)? playerTransform : enemyTransform;

		BattlerSprite battler = UnityEngine.Object.Instantiate(battlerPrefab, parent).GetComponent<BattlerSprite>();

		battler.Initialise(battleManager, characterSlot.Character);

		return battler;
	}

	#endregion

	public void UpdateDisplay()
	{

	}
}
