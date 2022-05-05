using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Dialogue Bool", menuName = "Dialogue/Dialogue Bool")]
public class DialogueBool : DialogueVariable
{
    [SerializeField] private bool value;

    [SerializeField] private bool global;

    public bool Value 
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

    public void Toggle()
    {
        this.value = !value;
    }

    public bool IsTrue()
    {
        if (this.value == true)
        {
            return true;
        }
        return false;
    }
    #region SavingLoading

    [Serializable]
    public struct DialogueBoolSaveData
    {
        public bool Value;

        public DialogueBoolSaveData(bool Value)
        {
            this.Value = Value;
        }
    }

    #endregion

}