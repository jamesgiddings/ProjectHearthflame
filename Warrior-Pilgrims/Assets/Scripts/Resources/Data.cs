using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Sirenix.OdinInspector;

public abstract class Data : ScriptableObject
{
    [HideInInspector]
    [ScriptableObjectId]
    public string UID;
    
    [BoxGroup("General")]
    [HorizontalGroup("General/Split", 60)]
    [VerticalGroup("General/Split/Left"), HideLabel]
    [SerializeField] protected new string name;

    public string Name => name;
}
