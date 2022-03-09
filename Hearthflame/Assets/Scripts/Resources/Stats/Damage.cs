using GramophoneUtils.Magic;
using System;

public struct Damage
{
	public readonly float Value;
	public readonly Element Element;
	public readonly AttackType AttackType;
	public readonly object source;

	public Damage(float value, Element element, AttackType attackType, object source = null)
	{
		this.Value = Math.Abs(value);
		this.Element = element;
		this.AttackType = attackType;
		this.source = null;
	}
}
