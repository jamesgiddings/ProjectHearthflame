using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class PlayerInputBehaviour : MonoBehaviour
{
    #region Attributes/Fields/Properties

    [SerializeField] private PlayerInputObject _playerInputObject;

    private PlayerInput _playerInput;

    private InputAction _acceptAction;
    public InputAction AcceptAction => _acceptAction;

    private InputAction _cancelAction;
    public InputAction CancelAction => _cancelAction;

    #endregion

    #region Callbacks

    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
        _acceptAction = _playerInput.actions["Accept"];
        _cancelAction = _playerInput.actions["Cancel"];
    }

    #endregion

    #region Utilities

    public void Accept(InputAction.CallbackContext context)
    {
        Debug.Log("Accept");
        context.ReadValueAsButton();
    }

    #endregion
}
