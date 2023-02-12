using GramophoneUtils.Magic;
using GramophoneUtils.Stats;
using System;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "New Healing Blueprint Object", menuName = "Skills/Effects/Healing Blueprint Object")]
public class HealingBlueprint : ScriptableObject, IBlueprint
{
    [SerializeField] private string _name;
    public string Name => _name;


    [SerializeField] private Sprite _sprite;
    public Sprite Sprite => _sprite;


    [SerializeField] float _value;
    public float Value => _value;


	[SerializeField] private object _source = null;
	public object source => _source;

	public T CreateBlueprintInstance<T>(object source = null)
	{
		Healing healingInstance = new Healing(_name, new Guid().ToString(), _sprite, _value, source);
		return (T)Convert.ChangeType(healingInstance, typeof(T));
	}
}
