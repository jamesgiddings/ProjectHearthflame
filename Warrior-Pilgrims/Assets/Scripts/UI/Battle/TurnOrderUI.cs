using GramophoneUtils.Characters;
using GramophoneUtils.Stats;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnOrderUI : MonoBehaviour
{
	[SerializeField] private Transform _characterSlotsHolder;

	[SerializeField] private GameObject _characterTurnSlotPrefab;

	private CharacterModel _characterModel;
	private BattleDataModel _battleDataModel;
	private List<CharacterTurnSlotUI> _characterTurnSlotUIs;

    #region API

    public void Initialise()
	{
		_battleDataModel = ServiceLocator.Instance.BattleDataModel;
		_characterModel = ServiceLocator.Instance.CharacterModel;
    ServiceLocator.Instance.BattleManager.BattleDataModel.OnCurrentActorChanged += UpdateTurnOrderUI;

		_characterTurnSlotUIs = new List<CharacterTurnSlotUI>();

		foreach (Character character in _characterModel.AllCharacters)
		{
			CharacterTurnSlotUI characterTurnSlotUI = Instantiate(_characterTurnSlotPrefab, _characterSlotsHolder).GetComponent<CharacterTurnSlotUI>();
			_characterTurnSlotUIs.Add(characterTurnSlotUI);
			characterTurnSlotUI.SubscribeToOnCurrentActorChanged(ServiceLocator.Instance.BattleManager.BattleDataModel.OnCurrentActorChanged);
			characterTurnSlotUI.SlotResource = character;
		}
	}

	public void ReinitialiseOnBattlerChange()
	{
		RemoveAllSlots();
        Initialise();
	}

	public void UpdateTurnOrderUI()
	{
		for (int i = 0; i < _characterTurnSlotUIs.Count; i++)
		{
			_characterTurnSlotUIs[i].SlotResource = _battleDataModel.OrderedBattlersList[i];
			_characterTurnSlotUIs[i].Character = _battleDataModel.OrderedBattlersList[i];
            _characterTurnSlotUIs[i].GetIsCurrentCharacter();
			_characterTurnSlotUIs[i].UpdateSlotUI();
		}
	}

    #endregion

    #region Utilities

	private void RemoveAllSlots()
	{
        for (int i = 0; i < _characterSlotsHolder.childCount; i++)
        {
            Destroy(_characterSlotsHolder.GetChild(i).gameObject);
        }
    }

    #endregion
}
