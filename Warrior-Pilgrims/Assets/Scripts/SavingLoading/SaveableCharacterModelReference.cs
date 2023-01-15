using AYellowpaper;
using GramophoneUtils.SavingLoading;
using UnityEngine;

public class SaveableCharacterModelReference : MonoBehaviour, ISaveable
{
    #region Attributes/Fields/Properties

    [SerializeField] private InterfaceReference<ISaveable> _saveableObject;

    #endregion

    #region Constructors
    #endregion

    #region Callbacks
    #endregion

    #region Public Functions

    #region Saving/Loading

    public object CaptureState()
    {
        return _saveableObject.Value.CaptureState();
    }

    public void RestoreState(object state)
    {
        _saveableObject.Value.RestoreState(state);
    }

    #endregion

    #endregion

    #region Protected Functions
    #endregion

    #region Private Functions
    #endregion

    #region Inner Classes
    #endregion
}
