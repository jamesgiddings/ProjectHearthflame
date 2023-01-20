using GramophoneUtils.Characters;
using System;
using System.Collections.Generic;
using UnityEngine;

public interface ITargetManager
{
    List<Character> CurrentTargetsCache { get; }
    bool IsTargeting { get; }
    Action<List<Character>> OnCurrentTargetsChanged { get; set; }


    List<Character> ChangeTargeted(Vector2 direction);
    void ClearTargets();
    List<Character> GetAllPossibleTargets(ISkill skill, Character originator);
    List<Character> GetCurrentlyTargeted(ISkill skill, Character originator);
    Character GetTargetByMouse();
    void SimulatePlayerTargeting(ISkill skill, List<Character> charactersToTarget, Character originator);
    void SubscribeToBattleDataModelOnSkillUsed();
    void UnsubscribeFromBattleDataModelOnSkillUsed();
    void UseSkill(Character originator);
}