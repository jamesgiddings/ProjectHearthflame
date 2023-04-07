using System;
using System.Collections.Generic;
using UnityEngine;

public interface ITargetManager
{
    List<ICharacter> CurrentTargetsCache { get; }
    bool IsTargeting { get; }
    Action<List<ICharacter>> OnCurrentTargetsChanged { get; set; }


    List<ICharacter> ChangeTargeted(Vector2 direction);
    void ClearTargets();
    List<ICharacter> GetAllPossibleTargets(ISkill skill, ICharacter originator);
    List<ICharacter> GetCurrentlyTargeted(ISkill skill, ICharacter originator);
    ICharacter GetTargetByMouse();
    void SimulatePlayerTargeting(ISkill skill, List<ICharacter> charactersToTarget, ICharacter originator);
    void SubscribeToBattleDataModelOnSkillUsed();
    void UnsubscribeFromBattleDataModelOnSkillUsed();
    void UseSkill(ICharacter originator);
}
