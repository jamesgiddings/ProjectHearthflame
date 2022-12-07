using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;
using static Dialogue.Chat;

namespace GramophoneUtils.Maps
{
    /// <summary>
    /// Represents an exploration scene in the game, and that scene's connection to other scenes.
    /// This class will track details about its random encounters. It will not track if it has been
    /// unlocked, as it is a scriptable object - that is tracked in the MapNodeParser, which is
    /// a monobehaviour.
    /// </summary>
    [NodeTint("#CCFFCC")]
    public class SceneNode : MapBaseNode
    {
        public string SceneName = "";
        [Output(dynamicPortList = true)] public List<SceneConnectionObject> Connections = new List<SceneConnectionObject>();

        [SerializeField] private SceneConnectionObject _currentSceneConnectionObject;

        public SceneConnectionObject CurrentSceneConnectionObject => _currentSceneConnectionObject;

        public void SelectScene()
        {
            int index = 0;
            NodePort port = null;
            if (Connections.Count == 0)
            {
                port = GetOutputPort("output");
            }
            else
            {
                if (Connections.Count <= index) return;
                port = GetOutputPort("connections " + index);
            }

            if (port == null) return;
            for (int i = 0; i < port.ConnectionCount; i++)
            {
                NodePort connection = port.GetConnection(i);
                (connection.node as MapBaseNode).Trigger();
            }
        }

        public override void Trigger()
        {
            (graph as MapGraph).Current = this;
        }
    }
}