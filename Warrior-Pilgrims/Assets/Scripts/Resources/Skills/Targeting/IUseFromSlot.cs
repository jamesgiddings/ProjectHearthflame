public interface IUseFromSlot
{
    #region Attributes/Fields/Properties

    bool Slot1 { get; }
    bool Slot2 { get; }
    bool Slot3 { get; }
    bool Slot4 { get; }

    #endregion

    #region Constructors
    #endregion

    #region Callbacks
    #endregion

    #region Public Functions

    bool CanUseFromSlot(ICharacter character);

    #endregion

    #region Protected Functions
    #endregion

    #region Private Functions
    #endregion

    #region Inner Classes
    #endregion
}
