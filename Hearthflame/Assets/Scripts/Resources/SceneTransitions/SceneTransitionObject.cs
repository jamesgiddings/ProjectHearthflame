using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "New Scene Transition Object", menuName = "Scene Transition")]
public class SceneTransitionObject : ScriptableObject
{
    [SerializeField] private string sceneName = "SceneName";
    [SerializeField] private string transitionName = "TransitionName";
    
    public String SceneName => sceneName;
    public String TransitionName => transitionName;
}
