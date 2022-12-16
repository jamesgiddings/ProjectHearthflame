using GramophoneUtils.Stats;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GramophoneUtils.Battles
{
    [CreateAssetMenu(fileName = "Check", menuName = "Battles/Check")]
    public class Check : ScriptableObject
    {
        [SerializeField] private StatType _statType;
        [SerializeField] private int _difficulty;
        [SerializeField] private string _displayName;

        public StatType StatType => _statType;
        public int Difficulty => _difficulty;
        public string DisplayName => _displayName;
    }

    public class CheckInstance
    {
        private StatType _statType;
        private int _difficulty;
        private string _displayName;
        private int _value;

        public Action<CheckInstance> OnCheckInstanceChanged;

        public StatType StatType => _statType;
        public int Difficulty => _difficulty;
        public string DisplayName => _displayName;
        public int Value => _value;

        public CheckInstance(Check check)
        {
            _statType = check.StatType;
            _difficulty = check.Difficulty;
            _displayName = check.DisplayName;
            _value = 0;
        }

        public float GetProgress()
        {
            return Math.Min(((float)_value / (float)_difficulty) , 1);
        }

        public bool IsCompleted()
        {
            return _value >= _difficulty;
        }

        public void MakeCheck(Character character)
        {
            _value += (int)character.StatSystem.GetStatValue(_statType);
            Debug.Log(character.Name);
            Debug.Log("(int)character.StatSystem.GetStatValue(_statType); " + (int)character.StatSystem.GetStatValue(_statType));
            Debug.Log(_statType.name);
            Debug.Log(_value);
            OnCheckInstanceChanged?.Invoke(this);
        }
    }
}


