using System;
using CodeMonkey.Utils;
using GramophoneUtils.Stats;
using GramophoneUtils;
using UnityEngine;

public class LevelSystemAnimated 
{
	public event EventHandler OnExperienceChanged;
	public event EventHandler OnLevelChanged;

	private LevelSystem levelSystem;
	private bool isAnimating;
	private float updateTimer;
	private float updateTimerMax;

	private int level;
	private int experience;
	private int expMinIncrement = 1;
	private int expMaxIncrement = 1000000;

	public LevelSystemAnimated(LevelSystem levelSystem)
	{
		SetLevelSystem(levelSystem);
		updateTimerMax = .016f;
		FunctionUpdater.Create(() => Update()); //TODO this is messy
	}

	private void SetLevelSystem(LevelSystem levelSystem)
	{
		this.levelSystem = levelSystem;
		levelSystem.OnExperienceChanged += LevelSystem_OnExperienceChanged;
		levelSystem.OnLevelChanged += LevelSystem_OnLevelChanged;

		level = levelSystem.GetLevel();
		experience = levelSystem.GetExperience();
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
			// Check if its time to update
			updateTimer += Time.deltaTime;
			while (updateTimer > updateTimerMax)
			{
				// Time to update
				updateTimer -= updateTimerMax;
				UpdateAddExperience();
			}
		}
	}

	private void UpdateAddExperience()
	{
		if (level < levelSystem.GetLevel())
		{
			// Local level under target level
			AddExperience();
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

	public void AddExperience()
	{
		experience += (int)Mathf.Clamp(((float)levelSystem.GetExperienceToNextLevel(level) / 50), expMinIncrement, expMaxIncrement);
		if (experience >= levelSystem.GetExperienceToNextLevel(level))
		{
			// Enough experience to level up
			level++; 

			experience = 0;
			if (OnLevelChanged != null) OnLevelChanged(this, EventArgs.Empty);
		}
		if (OnExperienceChanged != null) OnExperienceChanged(this, EventArgs.Empty);
	}

	public int GetLevel()
	{
		return level;
	}

	public float GetExperienceNormalized()
	{
		if (levelSystem.IsMaxLevel(level))
		{
			return 1f;
		}
		else if (levelSystem.GetExperienceToNextLevel(level) == 0) // to avoid divide by 0 errors
		{
			return 0f;
		}
		else
		{
			return (float)experience / levelSystem.GetExperienceToNextLevel(level);
		}
		
	}

	public void UpdateLevelSystemAnimated() // call this after loading, to update display without animating
	{
		level = levelSystem.GetLevel();
		experience = levelSystem.GetExperience();
		if (OnLevelChanged != null) OnLevelChanged(this, EventArgs.Empty);
		if (OnExperienceChanged != null) OnExperienceChanged(this, EventArgs.Empty);
}

}
