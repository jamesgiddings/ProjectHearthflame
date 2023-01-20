using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Move
{
    public readonly int Value;
    public readonly bool MoveByValue;
    public readonly object source;

    public Move(int value, bool MoveByValue, object source = null)
    {
        this.Value = value;
        this.MoveByValue = MoveByValue;
        this.source = source;
    }
}
