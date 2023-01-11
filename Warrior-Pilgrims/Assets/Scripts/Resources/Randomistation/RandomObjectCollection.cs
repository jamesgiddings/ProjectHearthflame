using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomObjectCollection<T>
{
	private List<RandomObject<T>> randomObjects;

	private float weightingSum;

	public List<RandomObject<T>> RandomObjects => randomObjects; // getter

	public RandomObjectCollection(List<RandomObject<T>> randomObjects)
	{
		this.randomObjects = randomObjects;
	}

	public float GetWeighting(T randomObject)
	{
		float weighting = 1;
		foreach (RandomObject<T> item in randomObjects)
		{
			if (EqualityComparer<T>.Default.Equals(item.randomObject, randomObject))
			{
				weighting = item.weighting;
			}
		}
		return weighting;
	}

	private float GetWeightingSum(List<T> blacklist = null)
	{
		float sum = 0;

		foreach (RandomObject<T> randomResource in randomObjects)
		{
			if (blacklist != null)
			{
				if (!blacklist.Contains(randomResource.randomObject))
					sum += randomResource.weighting;
			}
			else
			{
				sum += randomResource.weighting;
			}
		}
		return sum;
	}

	public RandomObject<T> GetRandomObject(List<T> blacklist = null)
	{		
		if (randomObjects.Count == 0)
		{
			return default(RandomObject<T>);
		}
		weightingSum = GetWeightingSum(blacklist);
		float rand = UnityEngine.Random.Range(0f, weightingSum);
		float runningSum = 0;
		
		foreach (var randomObject in randomObjects)
		{
			if (blacklist != null)
			{
				Debug.Log("blacklist.Contains(randomObject.randomObject): " + randomObject.randomObject.ToString() + " = " + blacklist.Contains(randomObject.randomObject));
				if (!blacklist.Contains(randomObject.randomObject))
				{
					runningSum += randomObject.weighting;
					if (rand <= runningSum)
					{
						return randomObject;
					}
				}
			}
			else
			{
				runningSum += randomObject.weighting;
				if (rand <= runningSum)
				{
					return randomObject;
				}
			}


		}
		return default(RandomObject<T>);
	}

	public RandomObject<T> GetRandomObjects<T>(int number)
	{
		List<RandomObject<T>> resources = new List<RandomObject<T>>();
		for (int i = 0; i < number; i++)
		{
			resources.Add(GetRandomObject() as RandomObject<T>);
		}
		return default(RandomObject<T>);
	}
}

