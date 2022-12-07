using Sirenix.OdinInspector;
using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New Scene Transition Object", menuName = "Scene Transition")]
public class SceneTransitionObject : Resource
{
    [Required]
    [SerializeField] 
    private string _triggerName;

    [Required]
    [SerializeField]
    private string _sceneName;

    [Required]
    [SerializeField]
    private string _transitionName;

    public string TriggerName => _triggerName;

    public string SceneName
    { 
        get
        {
            return _sceneName;
        }
        set
        {
            _sceneName = value;
        }
    }

    public string TransitionName
    { 
        get
        {
            return _transitionName;
        }
        set
        {
            _transitionName = value;
        }
    }
}
