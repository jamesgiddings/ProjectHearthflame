using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using XNode;

namespace GramophoneUtils.Maps
{
    public class MapEntryNode : TransitionNode
    {
        public override void Trigger()
        {
            base.Trigger();
        }

#if UNITY_EDITOR

        private void OnValidate()
        {
            NodePort inputPort = GetInputPort("input");

            if (inputPort.ConnectionCount > 0)
            {
                Debug.LogWarning("MapEntryNodes should have no input connections");
                for (int i = 0; i < inputPort.ConnectionCount; i++)
                {
                    inputPort.GetConnection(i).Disconnect(inputPort);
                }
            }

            NodePort outputPort = GetOutputPort("output");
            OutputNode = ValidateConnectionAndReturnNodeIfValid(outputPort);
        }

        [Button("Create Transition Object")]
        public override void CreateTransitionObject()
        {
            if (SceneName == null || SceneName == "" || OutputNode == null)
            {
                Debug.LogError("Input Node or Output Node must not be null or inherit from MapBaseNode");
                return;
            }
            _transitionObject = ScriptableObject.CreateInstance(typeof(TransitionObject)) as TransitionObject;
            _transitionObject.Scene1Name = SceneName;
            _transitionObject.Scene2Name = OutputNode.SceneName;
            string assetPath = AssetDatabase.GenerateUniqueAssetPath("Assets/Scenes/Transition Objects/" + OutputNode.SceneName + "_" + this.name + "_TransitionObject.asset");
            AssetDatabase.CreateAsset(_transitionObject, assetPath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
#endif
    }

}