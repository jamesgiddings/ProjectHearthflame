using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	[SerializeField] private ResourceDatabase resourceDatabase;

    private static GameManager instance;

	public ResourceDatabase ResourceDatabase => resourceDatabase; // getter

	public Action<Scene> BattleSceneLoaded;

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
