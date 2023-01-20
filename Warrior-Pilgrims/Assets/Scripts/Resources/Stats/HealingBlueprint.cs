using GramophoneUtils.Magic;
using GramophoneUtils.Stats;
using System;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "New Healing Blueprint Object", menuName = "Skills/Effects/Healing Blueprint Object")]
public class HealingBlueprint : ScriptableObject, IBlueprint
{
	[SerializeField] float value;
	[SerializeField] object source = null;

	public T CreateBlueprintInstance<T>(object source = null)
	{
		Healing healingInstance = new Healing(value, source);
		return (T)Convert.ChangeType(healingInstance, typeof(T));
	}
}
