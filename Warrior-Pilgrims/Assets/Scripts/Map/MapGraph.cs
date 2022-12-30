using Dialogue;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using XNode;

namespace GramophoneUtils.Maps
{
    [CreateAssetMenu(menuName = "Map/Graph", order = 0)]
    public class MapGraph : NodeGraph
    {
        private MapBaseNode _current;

        private Dictionary<TransitionObject, TransitionNode> _transitionObjectToNodeDictionary;

        public Action<MapBaseNode> OnMapBaseNodeChanged;

        public Dictionary<TransitionObject, TransitionNode> TransitionObjectToNodeDictionary
        {
            get 
            { 
                if (_transitionObjectToNodeDictionary == null)
                {
                    return GetTransitionObjectNodeDictionary();
                }
                return _transitionObjectToNodeDictionary; 
            }
        }

        public MapBaseNode Current
        {
            get
            { 
                return _current;
            }
            set 
            {
                _current = value;
                OnMapBaseNodeChanged?.Invoke(_current);
            }
        }



        #region Callbacks

#if UNITY_EDITOR

        private void OnValidate()
        {
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

#endif
        #endregion

        #region API

        public void Restart()
        {
            //Find the first MapBaseNode without any inputs. This is the starting node.
            _current = nodes.Find(x => x is SceneNode && x.Inputs.All(y => !y.IsConnected)) as SceneNode;
        }

        /// <summary>
        /// Gets only the transition nodes.
        /// </summary>
        /// <returns></returns>
        public List<TransitionNode> GetTransitionNodes()
        {
            return nodes.OfType<TransitionNode>().ToList();
        }



        #endregion

        #region Utilities

        private Dictionary<TransitionObject, TransitionNode> GetTransitionObjectNodeDictionary()
        {
            Dictionary<TransitionObject, TransitionNode> transitionObjecTotNodeDictionary = new Dictionary<TransitionObject, TransitionNode>();
            foreach (TransitionNode transitionNode in GetTransitionNodes())
            {
                transitionObjecTotNodeDictionary.Add(transitionNode.TransitionObject, transitionNode);
            }
            return transitionObjecTotNodeDictionary;
        }

        #endregion


    }
}