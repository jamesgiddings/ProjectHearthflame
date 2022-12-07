using GramophoneUtils.SavingLoading;
using GramophoneUtils.Stats;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;

public class ServiceLocator : MonoBehaviour 
{
    [SerializeField, Required] private ServiceLocatorObject _serviceLocatorObject;
    [SerializeField] private DialogueUI _dialogueUI;
    [SerializeField] private EventSystem _eventSystem;
    [SerializeField] private Camera _mainCamera;
    [SerializeField] private PlayerBehaviour _playerBehaviour;
    [SerializeField] private PlayerModel _playerModel;
    [SerializeField] private SavingSystem _savingSystem;
    [SerializeField] private UIServiceLocator _uiServiceLocator;
    [SerializeField] private GameManager _gameManager;

    [BoxGroup("States")]
    [BoxGroup("States/Game States")]
    [SerializeField] private StateManager _gameStateManager;
    [BoxGroup("States/Game States")]
    [SerializeField] private State _explorationState;
    [BoxGroup("States/Game States")]
    [SerializeField] private State _battleState;
    [BoxGroup("States/Battle States")]
    [SerializeField] private StateManager _battleStateManager;
    [BoxGroup("States/Battle States")]
    [SerializeField] private State _battleStartState;
    [BoxGroup("States/Battle States")]
    [SerializeField] private State _battleWonState;
    [BoxGroup("States/Battle States")]
    [SerializeField] private State _battleLostState;
    [BoxGroup("States/Battle States")]
    [SerializeField] private State _battleOverState;
    [BoxGroup("States/Battle States")]
    [SerializeField] private State _playerTurnState;
    [BoxGroup("States/Battle States")]
    [SerializeField] private State _enemyTurnState;

    [BoxGroup("Battles")]
    [SerializeField] private BattlerDisplayUI _battlerDisplayUI;
    [BoxGroup("Battles")]
    [SerializeField] private BattleManager _battleManager;
    [BoxGroup("Battles")]
    [SerializeField] private BattleDataModel _battleDataModel;
    [BoxGroup("Battles")]
    [SerializeField] private TargetManager _targetManager;




    public static ServiceLocator Instance { get; private set; }
    public ServiceLocatorObject ServiceLocatorObject => _serviceLocatorObject;
    public UIServiceLocator UIServiceLocator => _uiServiceLocator;
    public DialogueUI DialogueUI => _dialogueUI;
    public EventSystem EventSystem => _eventSystem;
    public Camera MainCamera => _mainCamera;
    public PlayerBehaviour PlayerBehaviour => _playerBehaviour;
    public PlayerModel PlayerModel => _playerModel;
    public SavingSystem SavingSystem => _savingSystem;
    public GameManager GameManager => _gameManager;
    public StateManager GameStateManager => _gameStateManager;
    public State BattleState => _battleState;
    public State ExplorationState => _explorationState;
    public StateManager BattleStateManager => _battleStateManager;
    public State BattleStartState => _battleStartState;
    public State BattleWonState => _battleWonState;
    public State BattleLostState => _battleLostState;
    public State BattleOverState => _battleOverState;
    public State PlayerTurnState => _playerTurnState;
    public State EnemyTurnState => _enemyTurnState;
    public BattlerDisplayUI BattlerDisplayUI => _battlerDisplayUI;
    public BattleManager BattleManager => _battleManager;
    public BattleDataModel BattleDataModel => _battleDataModel;
    public TargetManager TargetManager => _targetManager;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }

    }
}
