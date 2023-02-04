using System;

public interface IElapsible : IApplyable
{
    #region Attributes/Fields/Properties

    public int Duration { get; }

    Action<IElapsible> OnDurationElapsed { get; set; }

    #endregion

    #region Constructors
    #endregion

    #region Callbacks
    #endregion

    #region Public Functions

    public void IncrementDuration(int value = -1);

    public void ElapseDuration();

    #endregion

    #region Protected Functions
    #endregion

    #region Private Functions
    #endregion

    #region Inner Classes
    #endregion
}
