using GramophoneUtils.Magic;
using System;
using UnityEngine;

[Serializable]
public class Damage : IResource
{
	public readonly float Value;
	public readonly Element Element;
	public readonly AttackType AttackType;
	public readonly object Source;

	private string _name;
	public string Name => _name;

	private string _uid;
	public string UID => _uid;

	private Sprite _sprite;
	public Sprite Sprite => _sprite;

	public Damage(
        string name,
        string uid,
        Sprite sprite,
        float value, 
		Element element, 
		AttackType attackType, 
		object source = null)
	{
		this._name = name;
		this._uid = uid;
		this._sprite = sprite;
		this.Value = Math.Abs(value);
		this.Element = element;
		this.AttackType = attackType;
		this.Source = source;
	}

    public Damage(Damage damage)
    {
        this._name = damage.Name;
        this._uid = damage.UID;
        this._sprite = damage.Sprite;
        this.Value = Math.Abs(damage.Value);
        this.Element = damage.Element;
        this.AttackType = damage.AttackType;
        this.Source = damage.Source;
    }

	public string GetInfoDisplayText()
	{
		return Value + " " + Element.Name + " damage.";
    }
}
