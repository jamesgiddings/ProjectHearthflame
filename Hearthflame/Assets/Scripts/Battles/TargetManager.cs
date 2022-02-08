using GramophoneUtils.Stats;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Enum.Extensions;

public class TargetManager 
{
	private BattleManager battleManager;
	public TargetManager(BattleManager battleManager)
	{
		this.battleManager = battleManager;
	}

	public List<Character> GetAllPossibleTargets(Skill skill, Character originator)
	{
		List<Character> AllBattlers = battleManager.OrderedBattlersList;
		Debug.Log(AllBattlers.Count);
		IEnumerable<Character> query;

		if (originator.IsPlayer)
		{

		}
		else
		{

		}

		Debug.Log("skill.TargetAreaFlag:" + skill.TargetAreaFlag);
		query = AllBattlers.Where(battler => battler.GetTargetAreaFlag().Has(skill.TargetAreaFlag));
		Debug.Log("Getting all possible targets");

		List<Character> allPossibleTargets = new List<Character>();
		foreach (Character character in query)
		{
			allPossibleTargets.Add(character);
			Debug.Log(character.Name);
		}

		return null;
	}

	public Character[] GetCurrentlyTargeted()
	{
		return null;
	}

	public Character[] ChangeTargeted()
	{
		return null;
	}
}
