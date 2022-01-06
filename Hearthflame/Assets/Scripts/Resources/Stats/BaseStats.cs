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
        [SerializeField] private new string name;
        [SerializeField] private CharacterClass characterClass;
        [SerializeField] private int currentHealth;
        [SerializeField] private int maxHealth;
        [SerializeField] private int experience;

        public List<BaseStat> Stats => stats; //getter
        public string Name => name; //getter
        public CharacterClass CharacterClass => characterClass; //getter
        public int CurrentHealth => currentHealth; //getter
        public int MaxHealth => maxHealth; //getter
        public int Experience => experience; //getter


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

