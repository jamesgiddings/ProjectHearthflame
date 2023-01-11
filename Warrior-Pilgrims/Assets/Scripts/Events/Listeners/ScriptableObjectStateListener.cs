using GramophoneUtils.Events.CustomEvents;
using GramophoneUtils.Events.UnityEvents;
using UnityEngine;

namespace GramophoneUtils.Events.Listeners
{
    [CreateAssetMenu(fileName = "New Scriptable Object State Listener", menuName = "Game Events/Listeners/State Listener")]
    public class ScriptableObjectStateListener : ScriptableObjectEventListener<State, StateEvent, UnityStateEvent> { }
}