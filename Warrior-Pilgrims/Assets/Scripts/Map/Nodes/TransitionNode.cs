using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using XNode;

namespace GramophoneUtils.Maps
{
    public class TransitionNode : MapBaseNode
    {
        [Input(backingValue = ShowBackingValue.Never, connectionType = ConnectionType.Override)] public MapBaseNode input;
        [Output(backingValue = ShowBackingValue.Never, connectionType = ConnectionType.Override)] public MapBaseNode output;
        // Todo, the above is a bit wrong- transition nodes can connect to SceneNodes and MapJunctionNodes both ways

        [SerializeField] protected TransitionObject _transitionObject;

        protected MapBaseNode InputNode;
        protected MapBaseNode OutputNode;

        public TransitionObject TransitionObject => _transitionObject;

        public override void Trigger()
        {
            (graph as MapGraph).Current = this;
            Debug.Log("We are in the " + this.name + " node");
            MapJunctionNode mapJunctionNode = FindMapJunctionNode();
            mapJunctionNode.Trigger();
        }

        #region API

        public void RevealConnectedSceneNodes()
        {
            GetConnectedSceneNodes().ForEach((x) => x.SetRevealed(true, this.TransitionObject));
        }

        public List<SceneNode> GetConnectedSceneNodes()
        {
            List<SceneNode> connectedSceneNodes = new List<SceneNode>();
            foreach (NodePort nodePort in GetAllConnections())
            {
                if (nodePort.node is SceneNode)
                {
                    connectedSceneNodes.Add((SceneNode)nodePort.node);
                }
            }
            return connectedSceneNodes;
        }

        #endregion

        #region Utilities

        protected MapJunctionNode FindMapJunctionNode()
        {
            List<NodePort> connections = GetAllConnections();

            for (int i = 0; i < connections.Count; i++)
            {
                if (connections[i].node is MapJunctionNode)
                {
                    return connections[i].node as MapJunctionNode;
                }
            }
            Debug.LogError("No MapJunctionNode attached to TransitionNode");
            return null;
        }

        protected List<NodePort> GetAllConnections()
        {
            NodePort inputPort = GetInputPort("input");
            List<NodePort> inputConnections = inputPort.GetConnections();

            NodePort outputPort = GetOutputPort("output");
            List<NodePort> outputConnections = outputPort.GetConnections();

            inputConnections.AddRange(outputConnections);

            return inputConnections;
        }

        #endregion


#if UNITY_EDITOR

        private void OnValidate()
        {
            string inputNodeName;
            string outputNodeName;

            NodePort inputPort = GetInputPort("input");
            InputNode = ValidateConnectionAndReturnNodeIfValid(inputPort);
            inputNodeName = InputNode == null ? "" : InputNode.name;

            NodePort outputPort = GetOutputPort("output");
            OutputNode = ValidateConnectionAndReturnNodeIfValid(outputPort);
            outputNodeName = OutputNode == null ? "" : OutputNode.name;

            this.name = inputNodeName + "_" + outputNodeName;
        }

        protected MapBaseNode ValidateConnectionAndReturnNodeIfValid(NodePort nodePort)
        {
            if (nodePort.ConnectionCount == 0)
            {
                Debug.LogError("NodePort Count must be greater than 0.");
            }
            MapBaseNode nodeToValidate = (MapBaseNode)nodePort.GetConnection(0).node;

            if (nodeToValidate == null)
            {
                nodePort.GetConnection(0).Disconnect(nodePort);
                return null;
            } 

            return nodeToValidate;
        }

        [Button("Create Transition Object")]
        public virtual void CreateTransitionObject()
        {
            if (InputNode == null || OutputNode == null)
            {
                Debug.LogError("Input Node or Output Node must not be null or inherit from MapBaseNode");
                return;
            } 
            _transitionObject = ScriptableObject.CreateInstance(typeof(TransitionObject)) as TransitionObject;
            _transitionObject.Scene1Name = InputNode.SceneName;
            _transitionObject.Scene2Name = OutputNode.SceneName;
            string assetPath = AssetDatabase.GenerateUniqueAssetPath("Assets/Scenes/Transition Objects/" + this.name + "TransitionObject.asset");
            AssetDatabase.CreateAsset(_transitionObject, assetPath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

#endif
    }
}
