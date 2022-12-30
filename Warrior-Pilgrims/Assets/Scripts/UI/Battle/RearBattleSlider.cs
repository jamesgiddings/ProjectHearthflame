using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GramophoneUtils.Battles
{
    [RequireComponent(typeof(Slider))]
    public class RearBattleSlider : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _checkDisplayNameText;

        private Slider _slider;
        private bool _isSubscribedToAction = false;
        private CheckInstance _checkInstance;

        #region Callbacks

        private void Awake()
        {
           _slider = GetComponent<Slider>();
        }

        private void OnEnable()
        {
            _slider = GetComponent<Slider>();
            if (_slider.enabled)
            {
                _checkInstance.OnCheckInstanceChanged += UpdateDisplay;
                _isSubscribedToAction = true;
            }
        }

        void Start()
        {
            if (_checkInstance == null)
            {
                return;
            }
            UpdateDisplay(_checkInstance);
        }

        private void OnDisable()
        {
            if (_isSubscribedToAction)
            {
                _checkInstance.OnCheckInstanceChanged -= UpdateDisplay;
            }
        }

        #endregion

        #region API

        public void UpdateDisplay(CheckInstance checkInstance)
        {
            _slider.value = checkInstance.GetProgress();
            _checkDisplayNameText.text = checkInstance.DisplayName;
        }

        public void Initialise(CheckInstance checkInstance)
        {
            _checkInstance = checkInstance;
            gameObject.SetActive(true);
        }

        #endregion
    }
}