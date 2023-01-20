using GramophoneUtils.Utilities;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class AnimationService : ScriptableObjectThatCanRunCoroutines
{
    #region Attributes/Fields/Properties
    #endregion

    #region Constructors
    #endregion

    #region Callbacks
    #endregion

    #region Public Functions

    public enum AnimationType { Idle, Walk, Attack, Death }

    private Dictionary<AnimationType, Task> _animations;
    private CancellationTokenSource _cancellationTokenSource;

    public AnimationService()
    {
        _animations = new Dictionary<AnimationType, Task>();
        _cancellationTokenSource = new CancellationTokenSource();
    }

    public async Task PlayAnimation(AnimationType animationType)
    {
        if (_animations.ContainsKey(animationType))
        {
            _cancellationTokenSource.Cancel();
            _cancellationTokenSource = new CancellationTokenSource();
        }

        _animations[animationType] = new Task(() =>
        {
            //Check if the task has been cancelled
            if (_cancellationTokenSource.IsCancellationRequested)
            {
                _cancellationTokenSource.Token.ThrowIfCancellationRequested();
            }
            switch (animationType)
            {
                case AnimationType.Idle:
                    // play idle animation
                    break;
                case AnimationType.Walk:
                    // play walk animation
                    break;
                case AnimationType.Attack:
                    // play attack animation
                    break;
                case AnimationType.Death:
                    // play death animation
                    break;
            }
        }, _cancellationTokenSource.Token);
        _animations[animationType].Start();
        await _animations[animationType];
    }

    public void CancelAnimation()
    {
        _cancellationTokenSource.Cancel();
    }

    #endregion

    #region Protected Functions
    #endregion

    #region Private Functions
    #endregion

    #region Inner Classes
    #endregion
}
