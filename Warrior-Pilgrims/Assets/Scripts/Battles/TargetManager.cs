using GramophoneUtils.Characters;
using GramophoneUtils.Utilities;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Target Manager", menuName = "Battles/Systems/Target Manager")]
public class TargetManager : ScriptableObjectThatCanRunCoroutines, ITargetManager
{
    #region Attributes/Fields/Properties

    [SerializeField] private ServiceLocatorObject _serviceLocatorObject;

    private Action<List<Character>> _onCurrentTargetsChanged;
    public Action<List<Character>> OnCurrentTargetsChanged
    {
        get
        {
            return _onCurrentTargetsChanged;

        }
        set
        {
            _onCurrentTargetsChanged = value;
        }
    }

    private ITargets _targetsObject = null;

    [ShowInInspector]
    private ISkill _currentSkill;

    private List<Character> _currentTargetsCache = new List<Character>();
    public List<Character> CurrentTargetsCache => _currentTargetsCache;

    private bool _isTargeting = false;
    [ShowInInspector]
    public bool IsTargeting => _isTargeting;

    #endregion

    #region Constructors
    #endregion

    #region Callbacks
    #endregion

    #region Public Functions

    public List<Character> ChangeTargeted(Vector2 direction)
    {
        if (_targetsObject == null)
        {
            throw new NullReferenceException("ChangeTargeted was called before GetCurrentlyTargeted. Therefore ITargets targestObject was null.");
        }
        List<Character> currentlyTargeted = _targetsObject.ChangeCurrentlyTargeted(direction);
        _currentTargetsCache = currentlyTargeted;
        _onCurrentTargetsChanged?.Invoke(currentlyTargeted);
        return currentlyTargeted;
    }

    public void ClearTargets()
    {
        _targetsObject = null;
        _currentTargetsCache.Clear();
        _onCurrentTargetsChanged?.Invoke(_currentTargetsCache);
        _currentSkill = null;
        _isTargeting = false;
    }

    public List<Character> GetAllPossibleTargets(ISkill skill, Character originator)
    {
        if (_targetsObject == null)
        {
            _targetsObject = skill.TargetToSlots.GetTargetsObject(
                originator,
                _serviceLocatorObject.CharacterModel.PlayerCharacterOrder,
                _serviceLocatorObject.CharacterModel.EnemyCharacterOrder
                );
            _currentSkill = skill;
            _isTargeting = true;
        }

        return _targetsObject.GetCurrentlyTargeted();
    }

    public List<Character> GetCurrentlyTargeted(ISkill skill, Character originator)
    {
        if (_targetsObject == null)
        {
            _targetsObject = skill.TargetToSlots.GetTargetsObject(
                originator,
                _serviceLocatorObject.CharacterModel.PlayerCharacterOrder,
                _serviceLocatorObject.CharacterModel.EnemyCharacterOrder
                );
            _currentSkill = skill;
            _isTargeting = true;
        }

        List<Character> currentlyTargeted = _targetsObject.GetCurrentlyTargeted();
        _currentTargetsCache = currentlyTargeted;
        if (currentlyTargeted.Count > 0)
        {
            _onCurrentTargetsChanged?.Invoke(currentlyTargeted);
        }

        return currentlyTargeted;
    }

    public Character GetTargetByMouse()
    {
        throw new System.NotImplementedException();
    }

    public void SubscribeToBattleDataModelOnSkillUsed()
    {
        _serviceLocatorObject.BattleDataModel.OnSkillUsed += UseSkill;
    }

    public void UnsubscribeFromBattleDataModelOnSkillUsed()
    {
        _serviceLocatorObject.BattleDataModel.OnSkillUsed -= UseSkill;
    }

    public void UseSkill(Character originator)
    {
        //TODO, the character list that is passed to this function has to be cloned, as it is set to 0 at some point after it is passed to the function.
        //Mayba it could duplicate before it sends, or pass by value (struct?)?
        _currentSkill.Use(GetCurrentlyTargeted(_currentSkill, originator), originator);
        ClearTargets();
    }

    public void SimulatePlayerTargeting(ISkill skill, List<Character> charactersToTarget, Character originator) // TODO this is just for testing
        //so i put it in a pre processor block for editor only, but then when we build the game, it no longer fulfils the interface. 
        // MAybe the test class can use the impl class?
    {
        throw new System.NotImplementedException();
    }


    #endregion

    #region Protected Functions
    #endregion

    #region Private Functions
    #endregion

    #region Inner Classes
    #endregion

}
