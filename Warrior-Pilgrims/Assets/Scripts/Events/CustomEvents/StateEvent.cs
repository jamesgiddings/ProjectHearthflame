using UnityEngine;

namespace GramophoneUtils.Events.CustomEvents
{
    [CreateAssetMenu(fileName = "New State Event", menuName = "Game Events/State Event")]
    public class StateEvent : BaseGameEvent<State> { }
}
