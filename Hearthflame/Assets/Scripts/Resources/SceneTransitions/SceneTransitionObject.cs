using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New Scene Transition Object", menuName = "Scene Transition")]
public class SceneTransitionObject : ScriptableObject
{
    [SerializeField] private string _sceneName = "SceneName";
    [SerializeField] private string _transitionName = "TransitionName";
    
    public String SceneName => _sceneName;
    public String TransitionName => _transitionName;

    public void SetSceneName(string sceneName)
    {
        _sceneName = sceneName;
    }

    public void SetTransitionName(string transitionName)
    {
        _transitionName = transitionName;
    }
}
