using UnityEditor;
using UnityEngine;


public class Resource : Data
{
    
    [SerializeField] protected Sprite icon;

    public Sprite Icon => icon; //getter

    protected override void OnValidate()
    {
        base.OnValidate();
        ResourceDatabase.AddResource(this);
    }
}
