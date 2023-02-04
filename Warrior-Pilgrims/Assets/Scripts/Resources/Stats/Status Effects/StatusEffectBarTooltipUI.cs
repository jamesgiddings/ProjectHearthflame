using GramophoneUtils.Stats;
using UnityEngine;
using TMPro;


public class StatusEffectBarTooltipUI : MonoBehaviour, IStatusEffectBarTooltipUI
{
    #region Attributes/Fields/Properties

    [SerializeField] private TextMeshProUGUI _textMeshProUGUI;

    #endregion

    #region Constructors
    #endregion

    #region Callbacks

    #endregion

    #region Public Functions
    public void ShowTooltip(bool value, IStatusEffect statusEffect = null)
    {
        _textMeshProUGUI.text = statusEffect == null ? "" : statusEffect.TooltipText;
        gameObject.SetActive(value);
    }

    #endregion

    #region Protected Functions
    #endregion

    #region Private Functions
    #endregion

    #region Inner Classes
    #endregion

}
