using GramophoneUtils.Characters;
using GramophoneUtils.Stats;
using System.Collections.Generic;
using UnityEngine;


public class RandomBrainDeprecated : Brain
{

	[SerializeField] private List<RandomObject<ISkill>> randomSkills;
	[SerializeField] private List<RandomObject<CharacterClass>> randomTargets;

	private RandomObjectCollection<ISkill> randomSkillCollection;
	private RandomObjectCollection<ISkill> RandomSkillCollection 
	{ 
		get
		{
			if (randomSkillCollection != null)
			{
				return randomSkillCollection;
			}
			if (randomSkills.Count == 0)
			{
				foreach (ISkill skill in characterClass.SkillsAvailable)
				{
					randomSkills.Add(new RandomObject<ISkill>(skill, 1f));
				}
			}
			randomSkillCollection = new RandomObjectCollection<ISkill>(randomSkills);
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
				foreach (ISkill skill in characterClass.SkillsAvailable)
				{
					randomSkills.Add(new RandomObject<ISkill>(skill, 1f));
				}
			}

			randomSkillCollection = new RandomObjectCollection<ISkill>(randomSkills);
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
		randomSkills = new List<RandomObject<ISkill>>();

		foreach (ISkill skill in characterClass.SkillsAvailable)
		{
			randomSkills.Add(new RandomObject<ISkill>(skill, 1f));
		}

		randomSkillCollection = new RandomObjectCollection<ISkill>(randomSkills);
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

	public override ISkill ChooseSkill(Character currentActor)
	{
		Debug.Log("randomSkills.Count: " + randomSkills.Count);
		Debug.Log("RandomSkillCollection.RandomObjects.Count: " + RandomSkillCollection.RandomObjects.Count);
		RandomObject<ISkill> randomObject = RandomSkillCollection.GetRandomObject(currentActor.SkillSystem.LockedSkillsList);
		//Debug.Log(currentActor.SkillSystem.LockedSkillsList[0].name);
		return (ISkill)randomObject.randomObject;
	}

	public override List<Character> ChooseTargets(List<Character> availableTargets, ISkill skill)
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


