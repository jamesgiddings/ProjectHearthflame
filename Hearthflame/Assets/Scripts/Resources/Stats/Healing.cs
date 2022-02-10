using System;

public struct Healing
{
	public readonly float Value;
	public readonly object source;

	public Healing(float value, object source = null)
	{
		this.Value = Math.Abs(value);
		this.source = null;
	}
}