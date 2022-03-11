using GramophoneUtils.Stats;
using System;
using System.Collections.Generic;
using UnityEngine;

public class SkillSystem
{
	private Character character;
	private List<Skill> unlockedSkills;
	private Dictionary<Skill, int> lockedSkills;
	private Skill activeSkill;

	public Action<Skill, List<Character>> OnSkillUsed;

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
		lockedSkills = InitialiseLockedSkillsDictionary();
		if (lockedSkills.Count > 0)
		{
			activeSkill = GetDefaultNextActiveSkill();
		}
		else
		{
			activeSkill = null;
		}
		OnSkillUsed += IncreaseSkillUses;
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
		return dict;
	}

	private Skill GetDefaultNextActiveSkill()
	{
		if (GetSkillsAvailableToStartUnlocking().Count > 0)
		{
			return GetSkillsAvailableToStartUnlocking()[0];
		}
		return null;
	}

	private List<Skill> GetSkillsAvailableToStartUnlocking()
	{
		List<Skill> skillsAvailable = new List<Skill>();
		foreach (KeyValuePair<Skill, int> keyValuePair in lockedSkills)
		{
			if (keyValuePair.Key.CanStartUnlocking(keyValuePair.Key, character))
			{
				skillsAvailable.Add((keyValuePair.Key));
			}
		}
		
		return skillsAvailable;
		
	}

	public void UnlockSkill(Skill skill)
	{
		unlockedSkills.Add(skill);
		lockedSkills.Remove(skill);
	}

	public bool IsSkillUnlocked(Skill skill)
	{
		return unlockedSkills.Contains(skill);
	}

	public void IncreaseSkillUses(Skill skill, List<Character> targets)
	{
		if (activeSkill == null)
		{
			return;
		}
		if (skill.School == activeSkill.School)
		{
			if (lockedSkills.ContainsKey(activeSkill))
			{
				lockedSkills[activeSkill] += GetSkillIncreaseAmount();
			}
		}
		if (activeSkill.CanUnlock(activeSkill, character))
		{
			UnlockSkill(activeSkill);
		}
	}

	public int GetSkillIncreaseAmount()
	{
		return UnityEngine.Random.Range(skillUseIncreaseMinimumInclusive, skillUseIncreaseMaximumExclusive);
	}
}
