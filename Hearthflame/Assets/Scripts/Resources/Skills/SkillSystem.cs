using GramophoneUtils.Stats;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillSystem : MonoBehaviour
{
	private Character character;
	private List<Skill> unlockedSkillTypeList;
	private List<Skill> availableSkills;
	public Character Character => character;
	
	public SkillSystem(CharacterTemplate template, Character character)
	{
		this.character = character;
		unlockedSkillTypeList = new List<Skill>();
	}

	public void UnlockSkill(Skill skill)
	{
		unlockedSkillTypeList.Add(skill);
	}

	public bool IsSkillUnlocked(Skill skill)
	{
		return unlockedSkillTypeList.Contains(skill);
	}
}
