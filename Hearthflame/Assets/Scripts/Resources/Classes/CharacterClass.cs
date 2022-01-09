using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Character Class", menuName = "Character Classes/Character Class")]
public class CharacterClass : Resource
{
	[SerializeField] private int maxLevel;
	[SerializeField] private ExperienceData experienceData;

	public ExperienceData ExperienceData => experienceData; //getter
}
