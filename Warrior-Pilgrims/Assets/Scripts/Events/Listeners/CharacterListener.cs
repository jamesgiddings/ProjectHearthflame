using GramophoneUtils.Characters;
using GramophoneUtils.Events.CustomEvents;
using GramophoneUtils.Events.UnityEvents;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GramophoneUtils.Events.Listeners
{
    public class CharacterListener : BaseGameEventListener<Character, CharacterEvent, UnityCharacterEvent> { }
}