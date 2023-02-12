using GramophoneUtils.Stats;
using System.Collections.Generic;
using UnityEngine;

public interface IStatusEffectFactory
{
    #region Attributes/Fields/Properties
    #endregion

    #region Constructors
    #endregion

    #region Callbacks
    #endregion

    #region Public Functions

    IStatusEffect CreateStatusEffectFromInstance(IStatusEffect statusEffect);
    IStatusEffect CreateStatusEffectFromValue(StatusEffectType statusEffectTypeFlag, List<IStatModifier> statModifiers, Damage[][] turnDamageEffects, Healing[][] turnHealingEffects, Move[][] turnMoveEffects, string tooltip, string name, Sprite icon, bool damageMustLandForOtherEffectsToLand, int duration, object source);
    IStatusEffect CreateStatusEffectFromBlueprint(IStatusEffectBlueprint statusEffectBlueprint, object source = null);
    
    #endregion

    #region Protected Functions
    #endregion

    #region Private Functions
    #endregion

    #region Inner Classes
    #endregion
}
