using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GramophoneUtils.Stats
{
	[Serializable]
	public class LevelSystem
	{
		public event EventHandler OnExperienceChanged;
		public event EventHandler OnLevelChanged;

		private int level;
		private int experience;
		private int experienceToNextLevel;
		private CharacterClass characterClass;

		public LevelSystem(CharacterClass characterClass)
		{
			level = 0;
			experience = 0;
			experienceToNextLevel = 100;
			this.characterClass = characterClass;
		}

		public void AddExperience(int amount)
		{
			experience += amount;
			while (experience >= experienceToNextLevel)
			{
				// Enough experience to level up
				level++;
				experience -= experienceToNextLevel;
				if (OnLevelChanged != null) OnLevelChanged(this, EventArgs.Empty);
			}
			if (OnExperienceChanged != null) OnExperienceChanged(this, EventArgs.Empty);
		}

		public float GetExperienceNormalized()
		{
			return (float)experience / experienceToNextLevel;
		}

		public int GetLevel()
		{
			return level;
		}

		public int GetExperience()
		{
			return experience;
		}

		public int GetExperienceToNextLevel()
		{
			return experienceToNextLevel;
		}
	}
}

