using System;
using UnityEngine;
using Sirenix.OdinInspector;

[Serializable]
public abstract class Resource : Data
{
    [VerticalGroup("General/Split/Right")]
    [VerticalGroup("General/Split/Right/Sprites", 30)]
    [PreviewField(60), LabelWidth(50)]
    [SerializeField] protected Sprite _sprite;

    public Sprite Sprite => _sprite;

}