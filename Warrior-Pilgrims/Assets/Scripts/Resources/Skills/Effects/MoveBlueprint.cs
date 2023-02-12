using GramophoneUtils.Stats;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "New Move Blueprint Object", menuName = "Skills/Effects/Move Blueprint Object")]
public class MoveBlueprint : ScriptableObject, IBlueprint
{
    #region Attributes/Fields/Properties

    [SerializeField] private string _name;
    public string Name => _name;


    [SerializeField] private Sprite _sprite;
    public Sprite Sprite => _sprite;

    [SerializeField] private int _value;
    [SerializeField, Tooltip("If _moveByValue is true it moves the character forward (towards the centre) if negative, and backwards (away from the centre) if positive. If false it puts the character directly into that slot")]
    private bool _moveByValue;

    #endregion

    #region Constructors
    #endregion

    #region Callbacks
    #endregion

    #region Public Functions

    public T CreateBlueprintInstance<T>(object source = null)
    {
        Move moveInstance = new Move(_name, new Guid().ToString(), _sprite, _value, _moveByValue, source);
        return (T)Convert.ChangeType(moveInstance, typeof(T));
    }

    #endregion

    #region Protected Functions
    #endregion

    #region Private Functions
    #endregion

    #region Inner Classes
    #endregion

}
