using GramophoneUtils.Stats;
using System;
using UnityEngine;

[Serializable]
public class BaseStat
{
    [SerializeField] private StatType statType = null;
    [SerializeField] private float value = 0f;

    public StatType StatType => statType;

    public float Value => value;
}