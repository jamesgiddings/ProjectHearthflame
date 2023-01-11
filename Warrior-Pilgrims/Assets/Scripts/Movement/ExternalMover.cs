using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExternalMover : MonoBehaviour
{
    #region Attributes/Fields/Properties

    [SerializeField] private CharacterController2D _characterController2D;

    private float _move = 0f;
    private bool _crouch = false;
    private bool _jump = false;

    #endregion
    #region Callbacks

    void FixedUpdate()
    {
        _characterController2D.Move(_move, _crouch, _jump);
    }

    #endregion

    #region API

    public void SetMove(float value)
    {
        _move = value;
    }

    public void SetCrouch(bool value)
    {
        _crouch = value;
    }

    public void SetJump(bool value)
    {
        _jump = value;
    }

    #endregion
}
