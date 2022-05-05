using System;
using UnityEngine;

[Serializable]
public class Resource : Data
{
    
    [SerializeField] protected Sprite sprite;

    public Sprite Sprite => sprite; //getter
}