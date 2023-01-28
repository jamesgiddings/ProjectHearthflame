using GramophoneUtils.SavingLoading;
using GramophoneUtils.Stats;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Service Locator Object", menuName = "Systems/Service Locator Object")]
public class ServiceLocatorObject : ScriptableObject
{
    [SerializeField, Required] private Constants _constants;
    public Constants Constants => _constants;

    [SerializeField, Required] private StatSystemConstants _statSystemConstants;
    public StatSystemConstants StatSystemConstants => _statSystemConstants;

    [SerializeField, Required] private SceneController _sceneController;
    public SceneController SceneController => _sceneController;

    private static string _pathToServiceLocatorObject = "Assets/Resources/Systems/Service Locator Object.asset";
    public static string PathToServiceLocatorObject => _pathToServiceLocatorObject;

    [BoxGroup("Characters")]
    [SerializeField] private CharacterModel characterModel;
    public CharacterModel CharacterModel => characterModel;

    [BoxGroup("Tests")]
    [SerializeField] private TestObjectReferences _testObjectReferences;
    public TestObjectReferences TestObjectReferences => _testObjectReferences;

    [BoxGroup("States")]
    [SerializeField] private StateManager _gameStateManager;
    public StateManager GameStateManager => _gameStateManager;

    [BoxGroup("States/Battle")]
    [SerializeField] private State _gameBattleState;
    public State GameBattleState => _gameBattleState;

    [BoxGroup("States")]
    [SerializeField] private State _gameLoadingState;
    public State GameLoadingState => _gameLoadingState;

    [BoxGroup("States")]
    [SerializeField] private State _gameMenuState;
    public State GameMenuState => _gameMenuState;

    [BoxGroup("States")]
    [SerializeField] private State _gameExplorationState;
    public State GameExplorationState => _gameExplorationState;

    [BoxGroup("States/Battle")]
    [SerializeField] private StateManager _battleStateManager;
    public StateManager BattleStateManager => _battleStateManager;

    [BoxGroup("States/Battle")]
    [SerializeField] private State _battleInitialisationState;
    public State BattleInitialisationState => _battleInitialisationState;

    [BoxGroup("States/Battle")]
    [SerializeField] private State _battleLostState;
    public State BattleLostState => _battleLostState;

    [BoxGroup("States/Battle")]
    [SerializeField] private State _battleOverState;
    public State BattleOverState => _battleOverState;

    [BoxGroup("States/Battle")]
    [SerializeField] private State _battleStartState;
    public State BattleStartState => _battleStartState;

    [BoxGroup("States/Battle")]
    [SerializeField] private State _battleWonState;
    public State BattleWonState => _battleWonState;

    [BoxGroup("States/Battle")]
    [SerializeField] private EnemyTurnState _enemyTurnState;
    public EnemyTurnState EnemyTurnState => _enemyTurnState;

    [BoxGroup("States/Battle")]
    [SerializeField] private PlayerTurnState _playerTurnState;
    public PlayerTurnState PlayerTurnState => _playerTurnState;

    [BoxGroup("States/Battle")]
    [SerializeField] private State _postCharacterTurn;
    public State PostCharacterTurnState => _postCharacterTurn;

    [BoxGroup("States/Battle")]
    [SerializeField] private State _preCharacterTurn;
    public State PreCharacterTurnState => _preCharacterTurn;

    [BoxGroup("States/Battle")]
    [SerializeField] private State _preRound;
    public State PreRoundState => _preRound;    
    
    [BoxGroup("States/Battle")]
    [SerializeField] private State _postRound;
    public State PostRoundState => _postRound;

    [BoxGroup("Battle")]
    [BoxGroup("Battle/Battle Objects")]
    [SerializeField] private BattleManager _battleManager;
    public BattleManager BattleManager => _battleManager;

    [BoxGroup("Battle/Battle Objects")]
    [SerializeField] private BattleDataModel _battleDataModel;
    public BattleDataModel BattleDataModel => _battleDataModel;

    [BoxGroup("Saving/Loading")]
    [SerializeField] private SavingSystem _savingSystem;
    public SavingSystem SavingSystem => _savingSystem;

}
