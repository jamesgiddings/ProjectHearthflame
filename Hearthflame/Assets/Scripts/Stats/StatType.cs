using UnityEngine;

namespace GramophoneUtils.Stats
{
    [CreateAssetMenu(fileName = "New Stat Type", menuName = "StatSystem/Stat Type")]
    public class StatType : ScriptableObject, IStatType
    {
        [SerializeField] private new string name = "New Stat Type Name";
        [SerializeField] private float defaultValue = 0f;

        public string Name => name;

        public float DefaultValue => defaultValue;
    }
}