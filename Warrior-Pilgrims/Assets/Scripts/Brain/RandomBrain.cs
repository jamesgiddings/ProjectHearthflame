using GramophoneUtils.Characters;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Random Brain", menuName = "Brains/New Random Brain")]
public class RandomBrain : Resource, IBrain
{
    #region Attributes/Fields/Properties

    [SerializeField] RandomSkillObjectCollection _randomSkillObjectCollection;
    [SerializeField] RandomCharacterClassObjectCollection _randomCharacterClassObjectCollection;

    #endregion

    #region Constructors
    #endregion

    #region Callbacks
    #endregion

    #region Public Functions

    /// <summary>
    /// Get the RandomSkillObjectCollection to choose a skill randomly, passing the character's locked skills as the blacklist parameter
    /// </summary>
    /// <param name="character"></param>
    /// <returns></returns>
    public ISkill ChooseSkill(Character character)
    {
        if (_randomSkillObjectCollection == null)
        {
            throw new NullReferenceException("_randomSkillObjectCollection was null.");
        }

        IRandomObject<ISkill> randomObject = _randomSkillObjectCollection.GetRandomObject(character.SkillSystem.LockedSkillsList); 
        return (ISkill)randomObject.WeightedObject;
    }

    public List<Character> ChooseTargets(Character originator, ISkill skill) // TODO update this to avoid target manager, and not have to use RandomObject<>?
    {
        List<Character> targets = new List<Character>();

        List<RandomObject<Character>> randomTargets = new List<RandomObject<Character>>();

        List<Character> availableTargets = ServiceLocator.Instance.TargetManager.GetAllPossibleTargets(skill, originator);

        foreach (Character character in availableTargets)
        {
            randomTargets.Add(new RandomObject<Character>(character, _randomCharacterClassObjectCollection.GetWeighting(character.CharacterClass)));
        }

        RandomObjectCollection<Character> randomTargetCollection = new RandomObjectCollection<Character>(randomTargets);

        if (skill.TargetNumberFlag.HasFlag(TargetNumberFlag.Single))
        {
            targets.Add(randomTargetCollection.GetRandomObject().randomObject);
            return targets;
        }

        return null;
    }

    public void SetRandomSkillObjectCollection(RandomSkillObjectCollection randomSkillObjectCollection)
    {
        _randomSkillObjectCollection = randomSkillObjectCollection;
    }

    #endregion

    #region Protected Functions
    #endregion

    #region Private Functions
    #endregion

    #region Inner Classes
    #endregion

}
