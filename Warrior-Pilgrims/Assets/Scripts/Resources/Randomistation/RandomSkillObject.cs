using AYellowpaper;
using Sirenix.OdinInspector;
using System;
using System.Xml.Linq;
using UnityEditor;
using UnityEngine;

[Serializable, CreateAssetMenu(fileName = "New Random Skill Object", menuName = "Skills/Random Skill Object")]
public class RandomSkillObject : SerializedScriptableObject, IRandomObject<ISkill>
{
    #region Attributes/Fields/Properties

    [SerializeField] InterfaceReference<ISkill> _weightedObject;
    public ISkill WeightedObject 
    { 
        get 
        { 
            return (ISkill)_weightedObject.Value; 
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

    public void SetWeightedObject(ISkill skill, UnityEngine.Object underlyingObject)
    {
        _weightedObject = new InterfaceReference<ISkill>();
        _weightedObject.UnderlyingValue = underlyingObject;
        _weightedObject.Value = skill;
    }

    #endregion

    #region Protected Functions
    #endregion

    #region Private Functions
    #endregion

    #region Inner Classes
    #endregion

}
