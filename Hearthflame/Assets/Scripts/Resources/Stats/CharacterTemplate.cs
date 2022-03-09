using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace GramophoneUtils.Stats
{
   [CreateAssetMenu(fileName = "New Character Template", menuName = "Characters/Character Template")]
    public class CharacterTemplate : Resource
    {
        [SerializeField] private StatTemplate stats;
        [SerializeField] private CharacterClass characterClass;
        [SerializeField] private int currentHealth;
        [SerializeField] private int maxHealth;
        [SerializeField] private Color color;
        [SerializeField] private Brain brain;
        [SerializeField] private bool startsUnlocked;

        public StatTemplate Stats => stats; //getter
        public CharacterClass CharacterClass => characterClass; //getter
        public int CurrentHealth => currentHealth; //getter
        public int MaxHealth => maxHealth; //getter
        public Color Color => color; //getter
        public Brain Brain => brain; // getter
        public bool StartsUnlocked => startsUnlocked; // getter

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

