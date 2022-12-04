using GramophoneUtils.Stats;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This class is for the map mode of the game. A node represents a linear area that the player can explore and connects two zones on the map.
/// Once the node is marked as complete, it opens up the connection to other zones connected to this node. You can then click through to areas
/// that you have been before without going through every room, but the game will make a random encounter check for each room passed through.
/// </summary>
[RequireComponent(typeof(Button))]
public class Node : MonoBehaviour
{
    [SerializeField] private string _sceneName = "";
    [SerializeField] private List<Node> _connectedNodes;
    [SerializeField] private SceneTransitionObject _sceneTransitionObject;
    private Button _button;

    private void OnEnable()
    {
        _button = GetComponent<Button>();
    }

    public void ChangeScene()
    {
        SceneController.CacheTransitionTriggerTargetName(_sceneTransitionObject.TransitionName);
        StartCoroutine(SceneController.ChangeScene(_sceneTransitionObject.SceneName, ServiceLocator.Instance.PlayerBehaviour));
    }

#if UNITY_EDITOR
    [Button("Create Scene Transition Object")]
    public void CreateSceneTransitionObject()
    {
        _sceneTransitionObject = ScriptableObject.CreateInstance(typeof(SceneTransitionObject)) as SceneTransitionObject;
        _sceneTransitionObject.SetSceneName(_sceneName);
        string assetPath = AssetDatabase.GenerateUniqueAssetPath("Assets/Scenes/" + _sceneName + "/" + _sceneName + "SceneTransitionObject.asset");
        AssetDatabase.CreateAsset(_sceneTransitionObject, assetPath);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
#endif
}


