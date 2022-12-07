using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace GramophoneUtils.Maps
{
    public class SceneNodeUI : MonoBehaviour
    {
        private SpriteRenderer _zoneSpriteRenderer;

        [SerializeField, Required] private SceneNode _sceneNode;

        [SerializeField] private List<SceneConnectionObject> _sceneConnectionObjects;

        [SerializeField] private Color _revealedColor = new Color(255, 255, 255, 0f);
        [SerializeField] private Color _lockedColor = new Color(50, 50, 50, 0.5f);
        [SerializeField] private Color _hoveredColor = new Color(255, 255, 255, 0.5f);

        private void Awake()
        {
            _zoneSpriteRenderer = GetComponent<SpriteRenderer>();
        }

        // Start is called before the first frame update
        private void Start()
        {
            _zoneSpriteRenderer.color = _revealedColor;
        }

        private void OnMouseOver()
        {
            _zoneSpriteRenderer.color = _hoveredColor;
        }

        private void OnMouseExit()
        {
            _zoneSpriteRenderer.color = _revealedColor;
        }

        private void OnMouseUp()
        {
            if (_sceneNode != null)
            {
                _sceneNode.Trigger();
            } 
            else
            {
                Debug.LogWarning("_sceneNode is null.");
            }
        }
    }
}

