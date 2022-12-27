using GramophoneUtils.Characters;
using GramophoneUtils.Stats;
using System.Collections.Generic;
using UnityEngine;

public abstract class Brain : Resource
{
	[SerializeField] protected CharacterClass characterClass;
	[SerializeField] protected ResourceDatabase resourceDatabase;

	protected GramophoneUtils.Characters.Character brainOwner;

	public abstract Skill ChooseSkill(Character currentActor);

	public abstract List<Character> ChooseTargets(List<Character> availableTargets, Skill skill);

	public void Initialise(GramophoneUtils.Characters.Character character)
	{
		brainOwner = character;
	}
}
