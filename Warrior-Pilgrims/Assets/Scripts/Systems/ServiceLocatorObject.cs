using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Service Locator Object", menuName = "Systems/Service Locator Object")]
public class ServiceLocatorObject : ScriptableObject
{
    [SerializeField, Required] private Constants _constants;
    [SerializeField, Required] private StatSystemConstants _statSystemConstants;
    [SerializeField, Required] private SceneController _sceneController;
    public Constants Constants => _constants;
    public StatSystemConstants StatSystemConstants => _statSystemConstants;
    public SceneController SceneController => _sceneController;

}
