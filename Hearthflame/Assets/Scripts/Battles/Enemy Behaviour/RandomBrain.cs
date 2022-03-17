using GramophoneUtils.Stats;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Random Brain", menuName = "Brains/New Random Brain")]
public class RandomBrain : Brain
{
	[SerializeField] private List<RandomObject<Skill>> randomSkills;
	[SerializeField] private List<RandomObject<CharacterClass>> randomTargets;

	private RandomObjectCollection<Skill> randomSkillCollection;
	private RandomObjectCollection<Skill> RandomSkillCollection 
	{ 
		get
		{
			if (randomSkillCollection != null)
			{
				return randomSkillCollection;
			}
			if (randomSkills.Count == 0)
			{
				foreach (Skill skill in characterClass.SkillsAvailable)
				{
					randomSkills.Add(new RandomObject<Skill>(skill, 1f));
				}
			}
			randomSkillCollection = new RandomObjectCollection<Skill>(randomSkills);
			return randomSkillCollection;
		}
	}

	private RandomObjectCollection<CharacterClass> randomCharacterClassCollection;

	private RandomObjectCollection<CharacterClass> RandomCharacterClassCollection
	{
		get
		{
			if (randomCharacterClassCollection != null)
			{
				return randomCharacterClassCollection;
			}
			if (randomTargets.Count == 0)
			{
				foreach (CharacterClass characterClass in resourceDatabase.CharacterClasses)
				{
					randomTargets.Add(new RandomObject<CharacterClass>(characterClass, 1f));
				}
			}
			randomCharacterClassCollection = new RandomObjectCollection<CharacterClass>(randomTargets);
			return randomCharacterClassCollection;
		}
	}

#if UNITY_EDITOR
	private void OnValidate()
	{
		if (characterClass != null)
		{
			if (randomSkills.Count == 0)
			{
				foreach (Skill skill in characterClass.SkillsAvailable)
				{
					randomSkills.Add(new RandomObject<Skill>(skill, 1f));
				}
			}

			randomSkillCollection = new RandomObjectCollection<Skill>(randomSkills);
		}

		if (resourceDatabase != null)
		{
			if (randomTargets.Count == 0)
			{
				foreach (CharacterClass characterClass in resourceDatabase.CharacterClasses)
				{
					randomTargets.Add(new RandomObject<CharacterClass>(characterClass, 1f));
				}
			}
			randomCharacterClassCollection = new RandomObjectCollection<CharacterClass>(randomTargets);
		}
	}

	public void ResetSkills()
	{
		randomSkills = new List<RandomObject<Skill>>();

		foreach (Skill skill in characterClass.SkillsAvailable)
		{
			randomSkills.Add(new RandomObject<Skill>(skill, 1f));
		}

		randomSkillCollection = new RandomObjectCollection<Skill>(randomSkills);
	}

	public void ResetTargets()
	{
		randomTargets = new List<RandomObject<CharacterClass>>();

		foreach (CharacterClass characterClass in resourceDatabase.CharacterClasses)
		{
			randomTargets.Add(new RandomObject<CharacterClass>(characterClass, 1f));
		}

		randomCharacterClassCollection = new RandomObjectCollection<CharacterClass>(randomTargets);
	}
#endif

	public override Skill ChooseSkill(Character currentActor)
	{
		Debug.Log("randomSkills.Count: " + randomSkills.Count);
		Debug.Log("RandomSkillCollection.RandomObjects.Count: " + RandomSkillCollection.RandomObjects.Count);
		RandomObject<Skill> randomObject = RandomSkillCollection.GetRandomObject(currentActor.SkillSystem.LockedSkillsList);
		//Debug.Log(currentActor.SkillSystem.LockedSkillsList[0].name);
		return (Skill)randomObject.randomObject;
	}

	public override List<Character> ChooseTargets(List<Character> availableTargets, Skill skill)
	{
		List<Character> targets = new List<Character>();

		List<RandomObject<Character>> randomTargets = new List<RandomObject<Character>>();

		foreach (Character character in availableTargets)
		{
			randomTargets.Add(new RandomObject<Character>(character, RandomCharacterClassCollection.GetWeighting(character.CharacterClass)));
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


