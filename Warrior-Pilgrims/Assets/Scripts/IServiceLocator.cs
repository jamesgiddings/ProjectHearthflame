using GramophoneUtils.SavingLoading;
using GramophoneUtils.Stats;
using UnityEngine;
using UnityEngine.EventSystems;

public interface IServiceLocator
{
    public static IServiceLocator Instance { get; private set; }
    public ServiceLocatorObject ServiceLocatorObject { get; }
    public UIServiceLocator UIServiceLocator { get; }
    public DialogueUI DialogueUI { get; }
    public EventSystem EventSystem { get; }
    public Camera MainCamera { get; }
    public CharacterGameObjectManager CharacterGameObjectManager { get; }
    public ICharacterModel CharacterModel { get; }
    public SavingSystem SavingSystem { get; }
    public GameManager GameManager { get; }
    public StateManager GameStateManager { get; }
    public State BattleState { get; }
    public State ExplorationState { get; }
    public State LoadingState { get; }
    public State MenuState { get; }
    public StateManager BattleStateManager { get; }
    public State BattleStartState { get; }
    public State BattleWonState { get; }
    public State BattleLostState { get; }
    public State BattleOverState { get; }
    public State PlayerTurnState { get; }
    public State EnemyTurnState { get; }
    public StateManager LoadingStateManager { get; }
    public State WaitForFade { get; }
    public State UnloadingOldScene { get; }
    public State GameObjectCallbacksState { get; }
    public State InstantiatingCharactersState { get; }
    public BattlerDisplayUI BattlerDisplayUI { get; }
    public BattleManager BattleManager { get; }
    public BattleDataModel BattleDataModel { get; }
    public ITargetManager TargetManager { get; }
}
