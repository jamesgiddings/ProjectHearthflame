using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnOrderUI : MonoBehaviour
{
	[SerializeField] Transform characterSlotsHolder;

	[SerializeField] GameObject characterSlotPrefab;

	private BattleManager battleManager;
	private CharacterInventory characterInventory;

	public void Initialise(BattleManager battleManager) 
	{
		this.battleManager = battleManager;
		this.characterInventory = battleManager.BattlersListNew;
		//characterInventory.onCharactersUpdated.AddListener(UpdateTurnOrderUI);
		foreach (CharacterSlot characterSlot in characterInventory.CharacterSlots)
		{
			CharacterSlotUI characterSlotUI = UnityEngine.Object.Instantiate(characterSlotPrefab, characterSlotsHolder).GetComponent<CharacterSlotUI>();
			characterSlotUI.SlotResource = characterSlot.character;
		}
		characterInventory.onCharactersUpdated.Raise();
	}

	public void UpdateTurnOrderUI()
	{

	}
}
