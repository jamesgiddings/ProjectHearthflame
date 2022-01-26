using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace GramophoneUtils.Stats
{
   [CreateAssetMenu(fileName = "New Base Stats", menuName = "Characters/Character Template")]
    public class CharacterTemplate : ScriptableObject
    {
        [SerializeField] private List<BaseStat> stats = new List<BaseStat>();
        [SerializeField] private new string name;
        [SerializeField] private CharacterClass characterClass;
        [SerializeField] private int currentHealth;
        [SerializeField] private int maxHealth;

        public List<BaseStat> Stats => stats; //getter
        public string Name => name; //getter
        public CharacterClass CharacterClass => characterClass; //getter
        public int CurrentHealth => currentHealth; //getter
        public int MaxHealth => maxHealth; //getter

        public Dictionary<string, IStatType> StatTypeStringRefDictionary;

		private void OnEnable()
		{
            StatTypeStringRefDictionary = new Dictionary<string, IStatType>();
            foreach (BaseStat stat in stats)
			{
                StatTypeStringRefDictionary.Add(stat.StatType.Name, stat.StatType);
			}
		}

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

