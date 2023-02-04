using GramophoneUtils.Stats;

public interface IStatModifierFactory
{
    #region Attributes/Fields/Properties
    #endregion

    #region Constructors
    #endregion

    #region Callbacks
    #endregion

    #region Public Functions

    IStatModifier CreateStatModifierFromInstance(IStatModifier statModifier);
    IStatModifier CreateStatModifierFromInstance(IStatModifier statModifier, object[] sources);
    IStatModifier CreateStatModifierFromValue(IStatType statType, ModifierNumericType modifierNumericType, StatModifierType statModifierType, float value, object[] sources = null);
    IStatModifier CreateStatModifierFromBlueprint(IStatModifierBlueprint statModifierBlueprint, object[] sources = null);

    #endregion

    #region Protected Functions
    #endregion

    #region Private Functions
    #endregion

    #region Inner Classes
    #endregion
}
