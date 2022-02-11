using GramophoneUtils.Stats;
using System.Collections.Generic;
using UnityEngine;

public abstract class Brain : ScriptableObject
{
	[SerializeField] protected CharacterClass characterClass;

	protected Character brainOwner;

	public abstract Skill ChooseSkill();

	public abstract List<Character> ChooseTargets(List<Character> availableTargets, Skill skill);

	public void Initialise(Character character)
	{
		brainOwner = character;
	}
}
