using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InstantiableResource<T> : Resource, IInstantiable<T>
{
    #region API
    public abstract T Instance(T instantiable);
    #endregion
}
