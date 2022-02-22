using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GramophoneUtils.Stats
{
    public class StatSystem
    {
        private readonly Dictionary<IStatType, Stat> stats = new Dictionary<IStatType, Stat>();
		public Dictionary<IStatType, Stat> Stats => stats; //getter
		public StatSystem() { } //constructor 1
		public StatSystem(CharacterTemplate template) //constructor 2
		{
			foreach (var stat in template.Stats.Stats)
			{
				stats.Add(stat.StatType, new Stat(stat.Value));
			}
		}

		public void AddModifier(StatModifier modifier)
		{
			if (!stats.TryGetValue(modifier.StatType, out Stat stat))
			{
				stat = new Stat(modifier.StatType);
				stats.Add(modifier.StatType, stat);
			}
			stat.AddModifier(modifier);
			modifier.OnDurationElapsed += RemoveModifier;
		}

		public void UpdateStatBaseValue(StatModifierBlueprint statComponentBlueprint)
		{
			if (!stats.TryGetValue(statComponentBlueprint.StatType, out Stat stat))
			{
				stat = new Stat(statComponentBlueprint.StatType);
				stats.Add(statComponentBlueprint.StatType, stat);
			}
			stat.UpdateBaseValue(statComponentBlueprint.Value);
		}		
		
		public void IncrementStatBaseValue(StatModifierBlueprint statComponentBlueprint)
		{
			if (!stats.TryGetValue(statComponentBlueprint.StatType, out Stat stat))
			{
				stat = new Stat(statComponentBlueprint.StatType);
				stats.Add(statComponentBlueprint.StatType, stat);
			}
			stat.IncrementBaseValue(statComponentBlueprint.Value);
		}

		public Stat GetStat(IStatType type)
		{
			if (!stats.TryGetValue(type, out Stat stat))
			{
				stat = new Stat(type);
				stats.Add(type, stat);
			}
			return stat;
		}

		public float GetStatValue(IStatType type)
		{
			if (!stats.TryGetValue(type, out Stat stat))
			{
				stat = new Stat(type);
				stats.Add(type, stat);
			}

			return stat.Value;
		}

		public float GetBaseStatValue(IStatType type)
		{
			if (!stats.TryGetValue(type, out Stat stat))
			{
				stat = new Stat(type);
				stats.Add(type, stat);
			}

			return stat.GetBaseValue();
		}

		public void RemoveModifier(StatModifier modifier)
		{
			if (!stats.TryGetValue(modifier.StatType, out Stat stat))
			{
				return;
			}
			stat.RemoveModifier(modifier);
			modifier.OnDurationElapsed -= RemoveModifier;
		}
	}
}
