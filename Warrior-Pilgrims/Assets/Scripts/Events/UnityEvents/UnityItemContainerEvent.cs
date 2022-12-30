using System;
using GramophoneUtils.Items;
using UnityEngine.Events;

namespace GramophoneUtils.Events.UnityEvents
{
    [Serializable] public class UnityItemContainerEvent : UnityEvent<IItemContainer> { }
}