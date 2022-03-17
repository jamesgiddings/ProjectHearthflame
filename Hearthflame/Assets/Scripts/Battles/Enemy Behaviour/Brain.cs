using GramophoneUtils.Stats;
using System.Collections.Generic;
using UnityEngine;

public abstract class Brain : ScriptableObject
{
	[SerializeField] protected CharacterClass characterClass;
	[SerializeField] protected ResourceDatabase resourceDatabase;

	protected Character brainOwner;

	public abstract Skill ChooseSkill(Character currentActor);

	public abstract List<Character> ChooseTargets(List<Character> availableTargets, Skill skill);

	public void Initialise(Character character)
	{
		brainOwner = character;
	}
}
