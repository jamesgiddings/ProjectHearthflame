using GramophoneUtils.Stats;
using System;
using System.Collections.Generic;
using UnityEngine;
using static GramophoneUtils.Stats.LevelSystem;

[Serializable]
[CreateAssetMenu(fileName = "New Character Class", menuName = "Character Classes/Character Class")]
public class CharacterClass : Resource
{
	[SerializeField] private int baseHealth;
	[SerializeField] private int baseMaxHealth;
	[SerializeField] private int maxLevel;
	[SerializeField] private ExperienceData experienceData;
	[SerializeField] private List<Skill> skillsAvailable;
	[SerializeField] private List<LevelStatEffect> levelStatEffects;

	[Serializable]
	public class LevelStatEffect
	{
		[SerializeField] private List<Skill> skillsUnlocked; 
		[SerializeField] private int baseHealthIncrement;
		[SerializeField] private List<StatModifierBlueprint> statComponentBlueprints;
		
		public List<StatModifierBlueprint> StatComponentBlueprints => statComponentBlueprints;
		public int BaseHealthIncrement => baseHealthIncrement;
	}

	public ExperienceData ExperienceData => experienceData; //getter
	public List<Skill> SkillsAvailable => skillsAvailable; //getter

	public int BaseHealth => baseHealth;
	public int BaseMaxHealth => baseMaxHealth;

	public void LevelUp(System.Object sender, EventArgs args)
	{
		// do levelling up here
		LevelChangedEventArgs castArgs = args as LevelChangedEventArgs;
		IncrementStatBaseValues(castArgs);
		IncrementHealthBaseValues(castArgs);
	}

	private void IncrementHealthBaseValues(LevelChangedEventArgs castArgs)
	{
		castArgs.Character.HealthSystem.IncrementMaxHealth(levelStatEffects[castArgs.Level].BaseHealthIncrement);
		castArgs.Character.HealthSystem.IncrementCurrentHealth(levelStatEffects[castArgs.Level].BaseHealthIncrement);
	}

	private void IncrementStatBaseValues(LevelChangedEventArgs castArgs)
	{
		foreach (StatModifierBlueprint statComponentBlueprint in levelStatEffects[castArgs.Level].StatComponentBlueprints)
		{
			castArgs.Character.StatSystem.IncrementStatBaseValue(statComponentBlueprint);
		}
	}
}
