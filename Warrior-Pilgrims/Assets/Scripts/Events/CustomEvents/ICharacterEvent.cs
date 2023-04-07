using GramophoneUtils.Characters;
using UnityEngine;

namespace GramophoneUtils.Events.CustomEvents
{
    [CreateAssetMenu(fileName = "New ICharacter Event", menuName = "Game Events/Character Event")]
    public class ICharacterEvent : BaseGameEvent<ICharacter> { }
}
