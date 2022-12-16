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



    #endregion

    /*    [SerializeField] TextMeshProUGUI statLabel;
        [SerializeField] TextMeshProUGUI statValue;
        [SerializeField] Character character;

        private void OnEnable()
        {
            character.HealthSystem.OnHealthChanged += UpdateHealthDisplay;
            UpdateHealthDisplay();
        }

        public void Initialise(Character character)
        {
            this.character = character;
        }

        public void UpdateHealthDisplay(int value = 0)
        {
            statLabel.text = "Health: ";
            statValue.text = character.HealthSystem.CurrentHealth.ToString() + "/" + character.HealthSystem.MaxHealth.ToString();
        }*/


    #region Utilities

    private void InstantiateRearBattleSlider(CheckInstance checkInstance)
    {
        RearBattleSlider rearBattleSlider = Instantiate(_sliderPrefab, _sliderParent).GetComponent<RearBattleSlider>();
        rearBattleSlider.Initialise(checkInstance);
    }

    #endregion
}
