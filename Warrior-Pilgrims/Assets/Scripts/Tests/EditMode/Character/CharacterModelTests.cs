using GramophoneUtils.Characters;
using GramophoneUtils.Items.Containers;
using GramophoneUtils.Stats;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterModelTests : BasicEditModeTest
{
    #region Attributes/Fields/Properties

    private StateManager _gameStateManager;

    private State _gameBattleState;
    private State _gameLoadingState;
    private State _gameMenuState;
    private State _gameExplorationState;

    private StateManager _battleStateManager;

    private State _battleInitialisationState;
    private State _battleLostState;
    private State _battleOverState;
    private State _battleStartState;
    private State _battleWonState;
    private EnemyTurnState _enemyTurnState;
    private PlayerTurnState _playerTurnState;
    private State _preRoundState;
    private State _postRoundState;
    private State _preCharacterTurnState;
    private State _postCharacterTurnState;

    private State _testState;

    private CharacterOrder _playerCharacterOrder;
    private CharacterOrder _enemyCharacterOrder;

    private ISkill _shoot;
    private ISkill _bash;
    private ISkill _cleave;

    private Inventory _partyInventory;

    #endregion

    #region Constructors
    #endregion

    #region Callbacks
    #endregion

    #region Public Functions
    #endregion

    #region Constructors
    #endregion

    #region Callbacks
    #endregion

    #region Public Functions

    [SetUp]
    protected override void SetUp()
    {
        base.SetUp();

        _gameStateManager = ServiceLocatorObject.GameStateManager;

        _gameBattleState = ServiceLocatorObject.GameBattleState;
        _gameLoadingState = ServiceLocatorObject.GameLoadingState;
        _gameMenuState = ServiceLocatorObject.GameMenuState;
        _gameExplorationState = ServiceLocatorObject.GameExplorationState;

        _battleStateManager = ServiceLocatorObject.BattleStateManager;

        _battleInitialisationState = ServiceLocatorObject.BattleInitialisationState;
        _battleLostState = ServiceLocatorObject.BattleLostState;
        _battleOverState = ServiceLocatorObject.BattleOverState;
        _battleStartState = ServiceLocatorObject.BattleStartState;
        _battleWonState = ServiceLocatorObject.BattleWonState;
        _enemyTurnState = ServiceLocatorObject.EnemyTurnState;
        _playerTurnState = ServiceLocatorObject.PlayerTurnState;
        _preRoundState = ServiceLocatorObject.PreRoundState;
        _postRoundState = ServiceLocatorObject.PostRoundState;
        _preCharacterTurnState = ServiceLocatorObject.PreCharacterTurnState;
        _postCharacterTurnState = ServiceLocatorObject.PostCharacterTurnState;

        _testState = TestObjectReferences.TestState;

        Character1 = Character1Blueprint.Instance();
        Character1.IsPlayer = true;
        Character2 = Character2Blueprint.Instance();
        Character2.IsPlayer = true;
        Character3 = Character3Blueprint.Instance();
        Character3.IsPlayer = true;
        Character4 = Character3Blueprint.Instance();
        Character4.IsPlayer = true;

        EnemyCharacter1 = EnemyCharacter1Blueprint.Instance();
        Size2EnemyCharacter2 = Size2EnemyCharacter2Blueprint.Instance();
        EnemyCharacter3 = EnemyCharacter3Blueprint.Instance();
        Size2EnemyCharacter4 = Size2EnemyCharacter2Blueprint.Instance();

        _playerCharacterOrder = new CharacterOrder(new Character[] { Character1, Character2, Character3, Character4 });
        _enemyCharacterOrder = new CharacterOrder(new Character[] { EnemyCharacter1, Size2EnemyCharacter2, EnemyCharacter3 });

        _shoot = TestObjectReferences.Shoot;
        _cleave = TestObjectReferences.Cleave;
        _bash = TestObjectReferences.Bash;

        _partyInventory = TestObjectReferences.PartyInventory;

    }

    [Test]
    public void TestRemoveEnemyCharacter()
    {
        // Arrange
        _enemyCharacterOrder = new CharacterOrder(new Character[] { Character1 });
        CharacterModel.EnemyCharacterOrder = _enemyCharacterOrder;

        // Act
        CharacterModel.RemoveEnemyCharacter(Character1);
        
        // Assert
        Assert.IsFalse(CharacterModel.EnemyCharacters.Contains(Character1));
        Assert.IsTrue(CharacterModel.EnemyCharacters.Count == 0);
    }

/*    [Test]
    public void TestInstanceCharacters()
    {
        // Arrange
        var playerCharacterBlueprints = new Character[]
        {
            Character1Blueprint,
            Character2Blueprint
        };
        var characterModel = Substitute.For<ICharacterModel>();
        characterModel.PlayerCharacterBlueprints.Returns(playerCharacterBlueprints);
        characterModel.PartyInventory.Returns(_partyInventory);

        // Act
        var playerCharacters = characterModel.InstanceCharacters();

        // Assert
        Debug.Log("playerCharacterBlueprints:  " + playerCharacterBlueprints.Length + ", playerCharacters:  " + playerCharacters.Count);
        Assert.IsTrue(playerCharacters.Count == playerCharacterBlueprints.Length);
        foreach (var character in playerCharacters)
        {
            Assert.IsTrue(character.IsPlayer);
            Assert.IsTrue(character.PartyInventory == _partyInventory);
        }
    }*/

    [Test]
    public void TestClearDeadEnemyCharactersList()
    {
        // Arrange
        var characterModel = new CharacterModel();
        characterModel.AddEnemyToDeadEnemyCharactersList(Character1);
        characterModel.AddEnemyToDeadEnemyCharactersList(Character2);

        // Act
        characterModel.ClearDeadEnemyCharactersList();

        // Assert
        Assert.AreEqual(0, characterModel.DeadEnemyCharacters.Count);
    }

    [Test]
    public void TestClearDeadPlayerCharactersList()
    {
        // Arrange
        var characterModel = new CharacterModel();
        characterModel.AddPlayerToDeadPlayerCharactersList(Character1);
        characterModel.AddPlayerToDeadPlayerCharactersList(Character2);

        // Act
        characterModel.ClearDeadPlayerCharactersList();

        // Assert
        Assert.AreEqual(0, characterModel.DeadPlayerCharacters.Count);
    }

/*    [Test]
    public void TestRegisterCharacterDeath()
    {
        // Arrange
        var characterModel = new CharacterModel();
        var playerCharacter = new Character { IsPlayer = true };
        var enemyCharacter = new Character();
        characterModel._playerCharacters = new List<Character> { playerCharacter };
        characterModel._enemyCharacters = new List<Character> { enemyCharacter };

        // Act
        characterModel.RegisterCharacterDeath(playerCharacter);

        // Assert
        Assert.IsTrue(characterModel._unhandledCharacterDeath);
        Assert.IsTrue(characterModel._deadPlayerCharacters.Contains(playerCharacter));
        Assert.IsFalse(characterModel._playerCharacters.Contains(playerCharacter));

        // Act
        characterModel._unhandledCharacterDeath = false;
        characterModel.RegisterCharacterDeath(enemyCharacter);

        // Assert
        Assert.IsTrue(characterModel._unhandledCharacterDeath);
        Assert.IsTrue(characterModel._deadEnemyCharacters.Contains(enemyCharacter));
        Assert.IsFalse(characterModel._enemyCharacters.Contains(enemyCharacter));
    }*/


    #endregion

    #region Protected Functions
    #endregion

    #region Private Functions
    #endregion

    #region Inner Classes
    #endregion
}
