using System;
using System.Collections.Generic;
using UnityEngine;

namespace GramophoneUtils.Stats
{
    public interface IStatusEffect : IElapsible, IResource
    {
        #region Attributes/Fields/Properties

        StatusEffectTypeWrapper StatusEffectTypeWrapper { get; }
        List<IStatModifier> StatModifiers { get; }
        Damage[][] TurnDamageEffects { get; }
        Healing[][] TurnHealingEffects { get; }
        Move[][] TurnMoveEffects { get; }
        object Source { get; }
        bool DamageMustLandForOtherEffectsToLand { get; }
        string TooltipText { get; }
        Sprite Icon { get; }

        #endregion

        #region Constructors
        #endregion

        #region Callbacks
        #endregion

        #region Public Functions

        bool Equals(object obj);

        #endregion

        #region Protected Functions
        #endregion

        #region Private Functions
        #endregion

        #region Inner Classes
        #endregion
    }
}