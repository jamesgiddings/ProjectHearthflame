using System;
using UnityEngine;

[Serializable]
public class Healing : IResource
{
	public readonly float Value;
	public readonly object Source;

    private string _name;
    public string Name => _name;

    private string _uid;
    public string UID => _uid;

    private Sprite _sprite;
    public Sprite Sprite => _sprite;

    public Healing(
        string name, 
        string uid, 
        Sprite sprite, 
        float value, 
        object source = null)
	{
        this._name = name;
        this._uid = uid;
        this._sprite = sprite;
        this.Value = Math.Abs(value);
		this.Source = source;
	}

    public Healing(Healing healing)
    {
        this._name = healing.Name;
        this._uid = healing.UID;
        this._sprite = healing.Sprite;
        this.Value = Math.Abs(healing.Value);
        this.Source = healing.Source;
    }

    public Healing(Healing healing, object source)
    {
        this._name = healing.Name;
        this._uid = healing.UID;
        this._sprite = healing.Sprite;
        this.Value = Math.Abs(healing.Value);
        this.Source = source;
    }

    public string GetInfoDisplayText()
    {
        return Value + " healing.";
    }
}