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

    void Use(List<ICharacter> characterTargets, ICharacter originator);

    void DoNextBit(List<ICharacter> characterTargets, ICharacter originator);

    bool CanStartUnlocking(ISkill skill, ICharacter character);

    bool CanUnlock(ISkill skill, ICharacter character);

    #endregion

    #region Protected Functions
    #endregion

    #region Private Functions
    #endregion

    #region Inner Classes
    #endregion
}
