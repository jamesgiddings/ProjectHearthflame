using GramophoneUtils.Stats;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Enum.Extensions;
using System;

public class TargetManager 
{
	private BattleManager battleManager;

	private bool isTargeting;

	private Skill currentSkill;

	private int targetIndex;

	private List<Character> allPossibleTargetsCache = new List<Character>();

	private List<Character> currentTargetsCache = new List<Character>();

	public Action<List<Character>> OnCurrentTargetsChanged;

	public TargetManager(BattleManager battleManager)
	{
		this.battleManager = battleManager;
		battleManager.OnSkillUsed += UseSkill;
	}

	private void OnDestroy()
	{
		battleManager.OnSkillUsed -= UseSkill;
	}
	public List<Character> GetAllPossibleTargets(Skill skill, Character originator)
	{
		currentSkill = skill;
		targetIndex = 0;
		List<Character> AllBattlers = battleManager.BattlersList; // we want to use the BattlersList, not the ordered list becuase Battlers list is what is used to determine the order they are displayed.
		Debug.Log(AllBattlers.Count);
		IEnumerable<Character> query;
		List<Character> allPossibleTargets = new List<Character>();
		if (originator.IsPlayer)
		{
			query = AllBattlers.Where(battler => skill.TargetAreaFlag.Has(battler.GetTargetAreaFlag())); // check that the skills (mixed) TargetAreaFlag contains the battlers TargetAreaFlag (which should just be one area)
			Debug.Log("Getting all possible targets");

			foreach (Character character in query)
			{
				allPossibleTargets.Add(character);
				Debug.Log(character.Name);
			}
		}
		else
		{
			Debug.LogError("Enemy targeting not implemented.");
			//query = AllBattlers.Where(battler => skill.TargetAreaFlag.Has(battler.GetTargetAreaFlag())); // check that the skills (mixed) TargetAreaFlag contains the battlers TargetAreaFlag (which should just be one area)
			//Debug.Log("Getting all possible targets");

			//List<Character> allPossibleTargets = new List<Character>();
			//foreach (Character character in query)
			//{
			//	allPossibleTargets.Add(character);
			//	Debug.Log(character.Name);
			//}
		}

		allPossibleTargetsCache = allPossibleTargets;

		return allPossibleTargets;
	}

	public void UseSkill(Character originator)
	{
		currentSkill.Use(GetCurrentlyTargeted(currentSkill, originator));
	}

	public List<Character> GetCurrentlyTargeted(Skill skill, Character originator)
	{
		currentTargetsCache = new List<Character>(GetAllPossibleTargets(skill, originator));
		List<Character> currentlyTargeted = new List<Character>();
		switch (skill.TargetNumberFlag)
		{
			case TargetNumberFlag.Single:
				currentlyTargeted.Add(currentTargetsCache.FirstOrDefault());
				break;
			case TargetNumberFlag.All:
				currentlyTargeted.AddRange(currentTargetsCache);
				break;
			case TargetNumberFlag.AllExceptUser:
				currentlyTargeted.AddRange(currentTargetsCache.Where(character => character != originator));
				break;
		}
		OnCurrentTargetsChanged?.Invoke(currentlyTargeted);
		return currentlyTargeted;
	}

	public List<Character> ChangeTargeted(Vector2 direction) //float x, float y)
	{
		switch (currentSkill.TargetNumberFlag)
		{
			case TargetNumberFlag.Single:
				List<Character> currentlyTargeted = new List<Character>();
				switch (direction)
				{
					case Vector2 vector when (vector.x >= 0f && Math.Abs(vector.x) > Math.Abs(vector.y)): // move right
						break;
					case Vector2 vector when (vector.x <= 0f && Math.Abs(vector.x) > Math.Abs(vector.y)): // move left
						break;
					case Vector2 vector when (vector.y >= 0f && Math.Abs(vector.y) > Math.Abs(vector.x)): // move down
						targetIndex--;
						if (targetIndex < 0)
						{
							targetIndex = currentTargetsCache.Count - 1;
						}
						if (targetIndex > (currentTargetsCache.Count - 1))
						{
							targetIndex = 0;
						}
						Debug.Log(targetIndex);
						if (targetIndex > -1 && targetIndex < currentTargetsCache.Count)
							currentlyTargeted.Add(currentTargetsCache[targetIndex]);
						GameManager.Instance.StartCoroutine(InputDelay(0.1f));
						break;
					case Vector2 vector when (vector.y <= 0f && Math.Abs(vector.y) > Math.Abs(vector.x)): // move up
						targetIndex++;
						if (targetIndex < 0)
						{
							targetIndex = currentTargetsCache.Count - 1;
						}
						if (targetIndex > (currentTargetsCache.Count - 1))
						{
							targetIndex = 0;
						}
						Debug.Log(targetIndex);
						if (targetIndex > -1 && targetIndex < currentTargetsCache.Count)
							currentlyTargeted.Add(currentTargetsCache[targetIndex]);
						GameManager.Instance.StartCoroutine(InputDelay(0.1f));
						break;
						
				}
				OnCurrentTargetsChanged?.Invoke(currentlyTargeted);
				return currentlyTargeted;
			default:
				return currentTargetsCache;
		}
	}

	IEnumerator InputDelay(float time)
	{
		yield return new WaitForSeconds(time);
	}

	public Character GetTargetByMouse()
	{
		Debug.LogError("Mouse targeting not implemented.");
		return null;
	}
}
