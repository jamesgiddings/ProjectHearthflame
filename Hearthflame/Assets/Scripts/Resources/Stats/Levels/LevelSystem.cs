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
		private LevelSystemAnimated levelSystemAnimated;
		private CharacterClass characterClass;
		private int[] experienceRequirements;
		public int[] ExperienceRequirements => experienceRequirements;

		public LevelSystemAnimated LevelSystemAnimated => levelSystemAnimated; //getter

		public LevelSystem(CharacterClass characterClass)
		{
			level = 0;
			experience = 0;
			this.characterClass = characterClass;
			levelSystemAnimated = new LevelSystemAnimated(this);
			experienceRequirements = characterClass.ExperienceData.ExperienceRequirements;
		}



		public void AddExperience(int amount)
		{
			if (!IsMaxLevel())
			{
				experience += amount;
				while (!IsMaxLevel() && experience >= GetExperienceToNextLevel(level))
				{
					// Enough experience to level up
					
					experience -= GetExperienceToNextLevel(level);
					level++;

					if (OnLevelChanged != null) OnLevelChanged(this, EventArgs.Empty);
				}
				if (OnExperienceChanged != null) OnExperienceChanged(this, EventArgs.Empty);
			}
		}

		public float GetExperienceNormalized()
		{
			if (IsMaxLevel())
			{
				return 1f;
			}
			else
			{
				return (float)experience / GetExperienceToNextLevel(level);
			}
		}

		public int GetLevel()
		{
			return level;
		}

		public int GetExperience()
		{
			return experience;
		}

		public int GetExperienceToNextLevel(int level)
		{
			if (level < experienceRequirements.Length)
			{
				return experienceRequirements[level];
			} 
			else
			{
				// Level Invalid
				Debug.LogError("Level invalid: " + level + " (outside range of ExperienceData.ExperienceRequirements array).");
				return 100;
			}
		}

		public bool IsMaxLevel()
		{
			return IsMaxLevel(level);
		}

		public bool IsMaxLevel(int level)
		{
			return level == experienceRequirements.Length - 1;
		}
	}
}

