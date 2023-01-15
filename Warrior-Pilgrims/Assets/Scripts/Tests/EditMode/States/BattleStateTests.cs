using GramophoneUtils.Characters;
using GramophoneUtils.Stats;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using UnityEngine;

public class BattleStateTests : BasicEditModeTest
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
    }

    [Test]
    public void AssertGameStateManagerNotNullTest()
    {
        Assert.NotNull(_gameStateManager);
    }

    [Test]
    public void AssertBattleStateManagerNotNullTest()
    {
        Assert.NotNull(_battleStateManager);
    }

    [Test]
    public void Assert_BattleStateManagerInitialisesWithBattleInitialisationState()
    {
        _gameStateManager.ChangeState(_gameBattleState);
        Assert.NotNull(_battleStateManager.State);
        Assert.AreEqual(_gameBattleState, _gameStateManager.State);
        Assert.AreEqual(_battleInitialisationState, _battleStateManager.State);
        Assert.True(_battleInitialisationState.IsActive());

        _gameStateManager.ChangeState(_testState);
    }

    [Test]
    public void Assert_BattleStateManagerCanChangeIntoBattleStartStateAndItIsActive()
    {
        _gameStateManager.ChangeState(_gameBattleState);
        Assert.NotNull(_battleStateManager.State);
        Assert.AreEqual(_gameBattleState, _gameStateManager.State);
        Assert.AreEqual(_battleInitialisationState, _battleStateManager.State);
        Assert.True(_battleInitialisationState.IsActive());
        _battleStateManager.ChangeState(_battleStartState);
        Assert.AreEqual(_battleStartState, _battleStateManager.State);
        Assert.True(_battleStartState.IsActive());

        _gameStateManager.ChangeState(_testState);
    }

    [Test]
    public void Assert_BattleStateManagerCanChangeIntoBattlePreRoundStateItIsActive()
    {
        _gameStateManager.ChangeState(_gameBattleState);
        Assert.NotNull(_battleStateManager.State);
        Assert.AreEqual(_gameBattleState, _gameStateManager.State);
        Assert.AreEqual(_battleInitialisationState, _battleStateManager.State);
        Assert.True(_battleInitialisationState.IsActive());
        _battleStateManager.ChangeState(_battleStartState);
        Assert.AreEqual(_battleStartState, _battleStateManager.State);
        Assert.True(_battleStartState.IsActive());
        _battleStateManager.ChangeState(_preRoundState);
        Assert.AreEqual(_preRoundState, _battleStateManager.State);
        Assert.True(_preRoundState.IsActive());

        _gameStateManager.ChangeState(_testState);
    }

/*    [Test, Timeout(1000)]
    public async void Assert_BattleStateManagerCanChangeIntoBattlePreCharacterTurnStateItIsActive()
    {
        _gameStateManager.ChangeState(_gameBattleState);
        Assert.NotNull(_battleStateManager.State);
        Assert.AreEqual(_gameBattleState, _gameStateManager.State);
        Assert.AreEqual(_battleInitialisationState, _battleStateManager.State);
        Assert.True(_battleInitialisationState.IsActive());
        _battleStateManager.ChangeState(_battleStartState);
        Assert.AreEqual(_battleStartState, _battleStateManager.State);
        Assert.True(_battleStartState.IsActive());
        _battleStateManager.ChangeState(_preRoundState);
        Assert.AreEqual(_preRoundState, _battleStateManager.State);
        Assert.True(_preRoundState.IsActive());
        _battleStateManager.ChangeState(_preCharacterTurnState);
        Assert.AreEqual(_preCharacterTurnState, _battleStateManager.State);
        Assert.True(_preCharacterTurnState.IsActive());

        Func<bool> isState = () => _battleStateManager.State == _playerTurnState;

        bool isCorrectState = await Task.Run(() => LoopUntil(isState));
        Assert.True(isCorrectState);

        _gameStateManager.ChangeState(_testState);
    }

    [Test, Timeout(1000)]
    public async void Assert_BattleStateManagerChangesIntoPlayerTurnStateAndItIsActive()
    {
        _gameStateManager.ChangeState(_gameBattleState);
        Assert.NotNull(_battleStateManager.State);
        Assert.AreEqual(_gameBattleState, _gameStateManager.State);
        Assert.AreEqual(_battleInitialisationState, _battleStateManager.State);
        Assert.True(_battleInitialisationState.IsActive());
        _battleStateManager.ChangeState(_battleStartState);
        Assert.AreEqual(_battleStartState, _battleStateManager.State);
        Assert.True(_battleStartState.IsActive());
        _battleStateManager.ChangeState(_preRoundState);
        Assert.AreEqual(_preRoundState, _battleStateManager.State);
        Assert.True(_preRoundState.IsActive());
        _battleStateManager.ChangeState(_preCharacterTurnState);
        Assert.AreEqual(_preCharacterTurnState, _battleStateManager.State);
        Assert.True(_preCharacterTurnState.IsActive());

        Func<bool> isState = () => _battleStateManager.State == _playerTurnState;
        Func<bool> isNotState = () => _battleStateManager.State == _playerTurnState || _battleStateManager.State == _preCharacterTurnState;

        bool isCorrectState = await Task.Run(() => LoopUntil(isState, isNotState));
        Assert.True(isCorrectState);
        Assert.AreEqual(_playerTurnState, _battleStateManager.State);
        Assert.True(_playerTurnState.IsActive());

        _gameStateManager.ChangeState(_testState);
    }

    [Test, Timeout(1000)]
    public async void Assert_BattleStateManagerChangesIntoPostCharacterTurnStateAndItIsActive()
    {
        _gameStateManager.ChangeState(_gameBattleState);
        Assert.NotNull(_battleStateManager.State);
        Assert.AreEqual(_gameBattleState, _gameStateManager.State);
        Assert.AreEqual(_battleInitialisationState, _battleStateManager.State);
        Assert.True(_battleInitialisationState.IsActive());
        _battleStateManager.ChangeState(_battleStartState);
        Assert.AreEqual(_battleStartState, _battleStateManager.State);
        Assert.True(_battleStartState.IsActive());
        _battleStateManager.ChangeState(_preRoundState);
        Assert.AreEqual(_preRoundState, _battleStateManager.State);
        Assert.True(_preRoundState.IsActive());
        _battleStateManager.ChangeState(_preCharacterTurnState);
        Assert.AreEqual(_preCharacterTurnState, _battleStateManager.State);
        Assert.True(_preCharacterTurnState.IsActive());

        Func<bool> isState = () => _battleStateManager.State == _playerTurnState;
        Func<bool> isNotState = () => _battleStateManager.State == _playerTurnState || _battleStateManager.State == _preCharacterTurnState;

        bool isCorrectState = await Task.Run(() => LoopUntil(isState, isNotState));
        Assert.True(isCorrectState);
        Assert.AreEqual(_playerTurnState, _battleStateManager.State);
        Assert.True(_playerTurnState.IsActive());

        int health = EnemyCharacter1.HealthSystem.CurrentHealth;

        _playerTurnState.SimulatePlayerAction(_bash, new List<Character>() { EnemyCharacter1 }, Character1);

        Assert.AreEqual(health, EnemyCharacter1.HealthSystem.CurrentHealth);

        Func<bool> isState2 = () => _battleStateManager.State == _postCharacterTurnState;
        Func<bool> isNotState2 = () => _battleStateManager.State == _playerTurnState || _battleStateManager.State == _postCharacterTurnState;

        bool isCorrectState2 = await Task.Run(() => LoopUntil(isState2, isNotState2));
        Assert.True(isCorrectState);
        Assert.AreEqual(_postCharacterTurnState, _battleStateManager.State);
        Assert.True(_playerTurnState.IsActive());

        _gameStateManager.ChangeState(_testState);
    }*/

    #endregion

    #region Protected Functions
    #endregion

    #region Private Functions

    private bool LoopUntil(Func<bool> positiveCondition, Func<bool> negativeCondition = null)
    {
        var watch = new Stopwatch();
        watch.Start();
        while (!positiveCondition())
        {
            if (negativeCondition != null)
            {
                if (!negativeCondition())
                {
                    return false;
                }
            }
            if (watch.ElapsedMilliseconds > 2000)
            {
                return false;
            }
        }
        return true;
    }

    #endregion

    #region Inner Classes
    #endregion
}
