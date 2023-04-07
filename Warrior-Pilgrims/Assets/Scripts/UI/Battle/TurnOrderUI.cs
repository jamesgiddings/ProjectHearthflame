using GramophoneUtils.Stats;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

public class TurnOrderUI : MonoBehaviour
{
	[SerializeField] private Transform _characterSlotsHolder;

	[SerializeField] private GameObject _characterTurnSlotPrefab;

	private ICharacterModel _characterModel;
	private BattleDataModel _battleDataModel;
	private List<CharacterTurnSlotUI> _characterTurnSlotUIs;

	#region Callbacks

	private void OnDestroy()
	{
        ServiceLocator.Instance.BattleManager.BattleDataModel.OnCurrentActorChanged = null;
    }

	#endregion

	#region Public Functions

	public void Initialise()
	{
		_battleDataModel = ServiceLocator.Instance.BattleDataModel;
		_characterModel = ServiceLocator.Instance.CharacterModel;
		ServiceLocator.Instance.BattleManager.BattleDataModel.OnCurrentActorChanged = null;

        ServiceLocator.Instance.BattleManager.BattleDataModel.OnCurrentActorChanged += UpdateTurnOrderUI;

		_characterTurnSlotUIs = new List<CharacterTurnSlotUI>();

		foreach (ICharacter character in _characterModel.AllCharacters)
		{
			CharacterTurnSlotUI characterTurnSlotUI = Instantiate(_characterTurnSlotPrefab, _characterSlotsHolder).GetComponent<CharacterTurnSlotUI>();
			_characterTurnSlotUIs.Add(characterTurnSlotUI);
			characterTurnSlotUI.SubscribeToOnCurrentActorChanged(ServiceLocator.Instance.BattleManager.BattleDataModel.OnCurrentActorChanged);
			characterTurnSlotUI.SlotResource = character;
		}
	}

	public void ReinitialiseOnBattlerChange()
	{
        ServiceLocator.Instance.BattleManager.BattleDataModel.OnCurrentActorChanged = null;
        RemoveAllSlots();
        Initialise();
	}

	[Button]
	public void UpdateTurnOrderUI()
	{
		ReinitialiseOnBattlerChange();

        for (int i = 0; i < _characterTurnSlotUIs.Count; i++)
		{
			_characterTurnSlotUIs[i].SlotResource = _battleDataModel.OrderedBattlersList[i];
			_characterTurnSlotUIs[i].Character = _battleDataModel.OrderedBattlersList[i];
            _characterTurnSlotUIs[i].GetIsCurrentCharacter();
			_characterTurnSlotUIs[i].UpdateSlotUI();
		}
	}

    #endregion

    #region Private Functions

	private void RemoveAllSlots()
	{
        for (int i = 0; i < _characterSlotsHolder.childCount; i++)
        {
            Destroy(_characterSlotsHolder.GetChild(i).gameObject);
        }
    }

    #endregion
}
