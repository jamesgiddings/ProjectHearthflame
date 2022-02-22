using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace GramophoneUtils.Stats
{
   [CreateAssetMenu(fileName = "New Character Template", menuName = "Characters/Character Template")]
    public class CharacterTemplate : Data
    {
        private Dictionary<string, IStatType> statTypeStringRefDictionary;

        [SerializeField] private StatTemplate stats;
        [SerializeField] private Sprite icon;
        [SerializeField] private CharacterClass characterClass;
        [SerializeField] private int currentHealth;
        [SerializeField] private int maxHealth;
        [SerializeField] private bool isPlayer;
        [SerializeField] private Color color;

        public StatTemplate Stats => stats; //getter
        public Sprite Icon => icon; //getter
        public CharacterClass CharacterClass => characterClass; //getter
        public int CurrentHealth => currentHealth; //getter
        public int MaxHealth => maxHealth; //getter
        public bool IsPlayer => isPlayer; //getter
        public Color Color => color; //getter

        public Dictionary<string, IStatType> StatTypeStringRefDictionary
        {
            get
            {
                if (statTypeStringRefDictionary != null) { return statTypeStringRefDictionary; }
                statTypeStringRefDictionary = new Dictionary<string, IStatType>();
                foreach (BaseStat stat in stats.Stats)
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

