using GramophoneUtils.Items.Containers;
using GramophoneUtils.Stats;
using System.Collections.Generic;
using System;
using UnityEngine;

public interface ICharacter : IResource
{
    public string Description { get; }

    public CharacterClass CharacterClass { get; }

    public StatTemplate Stats { get; }

    public bool StartsUnlocked { get; }

    public int StartingLevel { get; }

    public GameObject CharacterPrefab { get; }

    public IBrain Brain { get; }

    Sprite Portrait { get; }

    Color Color { get; }

    StatSystem StatSystem { get; }

    HealthSystem HealthSystem { get; }

    LevelSystem LevelSystem { get; }

    SkillSystem SkillSystem { get; }

    EquipmentInventory EquipmentInventory { get; set; }

    bool IsRear { get; set; }

    bool IsPlayer { get; set; }

    bool IsUnlocked { get; set; }

    bool IsCurrentActor { get; }

    Inventory PartyInventory { get; set; }

    Dictionary<string, IStatType> StatTypeStringRefDictionary { get; }

    Action<ICharacter> OnCharacterTurnElapsed { get; set; }

    ICharacter Instance();

    BattlerNotificationImpl DequeueBattlerNoticiation();

    bool GetIsAnyNotificationInQueue();

    void SetIsCurrentActor(bool value);

    TargetAreaFlag GetTargetAreaFlag(bool IsOriginatorPlayer);

    TargetTypeFlag GetTargetTypeFlag();

    void AdvanceCharacterTurn();
}
