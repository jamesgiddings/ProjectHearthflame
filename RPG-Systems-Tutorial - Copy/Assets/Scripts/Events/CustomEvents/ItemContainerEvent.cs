using GramophoneUtils.Items;
using UnityEngine;

namespace GramophoneUtils.Events.CustomEvents
{
    [CreateAssetMenu(fileName = "New Item Container Event", menuName = "Game Events/Item Container Event")]
    public class ItemContainerEvent : BaseGameEvent<IItemContainer> { }
}
