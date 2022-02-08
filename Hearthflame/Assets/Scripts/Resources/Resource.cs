using UnityEditor;
using UnityEngine;

public class Resource : Data
{
    
    [SerializeField] protected Sprite icon;

    public Sprite Icon => icon; //getter

    //protected override void OnValidate()
    //{
    //    base.OnValidate();
    //    ResourceDatabase.AddResource(this);
    //}

    //protected virtual void OnValidate()
    //{
    //    ResourceDatabase.AddResource(this);
    //    Debug.Log("Adding resource");
    //}

    //[RuntimeInitializeOnLoadMethod]
    //protected virtual void RegisterWithDatabase()
    //{
    //    Debug.Log("Adding resource");
    //    ResourceDatabase.AddResource(this);
    //}
}
