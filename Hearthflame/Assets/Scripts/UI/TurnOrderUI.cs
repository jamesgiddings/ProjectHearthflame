using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnOrderUI : MonoBehaviour
{
	[SerializeField] Transform characterSlotsHolder;

	[SerializeField] GameObject characterTurnSlotPrefab;

	private BattleManager battleManager;
	private CharacterInventory characterInventory;
	private List<CharacterTurnSlotUI> characterTurnSlotUIs;

	public void Initialise(BattleManager battleManager) 
	{
		this.battleManager = battleManager;
		this.characterInventory = battleManager.OrderedBattlersCharacterInventory;
		battleManager.OnCurrentActorChanged += UpdateTurnOrderUI;

		characterTurnSlotUIs = new List<CharacterTurnSlotUI>();

		foreach (CharacterSlot characterSlot in characterInventory.CharacterSlots)
		{
			CharacterTurnSlotUI characterTurnSlotUI = UnityEngine.Object.Instantiate(characterTurnSlotPrefab, characterSlotsHolder).GetComponent<CharacterTurnSlotUI>();
			characterTurnSlotUI.CharacterInventory = characterInventory;
			characterTurnSlotUIs.Add(characterTurnSlotUI);
			characterTurnSlotUI.SubscribeToOnCurrentActorChanged(battleManager.OnCurrentActorChanged);
			characterTurnSlotUI.SlotResource = characterSlot.PartyCharacterTemplate;
		}
		characterInventory.onCharactersUpdated.Raise();
	}

	public void UpdateTurnOrderUI()
	{
		this.characterInventory = battleManager.OrderedBattlersCharacterInventory;
		for (int i = 0; i < characterTurnSlotUIs.Count; i++)
		{
			characterTurnSlotUIs[i].SlotResource = characterInventory.GetSlotByIndex(i).PartyCharacterTemplate;
			characterTurnSlotUIs[i].Character = characterInventory.GetSlotByIndex(i).Character;
			Debug.Log("New order: " + characterInventory.GetSlotByIndex(i).Character.Name);
			characterTurnSlotUIs[i].GetIsCurrentCharacter();
			characterTurnSlotUIs[i].UpdateSlotUI();
		}
	}
}
