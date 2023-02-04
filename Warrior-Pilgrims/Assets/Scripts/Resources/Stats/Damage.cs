using GramophoneUtils.Magic;
using System;

[Serializable]
public class Damage
{
	public readonly float Value;
	public readonly Element Element;
	public readonly AttackType AttackType;
	public readonly object Source;

	public Damage(float value, Element element, AttackType attackType, object source = null)
	{
		this.Value = Math.Abs(value);
		this.Element = element;
		this.AttackType = attackType;
		this.Source = source;
	}

    public Damage(Damage damage)
    {
        this.Value = Math.Abs(damage.Value);
        this.Element = damage.Element;
        this.AttackType = damage.AttackType;
        this.Source = damage.Source;
    }
}
