using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System;
using UnityEditor;
using UnityEngine;
using XNode;
using Object = UnityEngine.Object;

namespace GramophoneUtils.Maps
{
    [NodeWidth(500)]
    public abstract class MapBaseNode : Node
    {
        [SerializeField] private string _sceneName;

        [HideInInspector] public Action OnNodeChanged;

        public string SceneName => _sceneName;

        abstract public void Trigger();

        public override object GetValue(NodePort port)
        {
            return null;
        }
    }
}