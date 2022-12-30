using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Dialogue Int", menuName = "Dialogue/Dialogue Int")]
public class DialogueInt : DialogueVariable
{
    [SerializeField] private int value;

    [SerializeField] private bool global;

    public int Value
    {
        get
        {
            return value;
        }

        set
        {
            this.value = value;
        }
    }

    public void Increment(int value)
    {
        this.value += value;
    }

    public bool GreaterThan(int value)
    {
        if (this.value > value)
        {
            return true;
        }
        return false;
    }


    public bool LessThan(int value)
    {
        if (this.value < value)
        {
            return true;
        }
        return false;
    }

    public bool GreaterThanOrEqualTo(int value)
    {
        if (this.value > value || this.value == value)
        {
            return true;
        }
        return false;
    }

    public bool LessThanOrEqualTo(int value)
    {
        if (this.value < value || this.value == value)
        {
            return true;
        }
        return false;
    }

    public bool Equals(int value)
    {
        if (this.value == value)
        {
            return true;
        }
        return false;
    }

    [Serializable]
    public struct DialogueIntSaveData
    {
        public int Value;

        public DialogueIntSaveData(int Value)
        {
            this.Value = Value;
        }
    }
}
