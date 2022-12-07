using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Service Locator Object", menuName = "Systems/Service Locator Object")]
public class ServiceLocatorObject : ScriptableObject
{
    [SerializeField, Required]
    private SceneController _sceneController;
    public SceneController SceneController => _sceneController;
}
