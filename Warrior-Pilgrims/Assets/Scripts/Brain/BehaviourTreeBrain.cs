using GramophoneUtils.Characters;
using GramophoneUtils.Stats;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviourTreeBrain : Brain
{
    public override ISkill ChooseSkill(Character currentActor)
    {
        throw new System.NotImplementedException();
    }

    public override List<Character> ChooseTargets(List<Character> availableTargets, ISkill skill)
    {
        throw new System.NotImplementedException();
    }
}
