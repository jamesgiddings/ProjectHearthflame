using GramophoneUtils.Items.Hotbars;
using GramophoneUtils.Stats;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "New Skill", menuName = "Character Classes/Skills")]
public class Skill : Resource, IHotbarItem
{
	[SerializeField] private CharacterClass[] classRestrictions;
	[SerializeField] private Skill[] prerequisites;
	[SerializeField] private int usesToUnlock;
	[SerializeField] private TargetAreaFlag targetAreaFlag;
	[SerializeField] private TargetNumberFlag targetNumberFlag;
	[SerializeField] private TargetTypeFlag targetTypeFlag = TargetTypeFlag.Alive;

	[SerializeField] private List<StatModifierBlueprint> statModifierBlueprints;
	[SerializeField] private List<DamageBlueprint> damageBlueprints;
	[SerializeField] private List<HealingBlueprint> healingBlueprints;

	public Action OnSkillUsed;
	public CharacterClass[] ClassRestrictions => classRestrictions; // getter
	public Skill[] Prerequisites => prerequisites; // getter
	public int UsesToUnlock => usesToUnlock; // getter

	public TargetAreaFlag TargetAreaFlag => targetAreaFlag;
	public TargetNumberFlag TargetNumberFlag => targetNumberFlag;
	public TargetTypeFlag TargetTypeFlag => targetTypeFlag;

	private List<StatModifier> skillStatModifiers { get { return InstanceSkillStatModifierBlueprints(); } }
	private List<Damage> skillDamages { get { return InstanceSkillDamageBlueprints(); } }
	private List<Healing> skillHealings { get { return InstanceSkillHealingBlueprints(); } }

	private List<StatModifier> InstanceSkillStatModifierBlueprints()
	{
		List<StatModifier> statModifiers = new List<StatModifier>();
		if (statModifierBlueprints.Count > 0)
		{
			foreach (var blueprint in statModifierBlueprints)
			{
				statModifiers.Add(blueprint.CreateBlueprintInstance<StatModifier>(this));
			}
		}
		return statModifiers;
	}
	private List<Damage> InstanceSkillDamageBlueprints()
	{
		List<Damage> damages = new List<Damage>();
		if (damageBlueprints.Count > 0)
		{
			foreach (var blueprint in damageBlueprints)
			{
				damages.Add(blueprint.CreateBlueprintInstance<Damage>(this));
			}
		}
		return damages;
	}	
	
	private List<Healing> InstanceSkillHealingBlueprints()
	{
		List<Healing> healings = new List<Healing>();
		if (healingBlueprints.Count > 0)
		{
			foreach (var blueprint in healingBlueprints)
			{
				healings.Add(blueprint.CreateBlueprintInstance<Healing>(this));
			}
		}
		return healings;
	}

	public void Use(List<Character> characterTargets, Character originator)
	{
		Debug.LogWarning("Here is where we should instance the blueprints, instancing them with the originator. The target can then adapt them on reception.");
		foreach (Character character in characterTargets)
		{
			ApplyStatModifiers(character); // change these to 'sendModified' statModifier struct, to 'receiveModified' statModifier struct
			ApplyDamageObjects(character);
			ApplyHealingObjects(character);
		}
	}

	private void ApplyStatModifiers(Character character)
	{
		foreach (StatModifier statModifier in skillStatModifiers)
		{
			character.StatSystem.AddModifier(statModifier);
		}
	}

	private void ApplyDamageObjects(Character character)
	{
		foreach (Damage damage in skillDamages)
		{
			character.HealthSystem.AddDamage(damage);
		}
	}

	private void ApplyHealingObjects(Character character)
	{
		foreach (Healing healing in skillHealings)
		{
			character.HealthSystem.AddHealing(healing);
		}
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
