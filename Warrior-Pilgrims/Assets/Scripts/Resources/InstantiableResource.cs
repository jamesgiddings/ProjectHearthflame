using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// This is an alternative class to inherit from where the intention of the resource is as 
/// a blueprint, with the game using instances of it, not the object itself. It is a generic
/// so the class must declare what type of resource it is in the declaration.
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class InstantiableResource<T> : Resource, IInstantiable<T>
{
    #region API
    public abstract T Instance();
    #endregion
}
