using GramophoneUtils.SavingLoading;
using Sirenix.OdinInspector;
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour, ISaveable
{
    #region Attributes/Fields/Properties

    [SerializeField] private ResourceDatabase resourceDatabase;

	private Vector2 movement = Vector2.zero;
	private Vector2 movementNormalized = Vector2.zero;

    private static GameManager instance;

	private StateManager _gameStateManager;
	[ShowInInspector] private State _currentState;
    [ShowInInspector] private State _currentSubState;

	public ResourceDatabase ResourceDatabase => resourceDatabase; // getter

	public Action<Scene> OnSceneLoaded;

	public Vector2 Movement
	{
		get
		{
			return movement;
		}
		set
		{
			movement = value;
		}
	}

	public Vector2 MovementNormalized
	{
		get
		{
			return movementNormalized;
		}
		set
		{
			movementNormalized = value;
		}
	}

	public static GameManager Instance
	{
		get
		{
			return instance;
		}
	}

    #endregion

    #region Callbacks

    private void Awake()
	{
		instance = this;
		SceneManager.sceneLoaded += SceneLoaded;
		_gameStateManager = ServiceLocator.Instance.GameStateManager;
        _gameStateManager.SetStartingState();
    }

	// Game loop
	private void Update()
	{

		if (_gameStateManager != null)
		{
#if UNITY_EDITOR
            _currentState = _gameStateManager.State == null ? null : _gameStateManager.State;
			if (_gameStateManager.State.SubStateManager != null)
			{
				_currentSubState = _gameStateManager.State.SubStateManager.State;
            }
			else
			{
                _currentSubState = null;
			}
#endif
			_gameStateManager.HandleInput();
        }
		else
		{
			Debug.LogError("_gameStateManager is null in GameManager.");
		}
    }

	private void OnDestroy()
    {
        SceneManager.sceneLoaded -= SceneLoaded;
    }

    #endregion

    #region API

    #region Saving Loading

    public object CaptureState()
    {
        return new GameManagerSaveData
        {
            // Scene

            Scene = ServiceLocator.Instance.ServiceLocatorObject.SceneController.GetActiveSceneName()
        };
    }

    public void RestoreState(object state)
    {
        var saveData = (GameManagerSaveData)state;
    }

    [Serializable]
    public struct GameManagerSaveData
    {
        public string Scene;
    }

    #endregion

    #endregion

    #region Utitlies

    private void SceneLoaded(Scene scene, LoadSceneMode mode)
	{
		OnSceneLoaded?.Invoke(scene);
	}

	#endregion



}
