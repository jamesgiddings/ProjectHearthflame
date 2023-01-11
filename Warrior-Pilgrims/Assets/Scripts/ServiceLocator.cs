using GramophoneUtils.SavingLoading;
using GramophoneUtils.Stats;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;

public class ServiceLocator : MonoBehaviour, IServiceLocator
{
    [SerializeField, Required] private ServiceLocatorObject _serviceLocatorObject;
    [SerializeField] private DialogueUI _dialogueUI;
    [SerializeField] private EventSystem _eventSystem;
    [SerializeField] private Camera _mainCamera;
    [SerializeField] private CharacterGameObjectManager _characterGameObjectManager;
    [SerializeField] private CharacterModel _characterModel;
    [SerializeField] private SavingSystem _savingSystem;
    [SerializeField] private UIServiceLocator _uiServiceLocator;
    [SerializeField] private GameManager _gameManager;

    [BoxGroup("Input")]
    [SerializeField] private PlayerInputBehaviour _playerInputBehaviour;
    public PlayerInputBehaviour PlayerInputBehaviour => _playerInputBehaviour;

    [BoxGroup("States")]
    [BoxGroup("States/Game States")]
    [SerializeField] private StateManager _gameStateManager;
    [BoxGroup("States/Game States")]
    [SerializeField] private State _explorationState;
    [BoxGroup("States/Game States")]
    [SerializeField] private State _battleState;
    [BoxGroup("States/Game States")]
    [SerializeField] private State _loadingState;
    [BoxGroup("States/Game States")]
    [SerializeField] private State _menuState;
    [BoxGroup("States/Battle States")]
    [SerializeField] private StateManager _battleStateManager;
    [BoxGroup("States/Battle States")]
    [SerializeField] private State _battleInitialisationState;
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
    [BoxGroup("States/Loading States")]
    [SerializeField] private StateManager _loadingStateManager;
    [BoxGroup("States/Loading States")]
    [SerializeField] private State _waitForFade;
    [BoxGroup("States/Loading States")]
    [SerializeField] private State _unloadingOldScene;
    [BoxGroup("States/Loading States")]
    [SerializeField] private State _gameObjectCallbacksState;
    [BoxGroup("States/Loading States")]
    [SerializeField] private State _instantiantingCharactersState;

    [BoxGroup("Battles")]
    [SerializeField] private Transform _battleUITransform;
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
    public CharacterGameObjectManager CharacterGameObjectManager => _characterGameObjectManager;
    public CharacterModel CharacterModel => _characterModel;
    public SavingSystem SavingSystem => _savingSystem;
    public GameManager GameManager => _gameManager;
    public StateManager GameStateManager => _gameStateManager;
    public State BattleState => _battleState;
    public State ExplorationState => _explorationState;
    public State LoadingState => _loadingState;
    public State MenuState => _menuState;
    public StateManager BattleStateManager => _battleStateManager;
    public State BattleInitialisationState => _battleInitialisationState;
    public State BattleStartState => _battleStartState;
    public State BattleWonState => _battleWonState;
    public State BattleLostState => _battleLostState;
    public State BattleOverState => _battleOverState;
    public State PlayerTurnState => _playerTurnState;
    public State EnemyTurnState => _enemyTurnState;
    public StateManager LoadingStateManager => _loadingStateManager;
    public State WaitForFade => _waitForFade;
    public State UnloadingOldScene => _unloadingOldScene;
    public State GameObjectCallbacksState => _gameObjectCallbacksState;
    public State InstantiatingCharactersState => _instantiantingCharactersState;
    public Transform BattleUITransform => _battleUITransform;
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
