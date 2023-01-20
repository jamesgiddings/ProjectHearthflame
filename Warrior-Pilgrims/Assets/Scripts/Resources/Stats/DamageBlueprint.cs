using GramophoneUtils.Magic;
using GramophoneUtils.Stats;
using System;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "New Damage Blueprint Object", menuName = "Skills/Effects/Damage Blueprint Object")]
public class DamageBlueprint : ScriptableObject, IBlueprint
{
	[SerializeField] float value;
	[SerializeField] Element element;
	[SerializeField] AttackType attackType;
	[SerializeField] object source = null;

	public T CreateBlueprintInstance<T>(object source = null)
	{
		Damage damageInstance = new Damage(value, element, attackType, source);
		return (T)Convert.ChangeType(damageInstance, typeof(T));
	}
}
