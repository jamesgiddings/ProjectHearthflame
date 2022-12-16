using GramophoneUtils.Events.CustomEvents;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class BattleBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject _rearBattlePrefab;
    [SerializeField] private GameObject _turnOrderPrefab;
    [SerializeField] private VoidEvent _onCharactersUpdated;
    [SerializeField] private RadialMenu _radialMenu;
    [SerializeField] private BattleRewardsDisplayUI _battleRewardsDisplayUI;

    public GameObject RearBattlePrefab => _rearBattlePrefab;
    public GameObject TurnOrderPrefab => _turnOrderPrefab;
    public VoidEvent OnCharactersUpdated => _onCharactersUpdated;
    public RadialMenu RadialMenu => _radialMenu;
    public BattleRewardsDisplayUI BattleRewardsDisplayUI => _battleRewardsDisplayUI;
}
