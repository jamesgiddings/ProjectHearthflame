using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	[SerializeField] private ResourceDatabase resourceDatabase;
	[SerializeField] private StatSystemConstants statSystemConstants;

	private Vector2 movement = Vector2.zero;
	private Vector2 movementNormalized = Vector2.zero;

	private GameState gameState;
	private GameStateManager gameStateManager;

    private static GameManager instance;

	public ResourceDatabase ResourceDatabase => resourceDatabase; // getter
	public StatSystemConstants StatSystemConstants => statSystemConstants; // getter

	public Action<Scene> BattleSceneLoaded;

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

	public State GameState
	{
		get
		{
			if (gameState == null)
			{
				Debug.LogError("gameState is null.");
			}
			return gameState;
		}
	}

	public GameStateManager GameStateManager => gameStateManager;

	public static GameManager Instance
	{
		get
		{
			if(instance == null)
			{
				Debug.LogError("Game Manager is null.");
			}
			return instance;
		}
	}

	private void Awake()
	{
		instance = this;
		SceneManager.sceneLoaded += SceneLoaded;
		gameStateManager = new GameStateManager();
	}

    private void Update()
    {
		gameState.Update();
    }

    private void SceneLoaded(Scene scene, LoadSceneMode mode)
	{
		BattleSceneLoaded?.Invoke(scene);
	}

	private void OnDestroy()
	{
		SceneManager.sceneLoaded -= SceneLoaded;
	}
}
