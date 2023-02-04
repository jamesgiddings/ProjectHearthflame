using AYellowpaper;
using GramophoneUtils.Characters;
using GramophoneUtils.Stats;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusEffectBarUI : MonoBehaviour, IStatusEffectBarUI
{
    #region Attributes/Fields/Properties

    [SerializeField] private GameObject _statusEffectIconPrefab;

    [SerializeField] private StatusEffectBarTooltipUI _statusEffectTooltipUI;

    [SerializeField] private GameObject _gridLayoutGroup;

    private bool _initialised = false;

    private IStatusEffectBarTooltipUI _tooltipComponent 
    {
        get 
        { 
            if (_tooltipComponent == null)
            {
                _tooltipComponent = _statusEffectTooltipUI.GetComponent<IStatusEffectBarTooltipUI>();
            }
            return _tooltipComponent; 
        }
        set
        {
            _tooltipComponent = value;
        }
    }

    private Character _character;

    private List<IStatusEffect> _statusEffects;

    #endregion

    #region Constructors
    #endregion

    #region Callbacks

    private void OnDestroy()
    {
        if (_initialised)
        {
            _character.StatSystem.OnStatusEffectsListUpdated -= UpdateStatusEffects;
        }
    }

    #endregion

    #region Public Functions
    public void Initialise(Character character)
    {
        _character = character;
        _statusEffects = new List<IStatusEffect>();
        _character.StatSystem.OnStatusEffectsListUpdated += UpdateStatusEffects;
        _initialised = true;
    }

    #endregion

    #region Protected Functions
    #endregion

    #region Private Functions

    private void UpdateStatusEffects(List<IStatusEffect> statusEffects)
    {
        _statusEffects = statusEffects;

        // Clear the current status effect icons
        foreach (Transform child in _gridLayoutGroup.transform)
        {
            Destroy(child.gameObject);
        }

        // Create a new icon for each status effect
        foreach (var statusEffect in _statusEffects)
        {
            var statusEffectIcon = Instantiate(_statusEffectIconPrefab, _gridLayoutGroup.transform);
            statusEffectIcon.GetComponent<Image>().sprite = statusEffect.Icon;
            //statusEffectIcon.GetComponent<Button>().onClick.AddListener(() => OnStatusEffectIconClicked(statusEffect));
        }

    }

    private void OnStatusEffectIconClicked(IStatusEffect statusEffect)
    {        
        _tooltipComponent.ShowTooltip(true, statusEffect);
    }

    #endregion

    #region Inner Classes
    #endregion
}
