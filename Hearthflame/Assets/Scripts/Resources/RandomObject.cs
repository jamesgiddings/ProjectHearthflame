using System;
using UnityEngine;

[Serializable]
public class RandomObject<T>
{
	public T randomObject;
	public float weighting;
	public RandomObject(T randomObject, float weighting = 1)
	{
		this.randomObject = randomObject;
		this.weighting = weighting;
	}
}
