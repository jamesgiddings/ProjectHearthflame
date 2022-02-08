using GramophoneUtils.Items.Hotbars;
using GramophoneUtils.Stats;
using System;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "New Skill", menuName = "Character Classes/Skills")]
public class Skill : Resource, IHotbarItem
{
	[SerializeField] private CharacterClass[] classRestrictions;
	[SerializeField] private Skill[] prerequisites;
	[SerializeField] private int usesToUnlock;
	[SerializeField] private TargetFlags targetFlag;

	private int targetFlagSum;

	public Action OnSkillUsed;
	public CharacterClass[] ClassRestrictions => classRestrictions; // getter
	public Skill[] Prerequisites => prerequisites; // getter
	public int UsesToUnlock => usesToUnlock; // getter

	private void OnValidate()
	{
		SetTargetFlagSum();
	}

	private void SetTargetFlagSum()
	{
		targetFlagSum = (int)targetFlag;
	}

	public void Use()
	{
		throw new System.NotImplementedException();
	}

	public bool CanStartUnlocking(Skill skill, Character character)
	{
		if (character.CharacterClass.SkillsAvailable.Contains(skill))
		{
			if (prerequisites.Intersect(character.SkillSystem.UnlockedSkills).Count() == prerequisites.Count()) // if the intersection of the two lists is the same length, then the UnlockedSkills contains all the prerequiste skills
			{
				return true;
			}
		}
		return false;
	}

	public bool CanUnlock(Skill skill, Character character)
	{
		if (CanStartUnlocking(skill, character))
		{
			if (skill.usesToUnlock == 0)
			{
				return true;
			}
			else if (character.SkillSystem.LockedSkills.ContainsKey(skill))
			{
				if (character.SkillSystem.LockedSkills[skill] >= skill.usesToUnlock)
				{
					return true;
				}
			}
		}
		return false;
	}
}
