using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GramophoneUtils.Events.CustomEvents
{
    [CreateAssetMenu(fileName = "New Character Event", menuName = "Game Events/Character Event")]
    public class CharacterEvent : BaseGameEvent<PartyCharacterTemplate> { }
}
