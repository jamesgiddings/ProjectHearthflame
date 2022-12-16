using GramophoneUtils.Maps;
using GramophoneUtils.SavingLoading;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System;
using System.Collections.Generic;
using UnityEngine;
using XNode;

public class MapNodeParser : SerializedMonoBehaviour, ISaveable
{
    [SerializeField, Required] private MapGraph _mapGraph;

    [SerializeField] private Dictionary<MapBaseNode, MapBaseNodeState> _mapNodeStateDictionary;

    [OdinSerialize, SerializeField] public Dictionary<TransitionObject, TransitionNode> TransitionObjectNodeDictionary => _mapGraph.TransitionObjectToNodeDictionary;

    #region Initialisation


    private void OnEnable()
    {
       

    } 

    private void Start()
    {
        /*        _mapGraph.Restart();
                ParseCurrentNode();*/
        TransitionObject cachedTransitionObject = ServiceLocator.Instance.ServiceLocatorObject.SceneController.CachedTransitionObject();
        _mapGraph.Current = _mapGraph.TransitionObjectToNodeDictionary[cachedTransitionObject];
        _mapGraph.Current.Trigger();
    }

    #endregion

    #region End Of Life

    private void OnDisable()
    {

    }

    #endregion

    #region API

    /// <summary>
    /// Not yet implemented.
    /// This will provide a set of paths (i.e. collections of scene nodes 
    /// between origin and target) for each node connected to the origin.
    /// </summary>
    /// <param name="originSceneNode"></param>
    /// <returns></returns>
    public static HashSet<SceneNodePathWrapper> GetConnectedSceneNodePaths(MapJunctionNode mapJunctionNode)
    {
        HashSet<TransitionNode> connectedTransitionNodes = GetConnectedTransitionNodes(mapJunctionNode);

        throw new NotImplementedException();
    }

    /// <summary>
    /// Returns a set of SceneNodes which are connected at depth 1 to the origin
    /// </summary>
    /// <param name="mapJunctionNode"></param>
    /// <returns></returns>
    public static HashSet<TransitionNode> GetConnectedTransitionNodes(MapJunctionNode mapJunctionNode)
    {
        HashSet<TransitionNode> connectedTransitionNodes = new HashSet<TransitionNode>();

        NodePort[] connections = CreateConnectionsArray(mapJunctionNode);
        Debug.Log("connections.lenght: " + connections.Length);
        for (int i = 0; i < connections.Length; i++)
        {
            if (connections[i].node is TransitionNode)
            {
                connectedTransitionNodes.Add(connections[i].node as TransitionNode);
            }
        }
        return connectedTransitionNodes;
    }



    #endregion
    private void ChangeScene(TransitionObject transitionObject)
    {
        transitionObject.ChangeScene();
    }

    private void ParseCurrentNode()
    {
        MapBaseNode baseNode = _mapGraph.Current;
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

    #region Utilities

    private static NodePort[] CreateConnectionsArray(MapJunctionNode mapJunctionNode)
    {
        NodePort inputPort = mapJunctionNode.GetInputPort("input");
        List<NodePort> inputConnections = inputPort.GetConnections();

        NodePort outputPort = mapJunctionNode.GetOutputPort("output");
        List<NodePort> outputConnections = outputPort.GetConnections();

        inputConnections.AddRange(outputConnections);

        return inputConnections.ToArray();
    }

    #endregion

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
