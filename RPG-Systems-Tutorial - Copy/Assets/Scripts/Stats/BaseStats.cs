using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace GramophoneUtils.Stats
{
   [CreateAssetMenu(fileName = "New Base Stats", menuName = "StatSystem/Base Stats")]
    public class BaseStats : ScriptableObject
    {
        [SerializeField] private List<BaseStat> stats = new List<BaseStat>();

        public List<BaseStat> Stats => stats; //getter

        [Serializable]
        public class BaseStat
		{
            [SerializeField] private StatType statType = null;
            [SerializeField] private float value = 0f;

            public StatType StatType => statType;

            public float Value => value;
        }
    }
}

