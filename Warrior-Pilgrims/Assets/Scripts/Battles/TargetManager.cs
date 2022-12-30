using GramophoneUtils.Stats;
using GramophoneUtils.Characters;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Enum.Extensions;
using System;

public class TargetManager : MonoBehaviour
{
	private BattleManager battleManager;

	private bool _isTargeting;

	private Skill _currentSkill;

	private int _targetIndex = 0;

	private List<Character> allPossibleTargetsCache = new List<Character>();

	private List<Character> _currentTargetsCache = new List<Character>();

	public Action<List<Character>> OnCurrentTargetsChanged;

	public List<Character> CurrentTargetsCache => _currentTargetsCache; // getter

	public bool IsTargeting => _isTargeting; // getter

    private void OnEnable()
	{
        battleManager = ServiceLocator.Instance.BattleManager;
        battleManager.BattleDataModel.OnSkillUsed += UseSkill;
    }

    private void OnDisable()
	{
		battleManager.BattleDataModel.OnSkillUsed -= UseSkill;
	}

	public List<Character> GetAllPossibleTargets(Skill skill, Character originator)
	{
		_isTargeting = true;
		_currentSkill = skill;
		//List<Character> AllBattlers = battleManager.BattleDataModel.BattlersList; // we want to use the BattlersList, not the ordered list becuase Battlers list is what is used to determine the order they are displayed.
		List<Character> AllBattlers = new List<Character>();
		AllBattlers.AddRange(ServiceLocator.Instance.CharacterModel.FrontPlayerCharactersList); // we want to use the BattlersList, not the ordered list becuase Battlers list is what is used to determine the order they are displayed.
        AllBattlers.AddRange(ServiceLocator.Instance.CharacterModel.FrontEnemyCharactersList);
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
        _currentSkill.Use(GetCurrentlyTargeted(_currentSkill, originator), originator);
		ClearTargets();
	}

	public void ClearTargets()
	{
		_currentTargetsCache.Clear();
		OnCurrentTargetsChanged?.Invoke(_currentTargetsCache);
		_currentSkill = null;
		_targetIndex = 0;
		_isTargeting = false;
	}

	public List<Character> GetCurrentlyTargeted(Skill skill, Character originator)
	{
		_currentTargetsCache = new List<Character>(GetAllPossibleTargets(skill, originator));
		List<Character> currentlyTargeted = new List<Character>();
		switch (skill.TargetNumberFlag)
		{
			case TargetNumberFlag.Single:
				if (originator.IsPlayer)
				{
					int index = _currentTargetsCache.Count > _targetIndex ? _targetIndex : 0; // loop around the possible targets
					if (_currentTargetsCache.Count > 0)
					{
						currentlyTargeted.Add(_currentTargetsCache[index]);
					}
				}
				else
				{
					currentlyTargeted.AddRange(originator.Brain.ChooseTargets(_currentTargetsCache, skill));
				}
				break;
			case TargetNumberFlag.All:
				currentlyTargeted.AddRange(_currentTargetsCache);
				break;
			case TargetNumberFlag.AllExceptUser:
				currentlyTargeted.AddRange(_currentTargetsCache.Where(character => character != originator));
				break;
			case TargetNumberFlag.Self:
				currentlyTargeted.AddRange(_currentTargetsCache.Where(character => character == originator));
				break;
		}
		if (currentlyTargeted.Count > 0)
			OnCurrentTargetsChanged?.Invoke(currentlyTargeted);
		return currentlyTargeted;
	}

	public List<Character> ChangeTargeted(Vector2 direction) //float x, float y)
	{
		switch (_currentSkill.TargetNumberFlag)
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
						_targetIndex--;
						if (_targetIndex < 0)
						{
							_targetIndex = _currentTargetsCache.Count - 1;
						}
						if (_targetIndex > (_currentTargetsCache.Count - 1))
						{
							_targetIndex = 0;
						}
						if (_targetIndex > -1 && _targetIndex < _currentTargetsCache.Count)
							currentlyTargeted.Add(_currentTargetsCache[_targetIndex]);
						GameManager.Instance.StartCoroutine(InputDelay(0.1f));
						break;
					case Vector2 vector when (vector.y <= 0f && Math.Abs(vector.y) > Math.Abs(vector.x)): // move up
						_targetIndex++;
						if (_targetIndex < 0)
						{
							_targetIndex = _currentTargetsCache.Count - 1;
						}
						if (_targetIndex > (_currentTargetsCache.Count - 1))
						{
							_targetIndex = 0;
						}
						if (_targetIndex > -1 && _targetIndex < _currentTargetsCache.Count)
							currentlyTargeted.Add(_currentTargetsCache[_targetIndex]);
						GameManager.Instance.StartCoroutine(InputDelay(0.1f));
						break;
						
				}
				OnCurrentTargetsChanged?.Invoke(currentlyTargeted);
				return currentlyTargeted;
			default:
				return _currentTargetsCache;
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
