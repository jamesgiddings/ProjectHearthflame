using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Sirenix.OdinInspector;

public abstract class Data : ScriptableObject
{
    [HideInInspector]
    [ScriptableObjectId]
    private string _uid;

    public string UID => _uid;
    
    [BoxGroup("General")]
    [HorizontalGroup("General/Split", 60)]
    [VerticalGroup("General/Split/Left"), HideLabel]
    [SerializeField] private string _name;

    public string Name => _name;
}
