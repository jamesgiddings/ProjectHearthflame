using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneFadeUI : MonoBehaviour
{

    #region Attributes/Fields/Properties
    
    [SerializeField, Required] private Animator _fadeAnimator;
    #endregion

    #region Callbacks

    private void OnEnable()
    {
        //PlayBlackImage();
    }

    #endregion

    #region API

    public void PlayBlackImage()
    {
        _fadeAnimator.Play("BlackImage");
    }

    [Button]
    public void PlaySceneFadeInAnimation()
    {
        _fadeAnimator.Play("SceneFadeInAnimation");
    }

    [Button]
    public void PlaySceneFadeOutAnimation()
    {
        _fadeAnimator.Play("SceneFadeOutAnimation");
    }

    [Button]
    public void SetIsLoading(bool value)
    {
        _fadeAnimator.SetBool("IsLoading", value);
    }

    #endregion
}
