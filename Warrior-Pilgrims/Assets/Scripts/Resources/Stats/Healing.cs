using System;

[Serializable]
public class Healing
{
	public readonly float Value;
	public readonly object Source;

	public Healing(float value, object source = null)
	{
		this.Value = Math.Abs(value);
		this.Source = source;
	}

    public Healing(Healing healing)
    {
        this.Value = Math.Abs(healing.Value);
        this.Source = healing.Source;
    }
}