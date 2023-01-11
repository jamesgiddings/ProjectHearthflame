using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IRandomObjectCollection<T>
{
    #region Attributes/Fields/Properties
    #endregion

    #region Constructors
    #endregion

    #region Callbacks
    #endregion

    #region Public Functions

    public List<IRandomObject<T>> GetRandomObjects(int number, List<T> blacklist = null, bool allowDuplicateObjects = true, int maximumAttempts = 100);

    public IRandomObject<T> GetRandomObject(List<T> blacklist = null, List<T> whitelist = null);

    #endregion

    #region Protected Functions
    #endregion

    #region Private Functions
    #endregion

    #region Inner Classes
    #endregion
}
