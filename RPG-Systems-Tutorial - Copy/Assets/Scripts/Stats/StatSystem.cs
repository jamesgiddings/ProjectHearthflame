using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GramophoneUtils.Stats
{
	[Serializable]
	public class StatSystem
	{
		private readonly Dictionary<IStatType, Stat> stats = new Dictionary<IStatType, Stat>();

		public Dictionary<IStatType, Stat> Stats => stats;
		public StatSystem() { }
		public StatSystem(BaseStats baseStats)
		{
			foreach (var stat in baseStats.Stats)
			{
				stats.Add(stat.StatType, new Stat(stat.Value));
			}
		}

		public void AddModifier(StatModifier modifier)
		{
			if(!stats.TryGetValue(modifier.StatType, out Stat stat))
			{
				stat = new Stat(modifier.StatType);
				stats.Add(modifier.StatType, stat);
			}
			stat.AddModifier(modifier);
			modifier.OnDurationElapsed += RemoveModifier;
		}

		public Stat GetStat(IStatType type)
		{
			if(!stats.TryGetValue(type, out Stat stat))
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

