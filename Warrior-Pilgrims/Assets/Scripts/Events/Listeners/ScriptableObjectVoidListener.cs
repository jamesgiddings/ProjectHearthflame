using GramophoneUtils.Events.CustomEvents;
using GramophoneUtils.Events.UnityEvents;
using UnityEngine;

namespace GramophoneUtils.Events.Listeners
{
    [CreateAssetMenu(fileName = "New Scriptable Object Void Listener", menuName = "Game Events/Listeners/Void Listener")]
    public class ScriptableObjectVoidListener : ScriptableObjectEventListener<Void, VoidEvent, UnityVoidEvent> { }
}