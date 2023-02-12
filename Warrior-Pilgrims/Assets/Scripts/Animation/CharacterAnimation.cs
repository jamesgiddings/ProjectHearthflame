using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimation : MonoBehaviour
{
    private Animator _animator;

    private CharacterController2D _characterController2D;

    #region Callbacks


    private void Start()
    {
        _animator = gameObject.GetComponent<Animator>();
        _characterController2D = gameObject.GetComponent<CharacterController2D>();
    }

    private void Update()
    {
        Vector2 normalizedVelocity = _characterController2D.GetVelocity().normalized;

        if (_animator == null)
        {
            return;
        }

        if (ServiceLocator.Instance.GameStateManager.State == ServiceLocator.Instance.ExplorationState) // TODO, hack
        {
            _animator.SetFloat("Horizontal", normalizedVelocity.x);
            _animator.SetFloat("Vertical", normalizedVelocity.y);
            _animator.SetFloat("Speed", _characterController2D.GetVelocity().sqrMagnitude);

            if (normalizedVelocity.x == 1 || normalizedVelocity.x == -1 || normalizedVelocity.y == 1 || normalizedVelocity.y == -1)
            {
                _animator.SetFloat("LastX", normalizedVelocity.x);
                _animator.SetFloat("LastY", normalizedVelocity.y);
            }
        } else
        {
            if (_animator.parameterCount == 0)
            {
                return;
            }
            _animator.SetFloat("Horizontal", 0f);
            _animator.SetFloat("Vertical", 0f);
        }
    }
    #endregion

    #region Public Functions

    public void SetLastX(int direction)
    {
        _animator.SetFloat("LastX", direction);
    }

    public void SetLastY(int direction)
    {
        _animator.SetFloat("LastY", direction);
    }

    #endregion
}
