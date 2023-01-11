using GramophoneUtils.Characters;
using GramophoneUtils.Stats;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Sequence Brain", menuName = "Brains/New Sequence Brain")]
public class SequenceBrain : Brain
{
	[SerializeField] private List<ISkill> skillOrder = null;
	private Queue<ISkill> SkillQueue => new Queue<ISkill>(skillOrder);

#if UNITY_EDITOR
	private void OnValidate()
	{
		if (skillOrder.Count == 0)
		{
			foreach (ISkill skill in characterClass.SkillsAvailable)
			{
				skillOrder.Add(skill);
			}
		}
	}
#endif

    public override ISkill ChooseSkill(Character currentActor)
	{
		// Check skills are unlocked

		ISkill skillToUse;
		skillToUse = SkillQueue.Dequeue();
		SkillQueue.Enqueue(skillToUse);
		return skillToUse;
	}

	public override List<Character> ChooseTargets(List<Character> availableTargets, ISkill skill)
	{
		throw new System.NotImplementedException();
	}
}
