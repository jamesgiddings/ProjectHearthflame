using GramophoneUtils.Characters;
using NUnit.Framework;

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
    private State _enemyTurnState;
    private State _playerTurnState;
    private State _preRoundState;
    private State _postRoundState;
    private State _preCharacterTurnState;
    private State _postCharacterTurnState;

    private CharacterOrder _playerCharacterOrder;
    private CharacterOrder _enemyCharacterOrder;




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
    }

    #endregion

    #region Protected Functions
    #endregion

    #region Private Functions
    #endregion

    #region Inner Classes
    #endregion
}
