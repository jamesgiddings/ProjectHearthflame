using GramophoneUtils.Events.CustomEvents;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BattleBehaviour : MonoBehaviour
{
    [SerializeField] GameObject turnOrderPrefab;
    [SerializeField] VoidEvent onCharactersUpdated;
    [SerializeField] BattlerDisplayUI battlerDisplayUI;

    public GameObject TurnOrderPrefab => turnOrderPrefab;
    public VoidEvent OnCharactersUpdated => onCharactersUpdated;
    public BattlerDisplayUI BattlerDisplayUI => battlerDisplayUI;
}
