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
    [SerializeField] private BattlerDisplayUI battlerDisplayUI;
    [SerializeField] private RadialMenu radialMenu;
    [SerializeField] private BattleRewardsDisplayUI battleRewardsDisplayUI;
    [SerializeField] private Camera battleCamera;
    [SerializeField] private GameObject battleEnvironment;

    public GameObject TurnOrderPrefab => turnOrderPrefab;
    public VoidEvent OnCharactersUpdated => onCharactersUpdated;
    public BattlerDisplayUI BattlerDisplayUI => battlerDisplayUI;
    public RadialMenu RadialMenu => radialMenu;
    public BattleRewardsDisplayUI BattleRewardsDisplayUI => battleRewardsDisplayUI;
    public Camera BattleCamera => battleCamera;
    public GameObject BattleEnvironment => battleEnvironment;  
}
