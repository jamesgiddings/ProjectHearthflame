using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GramophoneUtils.Stats;
using System;
using CodeMonkey.Utils;

public class LevelSystemAnimated 
{
	private LevelSystem levelSystem;
	private bool isAnimating;

	private int level;
	private int experience;
	private int experienceToNextLevel;

	public LevelSystemAnimated(LevelSystem levelSystem)
	{
		SetLevelSystem(levelSystem);
		FunctionUpdater.Create(() => Update()); //this is broken
	}

	private void SetLevelSystem(LevelSystem levelSystem)
	{
		this.levelSystem = levelSystem;
		levelSystem.OnExperienceChanged += LevelSystem_OnExperienceChanged;
		levelSystem.OnLevelChanged += LevelSystem_OnLevelChanged;

		level = levelSystem.GetLevel();
		experience = levelSystem.GetExperience();
		experienceToNextLevel = levelSystem.GetExperienceToNextLevel();
	}

	private void LevelSystem_OnLevelChanged(object sender, EventArgs e)
	{
		isAnimating = true;
	}

	private void LevelSystem_OnExperienceChanged(object sender, EventArgs e)
	{
		isAnimating = true;
	}

	private void Update()
	{
		if (isAnimating)
		{
			if(level < levelSystem.GetLevel())
			{
				// Local level under target level
				AddExperience();
			}
		} 
		else
		{
			// Local level equals the target level
			if (experience < levelSystem.GetExperience())
			{
				AddExperience();
			}
			else
			{
				isAnimating = false;
			}
		}
	}

	private void AddExperience()
	{
		experience++; //= (int)Mathf.Clamp(((float)experienceToNextLevel / 100), 1, addXPamount); use something like this if xp amounts get v large Oh and because with this the XP amount of the animation does not always end with the exact same value as the actual XP, so when the animation ends, again with animationxp=currentxp
		if (experience >= experienceToNextLevel)
		{
			level++;
		}
	}
}
