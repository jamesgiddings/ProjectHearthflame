using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralStateTests : BasicEditModeTest
{
    #region Attributes/Fields/Properties

    private StateManager _gameStateManager;

    private State _gameBattleState;
    private State _gameLoadingState;
    private State _gameMenuState;
    private State _gameExplorationState;

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
    }

    [Test]
    public void AssertGameStateManagerNotNullTest()
    {
        Assert.NotNull(_gameStateManager);
    }

    [Test]
    public void AssertGameBattleStateNotNullTest()
    {
        Assert.NotNull(_gameBattleState);
    }

    [Test]
    public void AssertGameExplorationStateNotNullTest()
    {
        Assert.NotNull(_gameExplorationState);
    }

    [Test]
    public void AssertGameLoadingStateNotNullTest()
    {
        Assert.NotNull(_gameLoadingState);
    }

    [Test]
    public void AssertGameMenuStateNotNullTest()
    {
        Assert.NotNull(_gameMenuState);
    }

    [Test]
    public void AssertGameManagerCanSetStateManagersStartingStateTest()
    {
        _gameStateManager.SetStartingState();

        Assert.NotNull(_gameStateManager.StartingState);
        Assert.NotNull(_gameStateManager.State);
    }

    [Test]
    public void AssertGameStateManagerMovesIntoLoadingStateAsStartingStateTest()
    {
        _gameStateManager.SetStartingState();

        Assert.AreEqual(_gameLoadingState, _gameStateManager.State);
        Assert.AreNotEqual(_gameBattleState, _gameStateManager.State);
    }

    [Test]
    public void AssertGameStateManagerSetStartingStateSetsStartingStateForSubStatesTest()
    {
        _gameStateManager.SetStartingState();

        Assert.AreEqual(_gameLoadingState, _gameStateManager.State);
        Assert.NotNull(_gameLoadingState.SubStateManager);
        Assert.NotNull(_gameLoadingState.SubStateManager.State);
    }

    [Test]
    public void AssertGameStateManagerCanChangeStateToExplorationTest()
    {
        _gameStateManager.SetStartingState();

        Assert.AreEqual(_gameLoadingState, _gameStateManager.State);
        Assert.AreNotEqual(_gameExplorationState, _gameStateManager.State);

        _gameStateManager.ChangeState(_gameExplorationState);
        Assert.AreNotEqual(_gameLoadingState, _gameStateManager.State);
        Assert.AreEqual(_gameExplorationState, _gameStateManager.State);
    }

    #endregion

    #region Protected Functions
    #endregion

    #region Private Functions
    #endregion

    #region Inner Classes
    #endregion
}
