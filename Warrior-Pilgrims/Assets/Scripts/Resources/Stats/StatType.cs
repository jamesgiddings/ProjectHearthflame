using UnityEngine;

namespace GramophoneUtils.Stats
{
    [CreateAssetMenu(fileName = "New Stat Type", menuName = "Stat System/Stat Type")]
    public class StatType : ScriptableObject, IStatType
    {
        [SerializeField] private string _name = "New Stat Type Name";
        [SerializeField] private float _defaultValue = 0f;
        [SerializeField] private float _tuningMultiplier = 1f;

        public string Name => _name;

        public float DefaultValue => _defaultValue;

        public float TuningMultiplier => _tuningMultiplier;
    }
}