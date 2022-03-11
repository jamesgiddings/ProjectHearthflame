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

	private int targetIndex = 0;

	private List<Character> allPossibleTargetsCache = new List<Character>();

	private List<Character> currentTargetsCache = new List<Character>();

	public Action<List<Character>> OnCurrentTargetsChanged;

	public List<Character> CurrentTargetsCache => currentTargetsCache; // getter

	public bool IsTargeting => isTargeting; // getter

	public TargetManager(BattleManager battleManager)
	{
		this.battleManager = battleManager;
		battleManager.BattleDataModel.OnSkillUsed += UseSkill;
	}

	private void OnDestroy()
	{
		battleManager.BattleDataModel.OnSkillUsed -= UseSkill;
	}
	public List<Character> GetAllPossibleTargets(Skill skill, Character originator)
	{
		isTargeting = true;
		currentSkill = skill;
		List<Character> AllBattlers = battleManager.BattleDataModel.BattlersList; // we want to use the BattlersList, not the ordered list becuase Battlers list is what is used to determine the order they are displayed.
		IEnumerable<Character> query;
		List<Character> allPossibleTargets = new List<Character>();
		query = AllBattlers.Where(battler => skill.TargetAreaFlag.Has(battler.GetTargetAreaFlag(originator.IsPlayer)) && skill.TargetTypeFlag.Has(battler.GetTargetTypeFlag())); // check that the skills (mixed) TargetAreaFlag contains the battlers TargetAreaFlag (which should just be one area)
		foreach (Character character in query)
		{
			allPossibleTargets.Add(character);
		}
		
		allPossibleTargetsCache = allPossibleTargets;

		return allPossibleTargets;
	}

	public void UseSkill(Character originator)
	{
		currentSkill.Use(GetCurrentlyTargeted(currentSkill, originator), originator);
		ClearTargets();
	}

	public void ClearTargets()
	{		
		currentTargetsCache.Clear();
		OnCurrentTargetsChanged?.Invoke(currentTargetsCache);
		currentSkill = null;
		targetIndex = 0;
		isTargeting = false;
	}

	public List<Character> GetCurrentlyTargeted(Skill skill, Character originator)
	{
		currentTargetsCache = new List<Character>(GetAllPossibleTargets(skill, originator));
		List<Character> currentlyTargeted = new List<Character>();
		switch (skill.TargetNumberFlag)
		{
			case TargetNumberFlag.Single:
				if (originator.IsPlayer)
				{
					int index = currentTargetsCache.Count > targetIndex ? targetIndex : 0;
					if (currentTargetsCache.Count > 0)
					{
						currentlyTargeted.Add(currentTargetsCache[index]);
					}
				}
				else
				{
					currentlyTargeted.AddRange(originator.Brain.ChooseTargets(currentTargetsCache, skill));
				}
				break;
			case TargetNumberFlag.All:
				currentlyTargeted.AddRange(currentTargetsCache);
				break;
			case TargetNumberFlag.AllExceptUser:
				currentlyTargeted.AddRange(currentTargetsCache.Where(character => character != originator));
				break;
			case TargetNumberFlag.Self:
				currentlyTargeted.AddRange(currentTargetsCache.Where(character => character == originator));
				break;
		}
		if (currentlyTargeted.Count > 0)
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
