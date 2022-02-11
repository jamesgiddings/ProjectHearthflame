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
		Debug.Log("It does indeed contain it.");
		return weighting;
	}

	public float GetWeightingSum()
	{
		float sum = 0;

		foreach (RandomObject<T> randomResource in randomObjects)
		{
			sum += randomResource.weighting;
		}
		return sum;
	}

	public RandomObject<T> GetRandomObject()
	{		
		if (randomObjects.Count == 0)
		{
			return default(RandomObject<T>);
		}
		weightingSum = GetWeightingSum();
		float rand = UnityEngine.Random.Range(0f, weightingSum);
		Debug.Log("rand: " + rand);
		float runningSum = 0;
		foreach (var randomObject in randomObjects)
		{
			runningSum += randomObject.weighting;
			Debug.Log("runningSum: " + runningSum);
			if (rand <= runningSum)
			{
				Debug.Log("Adding ");
				return randomObject;
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

