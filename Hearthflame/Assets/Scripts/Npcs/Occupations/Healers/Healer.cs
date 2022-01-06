using GramophoneUtils.Events.CustomEvents;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GramophoneUtils.Npcs.Occupations.Healers
{
    public class Healer : MonoBehaviour, IOccupation
    {
        [SerializeField] private VoidEvent onStartHealerScenario = null;

        public string Name => "I can heal you!";
        public void Trigger(GameObject other)
        {
            onStartHealerScenario.Raise();
        }
    }
}
