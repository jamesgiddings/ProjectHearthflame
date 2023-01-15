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

        public void OnEventRaised(T item)
        {
            if (unityEventResponse != null)
            {
                unityEventResponse.Invoke(item);
            }
        }
    }
}
