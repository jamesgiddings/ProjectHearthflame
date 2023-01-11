using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[CreateAssetMenu(fileName = "New Random Character Class Object Collection", menuName = "Skills/Random Character Class Object Collection")]
public class RandomCharacterClassObjectCollection : SerializedScriptableObject, IRandomObjectCollection<CharacterClass>
{
    #region Attributes/Fields/Properties

    [SerializeField, TableList(DrawScrollView = true, MaxScrollViewHeight = 200, MinScrollViewHeight = 100)] List<RandomCharacterClassObject> _randomCharacterClassObjects;
    public List<RandomCharacterClassObject> RandomCharacterClassObjects => _randomCharacterClassObjects;

    private float _weightingSum;

    #endregion

    #region Constructors
    #endregion

    #region Callbacks
    #endregion

    #region Public Functions

    public IRandomObject<CharacterClass> GetRandomObject(List<CharacterClass> blacklist = null, List<CharacterClass> whitelist = null)
    {
        if (_randomCharacterClassObjects.Count == 0)
        {
            throw new Exception("There are no random classes to choose from");
        }

        List<RandomCharacterClassObject> tempRandomCharacterClassObjects;

        try
        {
            tempRandomCharacterClassObjects = ApplyBlacklistAndWhiteList(blacklist, whitelist);
        }
        catch (Exception)
        {
            throw new Exception();
        }

        _weightingSum = GetWeightingSum(blacklist, whitelist);
        float rand = Random.Range(0f, _weightingSum);
        float runningSum = 0;

        foreach (RandomCharacterClassObject randomCharacterClassObject in tempRandomCharacterClassObjects)
        {
            runningSum += randomCharacterClassObject.Weighting;
            if (rand <= runningSum)
            {
                return randomCharacterClassObject;
            }
        }
        throw new Exception("Failed to select randomCharacetClassObject. Random number exceeded weighting sum.");
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
    public List<IRandomObject<CharacterClass>> GetRandomObjects(int number, List<CharacterClass> blacklist = null, bool allowDuplicateObjects = true, int maximumAttempts = 100)
    {
        List<IRandomObject<CharacterClass>> resources = new List<IRandomObject<CharacterClass>>();
        for (int i = 0; i < number; i++)
        {
            IRandomObject<CharacterClass> randomCharacterClassObject = GetRandomObject(blacklist);
            if (allowDuplicateObjects)
            {
                resources.Add(GetRandomObject(blacklist));
            }
            else
            {
                if (resources.Contains(randomCharacterClassObject))
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

    public float GetWeighting(CharacterClass characterClass)
    {
        foreach (var randomObject in _randomCharacterClassObjects)
        {
            if (randomObject.WeightedObject.Equals(characterClass))
            {
                return randomObject.Weighting;
            }
        }
        throw new Exception("CharacterClass not found in collection");
    }

    #endregion

    #region Protected Functions
    #endregion

    #region Private Functions

    private List<RandomCharacterClassObject> ApplyBlacklistAndWhiteList(List<CharacterClass> blacklist, List<CharacterClass> whitelist)
    {
        List<RandomCharacterClassObject> tempRandomCharacterClassObjects = new List<RandomCharacterClassObject>();

        if (whitelist != null)
        {
            for (int i = 0; i < _randomCharacterClassObjects.Count; i++)
            {
                if (whitelist.Contains(_randomCharacterClassObjects[i].WeightedObject))
                {
                    tempRandomCharacterClassObjects.Add(_randomCharacterClassObjects[i]);
                }
            }
            if (tempRandomCharacterClassObjects.Count == 0)
            {
                throw new Exception("No overlap between RandomCharacterClassObjectCompilation and whitelist parameter.");
            }
        }
        else
        {
            tempRandomCharacterClassObjects.AddRange(_randomCharacterClassObjects);
        }

        if (blacklist != null)
        {
            for (int i = tempRandomCharacterClassObjects.Count - 1; i >= 0; i--)
            {
                if (blacklist.Contains(tempRandomCharacterClassObjects[i].WeightedObject))
                {
                    tempRandomCharacterClassObjects.Remove(tempRandomCharacterClassObjects[i]);
                }
            }
            if (tempRandomCharacterClassObjects.Count == 0)
            {
                throw new Exception("Blacklist has removed all skills");
            }
        }
        return tempRandomCharacterClassObjects;
    }

    private float GetWeightingSum(List<CharacterClass> blacklist = null, List<CharacterClass> whitelist = null)
    {
        List<RandomCharacterClassObject> tempRandomCharacterClassObjects = ApplyBlacklistAndWhiteList(blacklist, whitelist);

        float sum = 0;

        foreach (RandomCharacterClassObject randomCharacterClassObject in tempRandomCharacterClassObjects)
        {
            if (blacklist != null)
            {
                if (!blacklist.Contains(randomCharacterClassObject.WeightedObject))
                {
                    sum += randomCharacterClassObject.Weighting;
                }
            }
            else
            {
                sum += randomCharacterClassObject.Weighting;
            }
        }
        return sum;
    }

    #endregion

    #region Inner Classes
    #endregion

}
