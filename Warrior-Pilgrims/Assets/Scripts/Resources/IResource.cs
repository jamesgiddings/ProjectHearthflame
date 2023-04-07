using UnityEngine;

public interface IResource
{
    #region Attributes/Fields/Properties
    #endregion

    #region Constructors
    #endregion

    #region Callbacks
    #endregion

    #region Public Functions

    public string Name { get; }
    public string UID { get; }
    public Sprite Sprite { get; }

    public abstract string GetInfoDisplayText();

    #endregion

    #region Protected Functions
    #endregion

    #region Private Functions
    #endregion

    #region Inner Classes
    #endregion
}
