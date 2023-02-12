using GramophoneUtils.Magic;
using GramophoneUtils.Stats;
using System;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "New Damage Blueprint Object", menuName = "Skills/Effects/Damage Blueprint Object")]
public class DamageBlueprint : ScriptableObject, IBlueprint
{
    [SerializeField] private string _name;
    public string Name => _name;


    [SerializeField] private Sprite _sprite;
    public Sprite Sprite => _sprite;


    [SerializeField] float _value;
	public float Value => _value;


	[SerializeField] private Element _element;
	public Element Element => _element;


	[SerializeField] AttackType _attackType;
	public AttackType AttackType => _attackType;


	[SerializeField] object _source = null;
	public object Source => _source;

	public T CreateBlueprintInstance<T>(object source = null)
	{
		Damage damageInstance = new Damage(_name, new Guid().ToString(), _sprite, _value, _element, _attackType, source);
		return (T)Convert.ChangeType(damageInstance, typeof(T));
	}
}
