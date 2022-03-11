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

	private Battler[] battlerGameObjects;

	private Dictionary<Character, Battler> characterBattlerDictionary;

	public CharacterInventory PlayerBattlersList { get { return playerBattlersList; } set { playerBattlersList = value; } }
	public CharacterInventory EnemyBattlersList { get { return enemyBattlersList; } set { enemyBattlersList = value; } }
	public CharacterInventory OrderedBattlersListNew { get { return orderedBattlersListNew; } set { orderedBattlersListNew = value; } }
	public CharacterInventory BattlersListNew { get { return battlersListNew; } set { battlersListNew = value; } }

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

		InitialiseBattlers(playerBattlersList, true);
		InitialiseBattlers(enemyBattlersList, false);

		battleManager.BattleDataModel.OnCurrentActorChanged += UpdateDisplay;
	}

	private void InitialiseBattlers(CharacterInventory characterInventory, bool isPlayer)
	{
		for (int i = 0; i < characterInventory.CharacterSlots.Length; i++)
		{
			CharacterSlot characterSlot = characterInventory.CharacterSlots[i];
			Debug.LogWarning("Broken.");
			//Debug.Log(!characterSlot.Character.PartyCharacterTemplate.PartyCharacter.IsRear);
			if (characterSlot.Character != null)
			{
				if (isPlayer)
				{
					if (true) //characterSlot.CharacterTemplate.IsUnlocked) //&& !characterSlot.Character.PartyCharacterTemplate.PartyCharacter.IsRear)
					{
						battlerGameObjects[i] = InitialiseBattler(characterSlot, isPlayer, i);
						characterBattlerDictionary.Add(characterSlot.Character, battlerGameObjects[i]);
					}
				}
				else
				{
					battlerGameObjects[i] = InitialiseBattler(characterSlot, isPlayer, i);
					characterBattlerDictionary.Add(characterSlot.Character, battlerGameObjects[i]);
				}
			}
		}
	}

	private Battler InitialiseBattler(CharacterSlot characterSlot, bool isPlayer, int index)
	{
		Transform parent;
		parent = (isPlayer)? playerTransform : enemyTransform;

		Battler battler = UnityEngine.Object.Instantiate(battlerPrefab, parent.GetChild(index)).GetComponent<Battler>();

		battler.Initialise(battleManager, characterSlot.Character, battleManager.BattleBehaviour.BattleCamera);

		return battler;
	}

	#endregion

	public void UpdateDisplay()
	{

	}

	#region EndOfLife

	private void OnDestroy()
	{
		battleManager.BattleDataModel.OnCurrentActorChanged -= UpdateDisplay;
	}
	#endregion
}
