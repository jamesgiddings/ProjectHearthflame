using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Data : ScriptableObject
{
    [ScriptableObjectId]
    public string UID;

    [SerializeField] protected new string name;

    public string Name => name;
}
