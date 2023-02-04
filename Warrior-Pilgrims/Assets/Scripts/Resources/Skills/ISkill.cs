using GramophoneUtils.Characters;
using System.Collections.Generic;
using UnityEngine;

public interface ISkill : IApplyable
{
    #region Attributes/Fields/Properties

    string Name { get; }

    TargetNumberFlag TargetNumberFlag { get; }
    TargetAreaFlag TargetAreaFlag { get; }
    TargetTypeFlag TargetTypeFlag { get; }
    ITargetToSlots TargetToSlots { get; }
    IUseFromSlot UseFromSlot { get; }

    School School { get; }
    int UsesToUnlock { get; }

    SkillAnimType SkillAnimType { get; }
    GameObject ProjectilePrefab { get; }
    GameObject EffectPrefab { get; }
    RuntimeAnimatorController AnimatorController { get; }

    #endregion

    #region Constructors
    #endregion

    #region Callbacks
    #endregion

    #region Public Functions

    void Use(List<Character> characterTargets, Character originator);

    void DoNextBit(List<Character> characterTargets, Character originator);

    bool CanStartUnlocking(ISkill skill, Character character);

    bool CanUnlock(ISkill skill, Character character);

    #endregion

    #region Protected Functions
    #endregion

    #region Private Functions
    #endregion

    #region Inner Classes
    #endregion
}
