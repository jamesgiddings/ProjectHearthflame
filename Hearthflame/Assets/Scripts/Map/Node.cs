using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
namespace GramophoneUtils.Deprecated
{
    /// <summary>
    /// This class is for the map mode of the game. A node represents a linear area that the player can explore and connects two zones on the map.
    /// Once the node is marked as complete, it opens up the connection to other zones connected to this node. You can then click through to areas
    /// that you have been before without going through every room, but the game will make a random encounter check for each room passed through.
    /// </summary>
    [RequireComponent(typeof(Button))]
    public class Node : MonoBehaviour
    {
        [SerializeField] private string _sceneName = "";
        [SerializeField] private List<Node> _connectedNodes;
        private Button _button;

        private void OnEnable()
        {
            _button = GetComponent<Button>();
        }


#if UNITY_EDITOR

#endif
    }
}



