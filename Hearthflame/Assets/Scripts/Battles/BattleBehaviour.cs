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
    [SerializeField] private GameObject battleRewards;
    [SerializeField] private TextMeshProUGUI battleRewardsText;

    public GameObject TurnOrderPrefab => turnOrderPrefab;
    public VoidEvent OnCharactersUpdated => onCharactersUpdated;
    public BattlerDisplayUI BattlerDisplayUI => battlerDisplayUI;
    public RadialMenu RadialMenu => radialMenu;
    public GameObject BattleRewards => battleRewards;
    public TextMeshProUGUI BattleRewardsText => battleRewardsText;
}
