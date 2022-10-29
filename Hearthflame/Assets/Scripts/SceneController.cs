using GramophoneUtils.Stats;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneController
{

	private static string transitionTargetNameCache;
	private static PlayerBehaviour playerBehaviour;

	public static IEnumerator AdditiveLoadScene(Battle battle = null, PlayerModel player = null)
	{
		AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Battle Scene", LoadSceneMode.Additive);

		asyncLoad.completed += (AsyncOperation) => { InitialiseBattleManager(battle, player); };


		// Wait until the asynchronous scene fully loads
		while (!asyncLoad.isDone)
		{
			yield return null;
		}

	}

	private static void InitialiseBattleManager(Battle battle = null, PlayerModel player = null)
	{
		if (battle != null && player != null)
		{
			BattleManager battleManager = Object.FindObjectOfType<BattleManager>(true);
			battleManager.Initialise(battle, player);
		}
	}

	public static void UnloadScene(string sceneName)
	{
		SceneManager.UnloadSceneAsync(sceneName);
	}

	public static IEnumerator ChangeScene(string targetSceneName, PlayerBehaviour player = null, bool fromLoad = false)
	{
        ServiceLocator.Instance.SavingSystem.SaveOnSceneChange();
        playerBehaviour = player;
		AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(targetSceneName, LoadSceneMode.Single);

        asyncLoad.completed += (AsyncOperation) => { LoadSceneStateOnSceneChange(); };
        asyncLoad.completed += (AsyncOperation) => { SetPlayerPositionOnSceneChange(); };
		
		// Wait until the asynchronous scene fully loads
		while (!asyncLoad.isDone)
		{
			yield return asyncLoad;
		}

        

    }

	public static void CacheTransitionTriggerTargetName(string triggerName)
	{
		Debug.Log("Caching transition trigger name: " + triggerName);
		transitionTargetNameCache = triggerName;
	}

	public static string GetCachedTransitionTriggerTargetName()
	{
		return transitionTargetNameCache;
	}

	public static void SetPlayerPositionOnSceneChange(AsyncOperation obj = null)
	{
		Debug.Log("Position setting");
		//Debug.Log("Scene name: " + SceneManager.GetActiveScene().name);
		//Debug.LogError("Needs to wait until asynchronous scene load is complete.");
		Transform targetTransform = null;
		TransitionTrigger[] transitionTriggers = GameManager.FindObjectsOfType<TransitionTrigger>();
		//Debug.Log("transitionTriggers.Length" + transitionTriggers.Length);
		foreach (TransitionTrigger trigger in transitionTriggers)
		{
			//Debug.Log(trigger.OriginTransition);
			//Debug.Log(GetCachedTransitionTriggerTargetName());
			if (trigger.OriginTransition == GetCachedTransitionTriggerTargetName())
			{
				//Debug.Log("True");
				//Debug.Log("trigger.GetComponentInChildren<Transform>().name: " + trigger.EntryPoint);
				targetTransform = trigger.EntryPoint;
			}
		}
		//Debug.Log("targetTransform:" + targetTransform);
		if (targetTransform != null)
		{
			GameObject.FindGameObjectsWithTag("Player")[0].gameObject.transform.position = targetTransform.position;
		}
		if (obj != null)
			obj.completed -= SetPlayerPositionOnSceneChange;
		CacheTransitionTriggerTargetName("");
	}

    public static void LoadSceneStateOnSceneChange(AsyncOperation obj = null) // at the moment, this isn't getting called
	{
        Debug.Log("Scene name: " + SceneManager.GetActiveScene().name);
        ServiceLocator.Instance.SavingSystem.LoadOnSceneChange();
        if (obj != null)
            obj.completed -= SetPlayerPositionOnSceneChange;
    }

    public static string GetActiveSceneName()
	{
		if (SceneManager.GetActiveScene() != null)
		{
			return SceneManager.GetActiveScene().name;
		}
		else
		{
			return "";
		}
	}

	public static int GetActiveSceneBuildIndex()
	{
		if (SceneManager.GetActiveScene() != null)
		{
			return SceneManager.GetActiveScene().buildIndex;
		}
		else
		{
			return 0;
		}
	}
}
