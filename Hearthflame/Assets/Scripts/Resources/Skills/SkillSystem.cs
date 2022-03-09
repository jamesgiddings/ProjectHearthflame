using GramophoneUtils.Stats;
using System.Collections.Generic;
using UnityEngine;

public class SkillSystem
{
	private Character character;
	private List<Skill> unlockedSkills;
	private Dictionary<Skill, int> lockedSkills;
	private Skill activeSkill;

	private int skillUseIncreaseMinimumInclusive = 1; // these are so we can set the range of 
	private int skillUseIncreaseMaximumExclusive = 2; // possible increases when a skill is used.

	public Character Character => character;

	public List<Skill> UnlockedSkills => unlockedSkills; // getter

	public Dictionary<Skill, int> LockedSkills => lockedSkills; // getter

	public SkillSystem(Character character) // constructor
	{
		this.character = character;
		unlockedSkills = new List<Skill>();
		lockedSkills = new Dictionary<Skill, int>();
	}

	public void Initialise()
	{
		InitialiseLockedSkillsDictionary();
	}

	private Dictionary<Skill, int> InitialiseLockedSkillsDictionary()
	{
		Dictionary<Skill, int> dict = new Dictionary<Skill, int>();

		foreach (Skill skill in character.CharacterClass.SkillsAvailable)
		{
			if (skill.CanUnlock(skill, character))
			{
				UnlockSkill(skill);
			}
			else
			{
				dict.Add(skill, 0);
			}
		}
		Debug.Log("Character name: " + character.Name);
		Debug.Log("Unlocked Skills: ");
		foreach (Skill skill in UnlockedSkills)
		{
			Debug.Log(skill.name);
		}		
		Debug.Log("Locked Skills: ");
		foreach (KeyValuePair<Skill, int> pair in LockedSkills)
		{
			Debug.Log(pair.Key.name);
			Debug.Log(pair.Value);
		}
		return dict;
	}

	public void UnlockSkill(Skill skill)
	{
		unlockedSkills.Add(skill);
	}

	public bool IsSkillUnlocked(Skill skill)
	{
		return unlockedSkills.Contains(skill);
	}

	public void UseSkill(Skill skill)
	{
		if (skill == activeSkill)
		{
			lockedSkills[skill] += SkillUseIncrease();
		}
	}

	public int SkillUseIncrease()
	{
		return Random.Range(skillUseIncreaseMinimumInclusive, skillUseIncreaseMaximumExclusive);
	}
}
