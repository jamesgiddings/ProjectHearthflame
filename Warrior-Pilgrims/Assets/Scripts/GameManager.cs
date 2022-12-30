using GramophoneUtils.SavingLoading;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour, ISaveable
{
	[SerializeField] private ResourceDatabase resourceDatabase;

	private Vector2 movement = Vector2.zero;
	private Vector2 movementNormalized = Vector2.zero;

    private static GameManager instance;

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
			// if(instance == null)
			// {
			// 	Debug.LogError("Game Manager is null.");
			// }
			return instance;
		}
	}

	private void Awake()
	{
		Debug.Log("1");
		instance = this;
		SceneManager.sceneLoaded += SceneLoaded;
	}

	private void Start()
	{
        Debug.Log("3");
    }

	/*    private void Update() // Todo this can probably go no the states are monobehaviours
		{
			ServiceLocator.Instance.GameStateManager.Update();
		}*/

	private void SceneLoaded(Scene scene, LoadSceneMode mode)
	{
		OnSceneLoaded?.Invoke(scene);
	}

	private void OnDestroy()
	{
		SceneManager.sceneLoaded -= SceneLoaded;
	}

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

		// Position
		//Debug.Log("We are restoring the saved scene.");
		//SceneController.ChangeScene(saveData.Scene);
	}

	[Serializable]
	public struct GameManagerSaveData
	{
		public string Scene;
	}
		#endregion

}
