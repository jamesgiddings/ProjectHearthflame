using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using GramophoneUtils.Stats;

public class LevelDisplayUI : MonoBehaviour
{
	[SerializeField] TextMeshProUGUI levelText;
	[SerializeField] Slider experienceSlider;
	[SerializeField] StatSystemBehaviour statSystemBehaviour;
	private LevelSystem levelSystem;

	private void Awake()
	{
		levelSystem = statSystemBehaviour.StatSystem.LevelSystem;
		SetLevelNumber(levelSystem.GetLevel());
		SetExperienceBarSize(levelSystem.GetExperienceNormalized());
		levelSystem.OnExperienceChanged += LevelSystem_OnExperienceChanged;
		levelSystem.OnLevelChanged += LevelSystem_OnLevelChanged;
	}

	public void TestAdd500XP() //remove after testing
	{
		levelSystem.AddExperience(500);
	}

	private void LevelSystem_OnLevelChanged(object sender, System.EventArgs e)
	{
		SetLevelNumber(levelSystem.GetLevel());
	}

	private void LevelSystem_OnExperienceChanged(object sender, System.EventArgs e)
	{
		SetExperienceBarSize(levelSystem.GetExperienceNormalized());
	}

	private void SetExperienceBarSize(float experienceNormalized)
	{
		experienceSlider.value = experienceNormalized;
	}

	private void SetLevelNumber(int levelNumber)
	{
		levelText.text = (levelNumber + 1).ToString(); // the base level in the logic is 0, so the readable level is that +1
	}

	private void OnDestroy()
	{
		levelSystem.OnExperienceChanged -= LevelSystem_OnExperienceChanged;
		levelSystem.OnLevelChanged -= LevelSystem_OnLevelChanged;
	}
}
