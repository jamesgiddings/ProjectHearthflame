using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInstantiable<T>
{
    T Instance(T instantiable);
}