using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace GramophoneUtils.Stats
{
    [Serializable]
    public class TurnDamageHealingAndMoveEffects
    {
        #region Attributes/Fields/Properties

        [SerializeField, Required] private HealingBlueprint[] _turnHealingEffects;
        public HealingBlueprint[] TurnHealingEffects => _turnHealingEffects;


        [SerializeField, Required] private DamageBlueprint[] _turnDamageEffects;
        public DamageBlueprint[] TurnDamageEffects => _turnDamageEffects;


        [SerializeField, Required] private MoveBlueprint[] _turnMoveEffects;
        public MoveBlueprint[] TurnMoveEffects => _turnMoveEffects;

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