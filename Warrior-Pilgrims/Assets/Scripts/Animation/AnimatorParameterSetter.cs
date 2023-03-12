using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorParameterSetter : MonoBehaviour
{
    #region Attributes/Fields/Properties

    [SerializeField] private string parameterName = "Speed";

    [SerializeField] private float minSpeed = 0.5f;
    [SerializeField] private float maxSpeed = 1.5f;

    private Animator animator;

    #endregion

    #region Constructors
    #endregion

    #region Callbacks

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        animator.speed = Random.Range(minSpeed, maxSpeed);
    }

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
