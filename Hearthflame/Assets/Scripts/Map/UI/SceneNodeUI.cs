using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace GramophoneUtils.Maps
{
    public class SceneNodeUI : MonoBehaviour
    {
        private bool _revealed = false;

        private bool _locked = true;

        private SpriteRenderer _zoneSpriteRenderer;

        [SerializeField, Required] private SceneNode _sceneNode;

        [SerializeField] private List<TransitionObject> _transitionObjects;

        private Color _unrevealedColor = new Color(0.4f, 0.4f, 0.4f, 0.5f);
        private Color _lockedUnrevealedColor = new Color(0f, 0f, 0f, 1f);

        private Color _revealedColor = new Color(0.7f, 0.7f, 0.7f, 0.3f);
        private Color _lockedColor = new Color(0.1f, 0.1f, 0.1f, 0.6f);
        private Color _hoveredColor = new Color(1f, 1f, 1f, 0.5f);

        #region CallBacks

        private void Awake()
        {
            _zoneSpriteRenderer = GetComponent<SpriteRenderer>();
            if (_sceneNode == null)
            {
                this.gameObject.SetActive(false);
            }
        }

        private void OnEnable()
        {
            _sceneNode.OnNodeChanged += UpdateDisplay;
        }

        private void OnDisable()
        {
            _sceneNode.OnNodeChanged -= UpdateDisplay;
        }

        private void Start()
        {
            UpdateDisplay();
        }

        private void OnMouseOver()
        {
            if (_revealed && !_locked)
            {
                _zoneSpriteRenderer.color = _hoveredColor;
            }
        }

        private void OnMouseExit()
        {
            if (_revealed && !_locked)
            {
                _zoneSpriteRenderer.color = _revealedColor;
            }
            
        }

        private void OnMouseUp()
        {
            if (_sceneNode != null)
            {
                if (_revealed && !_locked)
                {
                    _sceneNode.Trigger();
                }
            } 
            else
            {
                Debug.LogWarning("_sceneNode is null.");
            }
        }

        #endregion

        #region API

        public void SetRevealed(bool value)
        {
            _revealed = value;
        }

        public void SetLocked(bool value)
        {
            _locked = value;
        }

        #endregion

        #region Utilities

        private void UpdateDisplay()
        {
            _locked = _sceneNode.Locked;
            _revealed = _sceneNode.Revealed;
            if (_locked && !_revealed)
            {
                Debug.Log(_sceneNode.SceneName);
                Debug.LogError("This should be triggereing with adys basement? but it isn't");
                _zoneSpriteRenderer.color = _lockedUnrevealedColor;
                return;
            }

            if (_locked && _revealed)
            {
                _zoneSpriteRenderer.color = _lockedColor;
                return;
            }
            _zoneSpriteRenderer.color = _revealed ? _revealedColor : _unrevealedColor;
        }

        #endregion
    }
}

