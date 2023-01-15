using UnityEngine;
using System.Collections.Generic;
using GramophoneUtils.Events.Listeners;
using Sirenix.OdinInspector;
using AYellowpaper;

namespace GramophoneUtils.Events.CustomEvents
{
    public abstract class BaseGameEvent<T> : ScriptableObject
    {
        [ShowInInspector] private List<IGameEventListener<T>> eventListeners = new List<IGameEventListener<T>>();

        [SerializeField] private List<InterfaceReference<IGameEventListener<T>>> gameListenersToAddFromScriptableObject;

        #region Callbacks

        private void OnEnable()
        {
            for (int i = 0; i < gameListenersToAddFromScriptableObject.Count; i++)
            {
                RegisterListener(gameListenersToAddFromScriptableObject[i].Value);
            }
        }

        private void OnDisable()
        {
            for (int i = 0; i < gameListenersToAddFromScriptableObject.Count; i++)
            {
                UnregisterListener(gameListenersToAddFromScriptableObject[i].Value);
            }
        }

        #endregion

        public void Raise(T item)
        {
            for(int i = eventListeners.Count - 1; i >= 0; i--)
                eventListeners[i].OnEventRaised(item);
        }

        public void RegisterListener(IGameEventListener<T> listener)
        {
            if(!eventListeners.Contains(listener))
                eventListeners.Add(listener);
        }

        public void UnregisterListener(IGameEventListener<T> listener)
        {
            if(eventListeners.Contains(listener))
                eventListeners.Remove(listener);
        }
    }
}
