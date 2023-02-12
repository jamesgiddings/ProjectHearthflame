using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UI Constants", menuName = "Constants/UI Constants")]
public class UIConstants : ScriptableObject
{
    [BoxGroup("Tooltips")]
    [SerializeField] private string _tooltipTitleSizeOpenTag;
    public string TOOLTIP_TITLE_SIZE_OPEN_TAG => _tooltipTitleSizeOpenTag;

    [BoxGroup("Tooltips")]
    [SerializeField] private string _tooltipSizeCloseTag;
    public string TOOLTIP_SIZE_CLOSE_TAG => _tooltipSizeCloseTag;

    [BoxGroup("Tooltips")]
    [SerializeField] private string _tooltipCharacterClassRestrictionColourOpenTag;
    public string TOOLTIP_CHARACTER_CLASS_RESTRICTION_COLOUR_OPEN_TAG => _tooltipCharacterClassRestrictionColourOpenTag;

    [BoxGroup("Tooltips")]
    [SerializeField] private string _tooltipColourCloseTag;
    public string TOOLTIP_COLOUR_CLOSE_TAG => _tooltipColourCloseTag;

    [BoxGroup("Tooltips")]
    [SerializeField] private string _tooltipBurnColourOpenTag;
    public string TOOLTIP_BURN_COLOUR_OPEN_TAG => _tooltipBurnColourOpenTag;

    [BoxGroup("Tooltips")]
    [SerializeField] private string _tooltipStunColourOpenTag;
    public string TOOLTIP_STUN_COLOUR_OPEN_TAG => _tooltipStunColourOpenTag;

    [BoxGroup("Tooltips")]
    [SerializeField] private string _tooltipBleedColourOpenTag;
    public string TOOLTIP_BLEED_COLOUR_OPEN_TAG => _tooltipBleedColourOpenTag;

    [BoxGroup("Tooltips")]
    [SerializeField] private string _tooltipFlavourTextOpenTag;
    public string TOOLTIP_FLAVOUR_TEXT_OPEN_TAG => _tooltipFlavourTextOpenTag;

    [BoxGroup("Tooltips")]
    [SerializeField] private string _tooltipFlavourTextCloseTag;
    public string TOOLTIP_FLAVOUR_TEXT_CLOSE_TAG => _tooltipFlavourTextCloseTag;
}
