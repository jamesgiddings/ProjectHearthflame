using UnityEngine;

public class Move : IResource
{
    public readonly int Value;
    public readonly bool MoveByValue;
    public readonly object Source;

    private string _name;
    public string Name => _name;

    private string _uid;
    public string UID => _uid;

    private Sprite _sprite;
    public Sprite Sprite => _sprite;

    public Move(
        string name,
        string uid,
        Sprite sprite,
        int value, 
        bool MoveByValue, 
        object source = null)
    {
        this._name = name;
        this._uid = uid;
        this._sprite = sprite;
        this.Value = value;
        this.MoveByValue = MoveByValue;
        this.Source = source;
    }

    public Move(Move move)
    {
        this._name = move.Name;
        this._uid = move.UID;
        this._sprite = move.Sprite;
        this.Value = move.Value;
        this.MoveByValue = move.MoveByValue;
        this.Source = move.Source;
    }

    public string GetInfoDisplayText()
    {
        string direction = MoveByValue ? Value > 0 ? "Forward " : "Backward " : "To Slot ";
        return MoveByValue + Value.ToString();
    }
}
