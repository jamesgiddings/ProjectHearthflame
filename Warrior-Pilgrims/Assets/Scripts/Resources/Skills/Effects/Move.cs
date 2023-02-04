using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move
{
    public readonly int Value;
    public readonly bool MoveByValue;
    public readonly object Source;

    public Move(int value, bool MoveByValue, object source = null)
    {
        this.Value = value;
        this.MoveByValue = MoveByValue;
        this.Source = source;
    }

    public Move(Move move)
    {
        this.Value = move.Value;
        this.MoveByValue = move.MoveByValue;
        this.Source = move.Source;
    }
}
