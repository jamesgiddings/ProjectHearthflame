using GramophoneUtils.Stats;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStatusEffectBlueprint
{
    #region Attributes/Fields/Properties

    public StatusEffectType StatusEffectTypeFlag { get; }
    public List<IStatModifier> StatModifiers { get; }
    public Damage[][] TurnDamageEffects { get; }
    public Healing[][] TurnHealingEffects { get; }
    public Move[][] TurnMoveEffects { get; }
    public int Duration { get; }
    public bool DamageMustLandForOtherEffectsToLand { get; }
    public string TooltipText { get; }
    public Sprite Icon { get; }

    #endregion

    #region Constructors
    #endregion

    #region Callbacks
    #endregion

    #region Public Functions
    #endregion

    #region Protected Functions
    #endregion

    #region Private Functions
    #endregion

    #region Inner Classes
    #endregion
}
