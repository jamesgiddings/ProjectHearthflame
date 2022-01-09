using GramophoneUtils.Items;
using UnityEngine;

namespace GramophoneUtils.Events.CustomEvents
{
    [CreateAssetMenu(fileName = "New Resource Event", menuName = "Game Events/Resource Event")]
    public class ResourceEvent : BaseGameEvent<Resource> { }
}
