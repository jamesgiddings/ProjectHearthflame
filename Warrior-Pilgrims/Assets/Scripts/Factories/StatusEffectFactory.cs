using GramophoneUtils.Stats;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Status Effect Factory", menuName = "Factories/Status Effect Factory")]
public class StatusEffectFactory : ScriptableObject, IStatusEffectFactory
{
    #region Attributes/Fields/Properties
    #endregion

    #region Constructors
    #endregion

    #region Callbacks
    #endregion

    #region Public Functions

    public IStatusEffect CreateStatusEffectFromBlueprint(IStatusEffectBlueprint statusEffectBlueprint, object source = null)
    {
        return new StatusEffect(
            statusEffectBlueprint.StatusEffectTypeFlag,
            statusEffectBlueprint.StatModifiers,
            statusEffectBlueprint.TurnDamageEffects,
            statusEffectBlueprint.TurnHealingEffects,
            statusEffectBlueprint.TurnMoveEffects,
            statusEffectBlueprint.TooltipText,
            statusEffectBlueprint.Icon,
            statusEffectBlueprint.DamageMustLandForOtherEffectsToLand,
            statusEffectBlueprint.Duration,
            source
            );
    }

    public IStatusEffect CreateStatusEffectFromInstance(IStatusEffect statusEffect)
    {
        return new StatusEffect(
            statusEffect.StatusEffectTypeWrapper.StatusEffectTypeFlag,
            statusEffect.StatModifiers,
            statusEffect.TurnDamageEffects,
            statusEffect.TurnHealingEffects,
            statusEffect.TurnMoveEffects,
            statusEffect.TooltipText,
            statusEffect.Icon,
            statusEffect.DamageMustLandForOtherEffectsToLand,
            statusEffect.Duration,
            statusEffect.Source
            );
    }

    public IStatusEffect CreateStatusEffectFromValue(
        StatusEffectType statusEffectTypeFlag, 
        List<IStatModifier> statModifiers, 
        Damage[][] turnDamageEffects, 
        Healing[][] turnHealingEffects, 
        Move[][] turnMoveEffects,
        string tooltipText,
        Sprite icon,
        bool damageMustLandForOtherEffectsToLand,
        int duration, 
        object source
        )
    {
        return new StatusEffect(
            statusEffectTypeFlag,
            statModifiers,
            turnDamageEffects,
            turnHealingEffects,
            turnMoveEffects,
            tooltipText,
            icon,
            damageMustLandForOtherEffectsToLand,
            duration,
            source
            );
    }

    #endregion

    #region Protected Functions
    #endregion

    #region Private Functions
    #endregion

    #region Inner Classes
    #endregion

}
