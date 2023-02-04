using GramophoneUtils.Characters;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public interface IApplyable
{
    #region Attributes/Fields/Properties
    #endregion

    #region Constructors
    #endregion

    #region Callbacks
    #endregion

    #region Public Functions

    Task Apply(Character target, Character originator, CancellationTokenSource tokenSource);

    Task Remove(Character target, Character originator, CancellationTokenSource tokenSource);

    #endregion

    #region Protected Functions
    #endregion

    #region Private Functions
    #endregion

    #region Inner Classes
    #endregion
}
