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

		public class LevelChangedEventArgs : EventArgs
		{
			public int Level { get; set; }
			public GramophoneUtils.Characters.Character Character { get; set; }
		}

		private readonly GramophoneUtils.Characters.Character character;
		private int level;
		private int experience;
		private LevelSystemAnimated levelSystemAnimated;
		private CharacterClass characterClass;
		private int[] experienceRequirements;
		public int[] ExperienceRequirements => experienceRequirements;

		public LevelSystemAnimated LevelSystemAnimated => levelSystemAnimated; //getter

		public LevelSystem(CharacterClass characterClass, GramophoneUtils.Characters.Character character, int level = 0, int experience = 0)
		{
			this.level = level;
			this.experience = experience;
			this.characterClass = characterClass;
			this.character = character;
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

					LevelChangedEventArgs args = new LevelChangedEventArgs();
					args.Level = level;
					args.Character = character;

					OnLevelChanged?.Invoke(this, args);
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

		public void SetLevel(int level)
		{
			this.level = level;
		}
		
		public void SetExperience(int experience)
		{
			this.experience = experience;
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

