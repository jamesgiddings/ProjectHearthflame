using GramophoneUtils.Events.CustomEvents;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    [SerializeField] private CharacterController2D _characterController2D;
    [SerializeField] private float _moveSpeed = 30f;

    [SerializeField] private bool _isFollower;
    public bool IsFollower => _isFollower;

    [ShowIf("_isFollower"), PropertyTooltip("This is the character to follow.")]
    [SerializeField] private CharacterMovement _followee;
    public CharacterMovement Followee => _followee;

    [ShowIf("_isFollower"), PropertyTooltip("This is the distance the character will follow at.")]
    [SerializeField] private float _gap;
    [SerializeField] private float _explorationGapTolerance;
    [SerializeField] private float _battleGapTolerance;
    private float _tolerance;

    [BoxGroup("Movement By State")]
    [SerializeField] private StateManager _gameStateManager;
    [BoxGroup("Movement By State")]
    [SerializeField] private State _explorationState;
    [BoxGroup("Movement By State")]
    [SerializeField] private StateEvent _enterExplorationState;
    [BoxGroup("Movement By State")]
    [SerializeField] private StateEvent _enterBattleState;

    private float _speedAdjustmentCoefficientToMaintainGap = 8f;

    private float _horizontalMove = 0f;
    private bool _jump;
/*
    private Vector3 _targetPosition = Vector3.zero;*/

    #region Callbacks

    private void OnEnable()
    {
        if (_gameStateManager == null)
        {
            return;
        }

        if (_gameStateManager.State != null)
        {
            if (_gameStateManager.State == _explorationState)
            {
                SetIsEnabled(true);
            }
        }
    }

    private void OnDisable()
    {
        SetIsEnabled(false);
    }

    void Start()
    {
        if (!_isFollower || _followee == null)
        {
            return;
        }
        _moveSpeed = _followee.GetMoveSpeed();
    }

    void FixedUpdate()
    {
        _tolerance = ServiceLocator.Instance.GameStateManager.State == ServiceLocator.Instance.ExplorationState ? _explorationGapTolerance : _battleGapTolerance;

        UpdateFunction();
        if (!_isFollower && ServiceLocator.Instance.GameStateManager.State == ServiceLocator.Instance.ExplorationState)
        {
            _horizontalMove = Input.GetAxisRaw("Horizontal") * _moveSpeed;

            if (Input.GetButtonDown("Jump"))
            {
                _jump = true;
            }
            return;
        }
        else if (_followee != null)
        {
/*            _targetPosition = transform.position;
            if (_followee.gameObject.GetComponent<CharacterController2D>().IsFacingRight)
            {
                _targetPosition = _followee.transform.position - new Vector3(_gap, 0f);
            }
            else
            {
                _targetPosition = _followee.transform.position + new Vector3(_gap, 0f);
            }*/

            float distance = Math.Abs(transform.position.x - _followee.GetTransform().position.x);
            if (distance > _gap + _tolerance)
            {
                float speedAdjustment = Math.Max(distance - _gap, 0f);

                float x = _followee.GetTransform().position.x - transform.position.x;
                float y = _followee.GetTransform().position.y - transform.position.y;

                float hyp = (float)Math.Sqrt(x * x + y * y);
                x /= hyp;
                _horizontalMove = x * (_moveSpeed + (speedAdjustment * _speedAdjustmentCoefficientToMaintainGap));
            }
            else if (distance < _gap - _tolerance && ServiceLocator.Instance.GameStateManager.State == ServiceLocator.Instance.BattleState)
            {
                float speedAdjustment = Math.Max(_gap - distance, 0f);

                float x = _followee.GetTransform().position.x - transform.position.x;
                float y = _followee.GetTransform().position.y - transform.position.y;

                float hyp = (float)Math.Sqrt(x * x + y * y);
                x /= hyp;

                _horizontalMove = -x * (_moveSpeed + (speedAdjustment * _speedAdjustmentCoefficientToMaintainGap));
            }
            else
            {
                if (distance > _gap + (_tolerance / 2)) // turn back to face the followee
                {
                    _horizontalMove = -2f;
                } 
                else if (distance < _gap + (_tolerance / 2))
                {
                    _horizontalMove = 2f;
                }
                _horizontalMove = 0f;
            }
            return;
        }
        else // don't move
        {
            _horizontalMove = 0f; // stop the characters motion
        }
    }


    private void UpdateFunction()
    {
        if (_isFollower && _followee == null)
        {
            return;
        }

        _characterController2D.Move(_horizontalMove * Time.fixedDeltaTime, false, _jump);
/*
        if (!_isFollower)
        {
            _characterController2D.Move(_horizontalMove * Time.fixedDeltaTime, false, _jump);
        }
        if (_isFollower)
        {
            if (_horizontalMove != 0f)
            {
                _characterController2D.Follow(_targetPosition, _horizontalMove * Time.fixedDeltaTime);
            }
        }*/

        _jump = false;
    }

    #endregion

    #region API

    public float GetMoveSpeed()
    {
        return _moveSpeed;
    }

    public Transform GetTransform()
    {
        return transform;
    }

    public void SetFollowee(CharacterMovement followable)
    {
        if (!_isFollower)
        {
            Debug.LogWarning("This is a leader, and should not set a follower");
            return;
        }
        _followee = followable;
    }

    public void SetGap(float gap)
    {
        if (!_isFollower)
        {
            Debug.LogWarning("This is a leader, and should not set a gap");
        }
        _gap = gap;
    }

    public void SetIsFollower(bool value)
    {
        _isFollower = value;
    }

    public void SetIsEnabled(bool value)
    {
        this.enabled = value;
    }

    #endregion

    #region Utilities

    #endregion
}
