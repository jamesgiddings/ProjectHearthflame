using GramophoneUtils.Stats;
using System;
using System.Collections.Generic;
using UnityEngine;
using static GramophoneUtils.Stats.LevelSystem;

[CreateAssetMenu(fileName = "New Character Class", menuName = "Character Classes/Character Class")]
public class CharacterClass : Resource
{
	[SerializeField] private int maxLevel;
	[SerializeField] private ExperienceData experienceData;
	[SerializeField] private List<Skill> skillsAvailable;
	[SerializeField] private List<LevelStatEffects> levelStatEffects;

	[Serializable]
	public class LevelStatEffects
	{
		[SerializeField] private List<StatComponentBlueprint> statComponentBlueprints;
		public List<StatComponentBlueprint> StatComponentBlueprint => statComponentBlueprints;
	}

	public ExperienceData ExperienceData => experienceData; //getter
	public List<Skill> SkillsAvailable => skillsAvailable; //getter
	 //getter

	public void LevelUp(System.Object sender, EventArgs args)
	{
		// do levelling up here
		LevelChangedEventArgs castArgs = args as LevelChangedEventArgs;
		Debug.Log("Levelling up to level " + castArgs.Level);
	}
}
