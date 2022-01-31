using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace GramophoneUtils.Stats
{
   [CreateAssetMenu(fileName = "New Base Stats", menuName = "Characters/Character Template")]
    public class CharacterTemplate : ScriptableObject
    {
        private Dictionary<string, IStatType> statTypeStringRefDictionary;

        [SerializeField] private List<BaseStat> stats = new List<BaseStat>();
        [SerializeField] private Sprite icon;
        [SerializeField] private new string name;
        [SerializeField] private CharacterClass characterClass;
        [SerializeField] private int currentHealth;
        [SerializeField] private int maxHealth;
        [SerializeField] private bool isPlayer;

        public List<BaseStat> Stats => stats; //getter
        public Sprite Icon => icon; //getter
        public string Name => name; //getter
        public CharacterClass CharacterClass => characterClass; //getter
        public int CurrentHealth => currentHealth; //getter
        public int MaxHealth => maxHealth; //getter
        public bool IsPlayer => isPlayer; //getter


        public Dictionary<string, IStatType> StatTypeStringRefDictionary
        {
            get
            {
                if (statTypeStringRefDictionary != null) { return statTypeStringRefDictionary; }
                statTypeStringRefDictionary = new Dictionary<string, IStatType>();
                foreach (BaseStat stat in stats)
                {
                    statTypeStringRefDictionary.Add(stat.StatType.Name, stat.StatType);
                }
                return statTypeStringRefDictionary;
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

