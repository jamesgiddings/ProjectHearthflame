using GramophoneUtils.Stats;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "New Random Brain", menuName = "Brains/New Random Brain")]
public class RandomBrain : Brain
{
	[SerializeField] private List<RandomObject<Skill>> randomSkills = null;
	[SerializeField] private List<RandomObject<CharacterClass>> randomTargets = null;

	private RandomObjectCollection<Skill> randomSkillCollection;
	private RandomObjectCollection<CharacterClass> randomCharacterClassCollection;

	private ResourceDatabase resourceDatabase => (ResourceDatabase)AssetDatabase.LoadAssetAtPath("Assets / Scripts / Resources / Resource Database.asset", typeof(ResourceDatabase));

#if UNITY_EDITOR
	private void OnValidate()
	{
		if (randomSkills.Count == 0)
		{
			foreach (Skill skill in characterClass.SkillsAvailable)
			{
				randomSkills.Add(new RandomObject<Skill>(skill, 1f));
			}
		}
		randomSkillCollection = new RandomObjectCollection<Skill>(randomSkills);

		if (randomTargets.Count == 0)
		{
			foreach (CharacterClass characterClass in resourceDatabase.CharacterClasses)
			{
				randomTargets.Add(new RandomObject<CharacterClass>(characterClass, 1f));
			}
		}
		randomCharacterClassCollection = new RandomObjectCollection<CharacterClass>(randomTargets);
	}
#endif

	public override Skill ChooseSkill()
	{
		RandomObject<Skill> randomObject = randomSkillCollection.GetRandomObject();
		Debug.Log("randomSkillCollection == null: " + randomSkillCollection == null);
		Debug.Log("randomSkillCollection.RandomObjects.Count: " + randomSkillCollection.RandomObjects.Count);
		Debug.Log(randomObject);
		return (Skill)randomObject.randomObject;
	}

	public override List<Character> ChooseTargets(List<Character> availableTargets, Skill skill)
	{
		List<Character> targets = new List<Character>();

		List<RandomObject<Character>> randomTargets = new List<RandomObject<Character>>();

		foreach (Character character in availableTargets)
		{
			randomTargets.Add(new RandomObject<Character>(character, randomCharacterClassCollection.GetWeighting(character.CharacterClass)));
		}

		RandomObjectCollection <Character> randomTargetCollection = new RandomObjectCollection<Character>(randomTargets);

		if (skill.TargetNumberFlag.HasFlag(TargetNumberFlag.Single))
		{
			targets.Add(randomTargetCollection.GetRandomObject().randomObject);
			return targets;
		}
		return null;
	}
}


