using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Data : ScriptableObject
{
    [ReadOnly] public string UID;

    [SerializeField] protected new string name;

    public string Name => name;
        protected virtual void OnValidate()
    {
#if UNITY_EDITOR
        if (UID == "")
        {
            UID = GUID.Generate().ToString();

            UnityEditor.EditorUtility.SetDirty(this);
        }
#endif
    }
}
