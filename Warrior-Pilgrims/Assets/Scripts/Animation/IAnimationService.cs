using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public interface IAnimationService
{
    #region Attributes/Fields/Properties
    #endregion

    #region Constructors
    #endregion

    #region Callbacks
    #endregion

    #region Public Functions

    enum AnimationType { Idle, Walk, Attack, Death };

    Task PlayAnimation(AnimationType animationType);

    void CancelAnimation();

    #endregion

    #region Protected Functions
    #endregion

    #region Private Functions
    #endregion

    #region Inner Classes
    #endregion
}
