using System;
using UnityEngine;
using Sirenix.OdinInspector;

[Serializable]
public class Resource : Data, IResource
{
    [BoxGroup("General")]
    [PreviewField(60), LabelWidth(50)]
    [SerializeField] protected Sprite _sprite;

    public Sprite Sprite => _sprite;

    public virtual string GetInfoDisplayText()
    {
        return Name;
    }

}