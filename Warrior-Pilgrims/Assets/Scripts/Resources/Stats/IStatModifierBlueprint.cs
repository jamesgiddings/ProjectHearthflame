using GramophoneUtils.Stats;

public interface IStatModifierBlueprint
{
    #region Attributes/Fields/Properties
    #endregion

    #region Constructors
    #endregion

    #region Callbacks
    #endregion

    #region Public Functions

    public IStatType StatType { get; }

    public ModifierNumericType ModifierNumericType { get; }

    public StatModifierType StatModifierType { get; }

    public float Value { get; }

    public int Duration { get; }

    #endregion

    #region Protected Functions
    #endregion

    #region Private Functions
    #endregion

    #region Inner Classes
    #endregion
}
