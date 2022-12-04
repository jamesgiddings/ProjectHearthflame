using GramophoneUtils.Events.CustomEvents;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class BattleBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject turnOrderPrefab;
    [SerializeField] private VoidEvent onCharactersUpdated;
    [SerializeField] private RadialMenu radialMenu;
    [SerializeField] private BattleRewardsDisplayUI battleRewardsDisplayUI;

    public GameObject TurnOrderPrefab => turnOrderPrefab;
    public VoidEvent OnCharactersUpdated => onCharactersUpdated;
    public RadialMenu RadialMenu => radialMenu;
    public BattleRewardsDisplayUI BattleRewardsDisplayUI => battleRewardsDisplayUI;
}
