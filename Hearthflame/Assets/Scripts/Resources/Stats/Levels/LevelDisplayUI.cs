using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using GramophoneUtils.Stats;

public class LevelDisplayUI : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI levelText;
	[SerializeField] private Slider experienceSlider;
	[SerializeField] private Character character;

	private LevelSystem levelSystem;
	private LevelSystemAnimated levelSystemAnimated;

	private void Awake()
	{
		levelSystem = character.LevelSystem;
		levelSystemAnimated = character.LevelSystem.LevelSystemAnimated;
		SetLevelNumber(levelSystemAnimated.GetLevel());
		SetExperienceBarSize(levelSystemAnimated.GetExperienceNormalized());
		levelSystemAnimated.OnExperienceChanged += LevelSystemAnimated_OnExperienceChanged;
		levelSystemAnimated.OnLevelChanged += LevelSystemAnimated_OnLevelChanged;
	}

	public void TestAdd500XP() //remove after testing
	{
	levelSystem.AddExperience(50000);
	}

	private void LevelSystemAnimated_OnLevelChanged(object sender, System.EventArgs e)
	{
		SetLevelNumber(levelSystemAnimated.GetLevel());
	}

	private void LevelSystemAnimated_OnExperienceChanged(object sender, System.EventArgs e)
	{
		SetExperienceBarSize(levelSystemAnimated.GetExperienceNormalized());
	}

	private void SetExperienceBarSize(float experienceNormalized)
	{
		experienceSlider.value = experienceNormalized;
	}

	private void SetLevelNumber(int levelNumber)
	{
		levelText.text = (levelNumber + 1).ToString() + " " + character.CharacterClass.Name; // the base level in the logic is 0, so the readable level is that +1
	}

	private void OnDestroy()
	{
		levelSystemAnimated.OnExperienceChanged -= LevelSystemAnimated_OnExperienceChanged;
		levelSystemAnimated.OnLevelChanged -= LevelSystemAnimated_OnLevelChanged;
	}

	public void SetCharacter(Character character)
	{
		this.character = character;
	}

}
