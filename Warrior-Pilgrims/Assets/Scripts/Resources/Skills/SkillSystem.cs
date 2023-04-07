using System;
using System.Collections.Generic;

public class SkillSystem
{
	private ICharacter character;
	private List<ISkill> unlockedSkills;
	private Dictionary<ISkill, int> lockedSkills;
	private ISkill activeSkill;

	public Action<ISkill, List<ICharacter>> OnSkillUsed;
	public List<ISkill> LockedSkillsList => new List<ISkill>(lockedSkills.Keys);
	
	private int skillUseIncreaseMinimumInclusive = 1; // these are so we can set the range of 
	private int skillUseIncreaseMaximumExclusive = 2; // possible increases when a skill is used.

	public ICharacter Character => character;

	public List<ISkill> UnlockedSkills => unlockedSkills; // getter

	public Dictionary<ISkill, int> LockedSkills => lockedSkills; // getter

	public SkillSystem(ICharacter character) // constructor
	{
		this.character = character;
		unlockedSkills = new List<ISkill>();
		lockedSkills = new Dictionary<ISkill, int>();
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

	private Dictionary<ISkill, int> InitialiseLockedSkillsDictionary()
	{
		Dictionary<ISkill, int> dict = new Dictionary<ISkill, int>();
		foreach (ISkill skill in character.CharacterClass.SkillsAvailable)
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

	private ISkill GetDefaultNextActiveSkill()
	{
		if (GetSkillsAvailableToStartUnlocking().Count > 0)
		{
			return GetSkillsAvailableToStartUnlocking()[0];
		}
		return null;
	}

	private List<ISkill> GetSkillsAvailableToStartUnlocking()
	{
		List<ISkill> skillsAvailable = new List<ISkill>();
		foreach (KeyValuePair<ISkill, int> keyValuePair in lockedSkills)
		{
			if (keyValuePair.Key.CanStartUnlocking(keyValuePair.Key, character))
			{
				skillsAvailable.Add((keyValuePair.Key));
			}
		}
		
		return skillsAvailable;
		
	}

	public void UnlockSkill(ISkill skill)
	{
		unlockedSkills.Add(skill);
		lockedSkills.Remove(skill);
	}

	public bool IsSkillUnlocked(ISkill skill)
	{
		return unlockedSkills.Contains(skill);
	}

	public void IncreaseSkillUses(ISkill skill, List<ICharacter> targets)
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
