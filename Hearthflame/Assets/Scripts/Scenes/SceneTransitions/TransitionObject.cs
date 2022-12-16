using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "New Transition Object", menuName = "Transition Object")]
public class TransitionObject : ScriptableObject
{
    [SerializeField]
    private string _scene1Name;

    [SerializeField]
    private string _scene2Name;

    public string Scene1Name
    {
        get { return _scene1Name; }
        set { _scene1Name = value; }
    }

    public string Scene2Name
    {
        get { return _scene2Name; }
        set { _scene2Name = value; }
    }

    private string _destinationSceneName;
    private string _originSceneName;

    public string OriginSceneName
    {
        get
        {
            return _originSceneName;
        }
        set
        {
            _originSceneName = value;
        }
    }

    public string DestinationSceneName
    {
        get
        {
            return _destinationSceneName;
        }
        set
        {
            _destinationSceneName = value;
        }
    }

    #region API
    public void ChangeScene()
    {
        _destinationSceneName = ServiceLocator.Instance.ServiceLocatorObject.SceneController.GetDestinationSceneName(this);
        _originSceneName = _scene1Name == _destinationSceneName ? _originSceneName : _destinationSceneName;
        ServiceLocator.Instance.ServiceLocatorObject.SceneController.ChangeScene(this);
    }
    #endregion
}
