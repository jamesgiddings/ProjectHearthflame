using GramophoneUtils.Characters;
using GramophoneUtils.Stats;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Sequence Brain", menuName = "Brains/New Sequence Brain")]
public class SequenceBrain : Brain
{
	[SerializeField] private List<Skill> skillOrder = null;
	private Queue<Skill> SkillQueue => new Queue<Skill>(skillOrder);

	private void OnValidate()
	{
		if (skillOrder.Count == 0)
		{
			foreach (Skill skill in characterClass.SkillsAvailable)
			{
				skillOrder.Add(skill);
			}
		}
	}

	public override Skill ChooseSkill(Character currentActor)
	{
		// Check skills are unlocked

		Skill skillToUse;
		skillToUse = SkillQueue.Dequeue();
		SkillQueue.Enqueue(skillToUse);
		return skillToUse;
	}

	public override List<Character> ChooseTargets(List<Character> availableTargets, Skill skill)
	{
		throw new System.NotImplementedException();
	}
}
