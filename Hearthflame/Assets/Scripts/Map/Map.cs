using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

namespace GramophoneUtils.Maps
{
    [CreateAssetMenu(menuName = "Map/Graph", order = 0)]
    public class MapGraph : NodeGraph
    {
        private Node[] _nodes;
        private string _currentSceneName;

        [SerializeField] private Node _entryNode;

        [ShowInInspector] public Node[] Nodes => _nodes;

        private void OnValidate()
        {
            _nodes = FindObjectsOfType<Node>();
        }
    }
}