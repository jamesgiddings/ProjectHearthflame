using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;
using static Dialogue.Chat;
using Object = UnityEngine.Object;

namespace GramophoneUtils.Maps
{
    /// <summary>
    /// Represents an exploration scene in the game, and that scene's connection to other scenes.
    /// This class will track details about its random encounters. It will not track if it has been
    /// unlocked, as it is a scriptable object - that is tracked in the MapNodeParser, which is
    /// a monobehaviour.
    /// </summary>
    [NodeTint("#3F5A4D")]
    public class SceneNode : MapBaseNode
    {
        [Input(backingValue = ShowBackingValue.Never, connectionType = ConnectionType.Override)] public TransitionNode entryTransition;
        [Output(backingValue = ShowBackingValue.Never, connectionType = ConnectionType.Override)] public TransitionNode exitTransition;

        [SerializeField, BoxGroup("Lock and visibility status at start of game")] private bool _revealed = true;
        [SerializeField, BoxGroup("Lock and visibility status at start of game")] private bool _locked = true;

        public bool Revealed => _revealed;
        public bool Locked => _locked;

        private TransitionObject _transitionObjectReference;

        #region API
        public override void Trigger()
        {
            (graph as MapGraph).Current = this;
            if (_transitionObjectReference != null)
            {
                _transitionObjectReference.ChangeScene();
            }
        }

        public void SetRevealed(bool value, TransitionObject transitionObjectReference)
        {
            _revealed = value;
            _transitionObjectReference = transitionObjectReference; // this provides the reference to the TransitionObject which allowed this node to be accessible, and is what should be used in transitioning scenes.
            OnNodeChanged?.Invoke();
        }

        public void SetLocked(bool value)
        {
            _locked = value;
            OnNodeChanged?.Invoke();
        }

        #endregion
    }
}