using UnityEngine;
using Sirenix.OdinInspector;
using System;

public abstract class Data : ScriptableObject
{
    [SerializeField, HideInInspector]
    private string _uid;

    [ShowInInspector, BoxGroup("General")]
    public string UID
    {
        get { 
            if (!string.IsNullOrEmpty(_uid))
            {
                return _uid;
            }
            else
            {
                _uid = Guid.NewGuid().ToString();
#if UNITY_EDITOR
                UnityEditor.EditorUtility.SetDirty(this);
                UnityEditor.AssetDatabase.SaveAssets();
                UnityEditor.AssetDatabase.Refresh();
#endif
                return _uid;
            }
        } 
            
    }
    
    [BoxGroup("General")]
    [SerializeField] private string _name;

    public string Name
    {
        get { return _name; }
        set { _name = value; }
    }
}
