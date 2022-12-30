using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] TransitionObject _transitionObject;
    
    public void ChangeScene()
    {
        _transitionObject.ChangeScene();
    }
}
