using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "New Scene Connection Object", menuName = "Scene Connection")]
public class SceneConnectionObject : Resource
{
    [Required]
    [SerializeField]
    private string _triggerName;

    [Required]
    [SerializeField]
    private string _scene1Name;

    [Required]
    [SerializeField]
    private string _scene2Name;

    public string TriggerName
    {
        get
        {
            return _triggerName;
        }
        set
        {
            _triggerName = value;
        }
    }

    public string Scene1Name 
    {
        get 
        {
            return _scene1Name;
        }
        set 
        {
            _scene1Name = value;
        } 
    }
    public string Scene2Name 
    { 
        get
        {
            return _scene2Name;
        } 
        set
        {
            _scene2Name = value;
        } 
    }

    public void ChangeScene()
    {
        ServiceLocator.Instance.ServiceLocatorObject.SceneController.ChangeScene(this);
    }

    #region Utilities

#if UNITY_EDITOR
/*    [Button("Create Destination Scene Transition Object")]
    public void CreateDestinationSceneTransitionObject(String sceneName)
    {
        _origin = ScriptableObject.CreateInstance(typeof(SceneTransitionObject)) as SceneTransitionObject;
        _origin.SceneName = sceneName;
        string assetPath = AssetDatabase.GenerateUniqueAssetPath("Assets/Scenes/" + sceneName + "/" + sceneName + "SceneTransitionObject.asset");
        AssetDatabase.CreateAsset(_origin, assetPath);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }*/



    /*    [Button("Create Origin Scene Transition Object")]
        public void CreateOriginSceneTransitionObject()
        {
            _destination = ScriptableObject.CreateInstance(typeof(SceneTransitionObject)) as SceneTransitionObject;
            _destination.SceneName = _sceneName;
            string assetPath = AssetDatabase.GenerateUniqueAssetPath("Assets/Scenes/" + _sceneName + "/" + _sceneName + "SceneTransitionObject.asset");
            AssetDatabase.CreateAsset(_sceneTransitionObject, assetPath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }*/
#endif

    #endregion

}
