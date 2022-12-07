using Dialogue;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using XNode;

namespace GramophoneUtils.Maps
{
    [CreateAssetMenu(menuName = "Map/Graph", order = 0)]
    public class MapGraph : NodeGraph
    {
        private SceneNode _current;

        public Action<SceneConnectionObject> OnSceneNodeChanged;

        public SceneNode Current
        {
            get
            { 
                return _current;
            }
            set 
            {
                _current = value;
                OnSceneNodeChanged?.Invoke(_current.CurrentSceneConnectionObject);
            }
        }

        public void Restart()
        {
            //Find the first MapBaseNode without any inputs. This is the starting node.
            _current = nodes.Find(x => x is SceneNode && x.Inputs.All(y => !y.IsConnected)) as SceneNode;
        }

        public SceneNode SelectScene()
        {
            Debug.Log("i: ");
            _current.SelectScene();
            return _current;
        }


    }
}