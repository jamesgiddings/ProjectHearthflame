using GramophoneUtils.Magic;
using GramophoneUtils.Stats;
using System;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "New Damage Blueprint Object", menuName = "Skills/Effects/Damage Blueprint Object")]
public class DamageBlueprint : ScriptableObject, IBlueprint
{
	[SerializeField] float _value;
	public float Value => _value;


	[SerializeField] Element _element;
	public Element Element;


	[SerializeField] AttackType _attackType;
	public AttackType AttackType => _attackType;


	[SerializeField] object _source = null;
	public object Source => _source;

	public T CreateBlueprintInstance<T>(object source = null)
	{
		Damage damageInstance = new Damage(_value, _element, _attackType, source);
		return (T)Convert.ChangeType(damageInstance, typeof(T));
	}
}
