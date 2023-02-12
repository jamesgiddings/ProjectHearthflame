using UnityEngine;

namespace GramophoneUtils.Stats
{
    [CreateAssetMenu(fileName = "Stat Modifier Factory", menuName = "Factories/Stat Modifier Factory")]
    public class StatModifierFactory : ScriptableObject, IStatModifierFactory
    {
        #region Attributes/Fields/Properties
        #endregion

        #region Constructors
        #endregion

        #region Callbacks
        #endregion

        #region Public Functions

        public IStatModifier CreateStatModifierFromBlueprint(IStatModifierBlueprint statModifierBlueprint, object[] sources = null)
        {
            return new StatModifier(
                statModifierBlueprint.Name,
                statModifierBlueprint.UID,
                statModifierBlueprint.Sprite,
                statModifierBlueprint.StatType,
                statModifierBlueprint.ModifierNumericType,
                statModifierBlueprint.StatModifierType,
                statModifierBlueprint.Value,
                sources
                );
        }

        public IStatModifier CreateStatModifierFromInstance(IStatModifier statModifier)
        {
            return new StatModifier(
                statModifier.Name,
                statModifier.UID,
                statModifier.Sprite,
                statModifier.StatType,
                statModifier.ModifierNumericType,
                statModifier.StatModifierType,
                statModifier.Value,
                statModifier.Sources
                );
        }

        public IStatModifier CreateStatModifierFromInstance(IStatModifier statModifier, object[] sources = null)
        {
            object[] sourcesOverride = sources == null ? statModifier.Sources : sources;

            return new StatModifier(
                statModifier.Name,
                statModifier.UID,
                statModifier.Sprite,
                statModifier.StatType,
                statModifier.ModifierNumericType,
                statModifier.StatModifierType,
                statModifier.Value,
                sourcesOverride
                );
        }

        public IStatModifier CreateStatModifierFromValue(
            string name, 
            string uid, 
            Sprite sprite, 
            IStatType statType, 
            ModifierNumericType modifierNumericType, 
            StatModifierType statModifierType, 
            float value, 
            object[] sources = null)
        {
            return new StatModifier(
                name,
                uid,
                sprite,
                statType,
                modifierNumericType,
                statModifierType,
                value,
                sources
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

}
