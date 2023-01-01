using GramophoneUtils.Stats;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;
using static GramophoneUtils.Stats.LevelSystem;

[Serializable]
[CreateAssetMenu(fileName = "New Character Class", menuName = "Character Classes/Character Class")]
public class CharacterClass : Resource
{
    [VerticalGroup("General/Split/Left")]
    [TextArea(3, 5)]
    [SerializeField] private string description;

    [ProgressBar(0, 300)]
    [SerializeField] private int baseMaxHealth;
    [ProgressBar(0, 300)]
    [SerializeField] private int baseHealth;
    [ProgressBar(1, 40)]
    [SerializeField] private int maxLevel;
	[SerializeField] private ExperienceData experienceData;
	[SerializeField] private List<Skill> skillsAvailable;
    [FoldoutGroup("Level Progression")]
    [TableList(IsReadOnly = true, AlwaysExpanded = true)]
	[SerializeField] private List<LevelStatEffect> levelStatEffects;
	[SerializeField] private int size = 1;

	[Serializable]
	public class LevelStatEffect
	{
        [TableList(AlwaysExpanded = true)]
        [SerializeField] private List<Skill> skillsUnlocked; 
		[SerializeField] private int baseHealthIncrement;
        [TableList(AlwaysExpanded = true)]
		[SerializeField] private List<StatModifierBlueprint> statComponentBlueprints;
		
		public List<StatModifierBlueprint> StatComponentBlueprints => statComponentBlueprints;
		public int BaseHealthIncrement => baseHealthIncrement;
	}

	public ExperienceData ExperienceData => experienceData; //getter
	public List<Skill> SkillsAvailable => skillsAvailable; //getter

	public int BaseHealth => baseHealth;
	public int BaseMaxHealth => baseMaxHealth;
	public int Size => size;

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
