using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Experience Data", menuName = "StatSystem/Experience Data")]
public class ExperienceData : ScriptableObject
{
	[Tooltip("This shows the experience required for the NEXT level, at a given level. Levels in the game are zero-indexed. Level 0 is displayed in the game as Level 1, but in the code the player begins at 0th Level.")]
	[SerializeField] int[] experienceRequirements;

	public int[] ExperienceRequirements => experienceRequirements; //getter
}
