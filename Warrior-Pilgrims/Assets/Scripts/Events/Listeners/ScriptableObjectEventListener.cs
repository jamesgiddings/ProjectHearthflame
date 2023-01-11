using GramophoneUtils.Events.CustomEvents;
using UnityEngine;
using UnityEngine.Events;

namespace GramophoneUtils.Events.Listeners
{
    public class ScriptableObjectEventListener<T, E, UER> : ScriptableObject,
        IGameEventListener<T> where E : BaseGameEvent<T> where UER : UnityEvent<T>
    {
        [SerializeField] private E gameEvent = null;
        public E GameEvent { get { return gameEvent; } set { gameEvent = value; } }

        [SerializeField] private UER unityEventResponse = null;

#if UNITY_EDITOR

        private void OnValidate()
        {
            if (gameEvent == null) 
            { 
                return; 
            }
            GameEvent.RegisterListener(this);
        }
        
#endif

        private void OnEnable()
        {
            if (gameEvent == null) 
            { 
                return; 
            }

            GameEvent.RegisterListener(this);
        }

        private void OnDisable()
        {
            if (gameEvent == null) return;

            GameEvent.UnregisterListener(this);
        }

        public void OnEventRaised(T item)
        {
            if (unityEventResponse != null)
            {
                unityEventResponse.Invoke(item);
            }
        }
    }
}
