using GramophoneUtils.Battles;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RearBattleUI : MonoBehaviour
{
    [SerializeField] private GameObject _sliderPrefab;
    [SerializeField] private Transform _sliderParent;

    private BattleDataModel _battleDataModel;

    #region Callbacks

    private void OnEnable()
    {
        _battleDataModel = ServiceLocator.Instance.BattleDataModel;
        foreach (CheckInstance checkInstance in _battleDataModel.BattleRear.GetAllCheckInstances())
        {
            InstantiateRearBattleSlider(checkInstance);
        }
    }

    private void OnDisable()
    {
        for (int i = 0; i < _sliderParent.childCount; i++)
        {
            Destroy(_sliderParent.GetChild(i).gameObject);
        }
    }

    #endregion

    #region Utilities

    private void InstantiateRearBattleSlider(CheckInstance checkInstance)
    {
        RearBattleSlider rearBattleSlider = Instantiate(_sliderPrefab, _sliderParent).GetComponent<RearBattleSlider>();
        rearBattleSlider.Initialise(checkInstance);
    }

    #endregion
}
