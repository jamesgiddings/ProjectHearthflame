using GramophoneUtils.Magic;
using System;

public struct Damage
{
	public readonly float Value;
	public readonly Element Element;
	public readonly object source;

	public Damage(float value, Element element, object source = null)
	{
		this.Value = Math.Abs(value);
		this.Element = element;
		this.source = null;
	}
}
