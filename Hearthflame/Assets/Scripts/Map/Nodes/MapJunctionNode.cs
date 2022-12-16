using Sirenix.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.MemoryProfiler;
using UnityEngine;

namespace GramophoneUtils.Maps
{
    public class MapJunctionNode : MapBaseNode
    {
        [Input(backingValue = ShowBackingValue.Never)] public TransitionNode input;
        [Output(backingValue = ShowBackingValue.Never)] public TransitionNode output;

        private HashSet<SceneNode> _connectedSceneNodes;
        private HashSet<SceneNode> _traversibleSceneNodes;

        private Dictionary<SceneNode, SceneNodePathWrapper> _sceneNodeToSceneNodePath;

        

        //Todo maybe this should maintain its own list of first order connections? Rather than using static functions in MapParser

        public override void Trigger()
        {
            (graph as MapGraph).Current = this;
            Debug.Log("Triggering " + this.name);
            GetAvailableSceneNodes();
        }
        #region Utilities

#if UNITY_EDITOR

        private void OnValidate()
        {
            //GetAvailableSceneNodes();
        }

        private void GetAvailableSceneNodes()
        {
            HashSet<TransitionNode> connectedNodes = MapNodeParser.GetConnectedTransitionNodes(this);
            Debug.Log(connectedNodes.Count);
            connectedNodes.ForEach((x) => x.RevealConnectedSceneNodes());
            //HashSet<SceneNodePathWrapper> connectedSceneNodes = MapNodeParser.GetConnectedSceneNodePaths(this);
        }
#endif

        #endregion
    }
}