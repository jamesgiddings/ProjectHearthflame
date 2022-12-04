using System;
using UnityEngine;
using Sirenix.OdinInspector;

[Serializable]
public class Resource : Data
{
    [VerticalGroup("General/Split/Right")]
    [VerticalGroup("General/Split/Right/Sprites", 30)]
    [PreviewField(60), LabelWidth(50)]
    [SerializeField] protected Sprite sprite;

    public Sprite Sprite => sprite;
}