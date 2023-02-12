using GramophoneUtils.Characters;
using GramophoneUtils.Stats;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GramophoneUtils.Stats
{
    public interface IStatModifier : IApplyable, IResource
    {
        #region Attributes/Fields/Properties

        IStatType StatType { get; }
        ModifierNumericType ModifierNumericType { get; }
        StatModifierType StatModifierType { get; }
        float Value { get; }
        object[] Sources { get; }

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
}