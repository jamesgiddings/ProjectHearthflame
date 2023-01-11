using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;
using System.Linq;

[CreateAssetMenu(fileName = "New Random Skill Object Collection", menuName = "Skills/Random Skill Object Collection")]
public class RandomSkillObjectCollection : SerializedScriptableObject, IRandomObjectCollection<ISkill>
{
    #region Attributes/Fields/Properties

    [SerializeField, TableList(DrawScrollView = true, MaxScrollViewHeight = 200, MinScrollViewHeight = 100)] List<RandomSkillObject> _randomSkillObjects;
    public List<RandomSkillObject> RandomSkillObjects => _randomSkillObjects;

    private float _weightingSum;

    #endregion

    #region Constructors
    #endregion

    #region Callbacks
    #endregion

    #region Public Functions

    public IRandomObject<ISkill> GetRandomObject(List<ISkill> blacklist = null, List<ISkill> whitelist = null)
    {
        if (_randomSkillObjects.Count == 0)
        {
            throw new Exception("There are no random skills to choose from");
        }

        List<RandomSkillObject> tempRandomSkillObjects;

        try
        {
            tempRandomSkillObjects = ApplyBlacklistAndWhiteList(blacklist, whitelist);
        }
        catch (Exception)
        {
            throw new Exception();
        }
        
        _weightingSum = GetWeightingSum(blacklist, whitelist);
        float rand = Random.Range(0f, _weightingSum);
        float runningSum = 0;

        foreach (RandomSkillObject randomSkillObject in tempRandomSkillObjects)
        {
            runningSum += randomSkillObject.Weighting;
            if (rand <= runningSum)
            {
                return randomSkillObject;
            }
        }
        throw new Exception("Failed to select randomSkillObject. Random number exceeded weighting sum.");
    }

    

    /// <summary>
    /// This allows the caller to get a list of random objects. The int maximumAttempts parameter is to stop the game
    /// continuously trying to find an object which it may not find for a long time given a small collection size, especially
    /// if allowDuplicateObjects is set to false. If maximumAttempts is set to -1, the function will repeat until the conditions
    /// are met.
    /// </summary>
    /// <param name="number"></param>
    /// <param name="allowDuplicateObjects"></param>
    /// <param name="blacklist"></param>
    /// <param name="maximumAttempts"></param>
    /// <returns></returns>
    public List<IRandomObject<ISkill>> GetRandomObjects(int number, List<ISkill> blacklist = null, bool allowDuplicateObjects = true, int maximumAttempts = 100)
    {
        List<IRandomObject<ISkill>> resources = new List<IRandomObject<ISkill>>();
        for (int i = 0; i < number; i++)
        {
            IRandomObject<ISkill> randomSkillObject = GetRandomObject(blacklist);
            if (allowDuplicateObjects)
            {
                resources.Add(GetRandomObject(blacklist));
            }
            else
            {
                if (resources.Contains(randomSkillObject)) 
                {
                    i--; // loop through again
                    if (maximumAttempts != -1) // if we want to control the number of retries
                    {
                        if (!(maximumAttempts > 0)) // if we have made all the attempts set out in the parameters
                        {
                            return resources; // return early
                        }
                        maximumAttempts--;
                    }
                    continue;
                }
            }
        }
        return resources;
    }

#if UNITY_EDITOR
    public void AddSkillObject(RandomSkillObject randomSkillObject)
    {
        if (_randomSkillObjects == null)
        {
            _randomSkillObjects = new List<RandomSkillObject>();
        }
        _randomSkillObjects.Add(randomSkillObject);
        EditorUtility.SetDirty(this);
    }
#endif

    #endregion

    #region Protected Functions
    #endregion

    #region Private Functions

    private List<RandomSkillObject> ApplyBlacklistAndWhiteList(List<ISkill> blacklist, List<ISkill> whitelist)
    {
        List<RandomSkillObject> tempRandomSkillObjects = new List<RandomSkillObject>();

        if (whitelist != null)
        {
            for (int i = 0; i < _randomSkillObjects.Count; i++)
            {
                if (whitelist.Contains(_randomSkillObjects[i].WeightedObject))
                {
                    tempRandomSkillObjects.Add(_randomSkillObjects[i]);
                }
            }
            if (tempRandomSkillObjects.Count == 0)
            {
                throw new Exception("No overlap between RandomSkillObjectCompilation and whitelist parameter.");
            }
        }
        else
        {
            tempRandomSkillObjects.AddRange(_randomSkillObjects);
        }

        if (blacklist != null)
        {
            for (int i = tempRandomSkillObjects.Count - 1; i >= 0; i--)
            {
                if (blacklist.Contains(tempRandomSkillObjects[i].WeightedObject))
                {
                    tempRandomSkillObjects.Remove(tempRandomSkillObjects[i]);
                }
            }
            if (tempRandomSkillObjects.Count == 0)
            {
                throw new Exception("Blacklist has removed all skills");
            }
        }
        return tempRandomSkillObjects;
    }

    private float GetWeightingSum(List<ISkill> blacklist = null, List<ISkill> whitelist = null)
    {
        List<RandomSkillObject> tempRandomSkillObjects = ApplyBlacklistAndWhiteList(blacklist, whitelist);

        float sum = 0;

        foreach (RandomSkillObject randomSkillObject in tempRandomSkillObjects)
        {
            if (blacklist != null)
            {
                if (!blacklist.Contains(randomSkillObject.WeightedObject))
                {
                    sum += randomSkillObject.Weighting;
                }
            }
            else
            {
                sum += randomSkillObject.Weighting;
            }
        }
        return sum;
    }

    #endregion

    #region Inner Classes
    #endregion

}
