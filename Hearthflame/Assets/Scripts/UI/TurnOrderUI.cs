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
		battleManager.BattleDataModel.OnCurrentActorChanged += UpdateTurnOrderUI;

		characterTurnSlotUIs = new List<CharacterTurnSlotUI>();

		foreach (CharacterSlot characterSlot in characterInventory.CharacterSlots)
		{
			CharacterTurnSlotUI characterTurnSlotUI = UnityEngine.Object.Instantiate(characterTurnSlotPrefab, characterSlotsHolder).GetComponent<CharacterTurnSlotUI>();
			characterTurnSlotUI.CharacterInventory = characterInventory;
			characterTurnSlotUIs.Add(characterTurnSlotUI);
			characterTurnSlotUI.SubscribeToOnCurrentActorChanged(battleManager.BattleDataModel.OnCurrentActorChanged);
			characterTurnSlotUI.SlotResource = characterSlot.CharacterTemplate;
		}
		characterInventory.onCharactersUpdated.Raise();
	}

	public void UpdateTurnOrderUI()
	{
		this.characterInventory = battleManager.OrderedBattlersCharacterInventory;
		for (int i = 0; i < characterTurnSlotUIs.Count; i++)
		{
			characterTurnSlotUIs[i].SlotResource = characterInventory.GetSlotByIndex(i).CharacterTemplate;
			characterTurnSlotUIs[i].Character = characterInventory.GetSlotByIndex(i).Character;
			characterTurnSlotUIs[i].GetIsCurrentCharacter();
			characterTurnSlotUIs[i].UpdateSlotUI();
		}
	}
}
