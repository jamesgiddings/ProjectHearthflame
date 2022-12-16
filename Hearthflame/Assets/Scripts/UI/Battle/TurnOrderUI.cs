using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnOrderUI : MonoBehaviour
{
	[SerializeField] private Transform _characterSlotsHolder;

	[SerializeField] private GameObject _characterTurnSlotPrefab;

	private BattleManager _battleManager;
	private CharacterInventory _characterInventory;
	private List<CharacterTurnSlotUI> _characterTurnSlotUIs;

	public void Initialise(BattleManager battleManager) 
	{
		this._battleManager = battleManager;
		this._characterInventory = battleManager.OrderedBattlersCharacterInventory;
		battleManager.BattleDataModel.OnCurrentActorChanged += UpdateTurnOrderUI;

		_characterTurnSlotUIs = new List<CharacterTurnSlotUI>();

		foreach (CharacterSlot characterSlot in _characterInventory.CharacterSlots)
		{
			CharacterTurnSlotUI characterTurnSlotUI = UnityEngine.Object.Instantiate(_characterTurnSlotPrefab, _characterSlotsHolder).GetComponent<CharacterTurnSlotUI>();
			characterTurnSlotUI.CharacterInventory = _characterInventory;
			_characterTurnSlotUIs.Add(characterTurnSlotUI);
			characterTurnSlotUI.SubscribeToOnCurrentActorChanged(battleManager.BattleDataModel.OnCurrentActorChanged);
			characterTurnSlotUI.SlotResource = characterSlot.CharacterTemplate;
		}
		_characterInventory.onCharactersUpdated.Raise();
	}

	public void UpdateTurnOrderUI()
	{
		this._characterInventory = _battleManager.OrderedBattlersCharacterInventory;
		for (int i = 0; i < _characterTurnSlotUIs.Count; i++)
		{
			_characterTurnSlotUIs[i].SlotResource = _characterInventory.GetSlotByIndex(i).CharacterTemplate;
			_characterTurnSlotUIs[i].Character = _characterInventory.GetSlotByIndex(i).Character;
			_characterTurnSlotUIs[i].GetIsCurrentCharacter();
			_characterTurnSlotUIs[i].UpdateSlotUI();
		}
	}
}
