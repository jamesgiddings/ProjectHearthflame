using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable, CreateAssetMenu(fileName = "New Random Character Class Object", menuName = "Skills/Random Character Class Object")]

public class RandomCharacterClassObject : SerializedScriptableObject, IRandomObject<CharacterClass>
{
    #region Attributes/Fields/Properties

    [SerializeField] CharacterClass _weightedObject;
    public CharacterClass WeightedObject
    {
        get
        {
            return _weightedObject;
        }
        set
        {
            _weightedObject = value;
        }
    }

    [SerializeField] private float _weighting;
    public float Weighting
    {
        get
        {
            return _weighting;
        }
        set
        {
            _weighting = value;
        }
    }

    #endregion

    #region Constructors
    #endregion

    #region Callbacks
    #endregion

    #region Public Functions
    #endregion

    #region Protected Functions
    #endregion

    #region Private Functions
    #endregion

    #region Inner Classes
    #endregion

}
