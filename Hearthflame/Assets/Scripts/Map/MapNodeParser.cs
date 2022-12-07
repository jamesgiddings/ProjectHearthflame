using GramophoneUtils.Maps;
using GramophoneUtils.SavingLoading;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;

public class MapNodeParser : SerializedMonoBehaviour, ISaveable
{
    [Required]
    [SerializeField] private MapGraph mapGraph;

    [SerializeField] private Dictionary<MapBaseNode, MapBaseNodeState> _mapNodeStateDictionary;

    #region Initialisation


    private void OnEnable()
    {
        mapGraph.OnSceneNodeChanged += ChangeScene; 
    } 

    private void Start()
    {
        mapGraph.Restart();
        ParseCurrentNode();
    }

    #endregion

    #region End Of Life

    private void OnDisable()
    {

    }

    #endregion

    private void ChangeScene(SceneConnectionObject sceneConnectionObject)
    {
        sceneConnectionObject.ChangeScene();
    }

    private void ParseCurrentNode()
    {
        MapBaseNode baseNode = mapGraph.Current;
        SceneNode sceneNode = null;

        if (baseNode is SceneNode)
        {
            sceneNode = (SceneNode)baseNode;
            // Do something
        }
    }

    [Serializable]
    public class MapBaseNodeState
    {
        [SerializeField] private bool _revealed;
        [SerializeField] private bool _locked;

        public MapBaseNodeState(bool revealed, bool locked)
        {
            _revealed = revealed;
            _locked = locked;
        }
    }

    #region Saving Loading
    public object CaptureState()
    {
        throw new System.NotImplementedException();
    }

    public void RestoreState(object state)
    {
        throw new System.NotImplementedException();
    }
    #endregion
}
