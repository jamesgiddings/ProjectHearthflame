using GramophoneUtils.Stats;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneController
{

	private static string transitionTargetNameCache;
	private static PlayerBehaviour playerBehaviour;

	public static IEnumerator AdditiveLoadScene(Battle battle = null, PlayerBehaviour player = null)
	{
		AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("BattleScene", LoadSceneMode.Additive);

		asyncLoad.completed += (AsyncOperation) => { InitialiseBattleManager(battle, player); };


		// Wait until the asynchronous scene fully loads
		while (!asyncLoad.isDone)
		{
			yield return null;
		}

	}

	private static void InitialiseBattleManager(Battle battle = null, PlayerBehaviour player = null)
	{
		if (battle != null && player != null)
		{
			BattleManager battleManager = Object.FindObjectOfType<BattleManager>(true);
			Debug.Log(battleManager == null);
			battleManager.Initialise(battle, player);
		}

	}

	public static void UnloadScene(string sceneName)
	{
		SceneManager.UnloadSceneAsync(sceneName);
	}

	public static IEnumerator ChangeScene(string targetSceneName, PlayerBehaviour player = null)
	{
		playerBehaviour = player;
		AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(targetSceneName, LoadSceneMode.Single);

		asyncLoad.completed += (AsyncOperation) => { SetPlayerPositionOnSceneChange(); };
		
		// Wait until the asynchronous scene fully loads
		while (!asyncLoad.isDone)
		{
			yield return null;
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
		Debug.Log("Scene name: " + SceneManager.GetActiveScene().name);
		//Debug.LogError("Needs to wait until asynchronous scene load is complete.");
		Transform targetTransform = null;
		TransitionTrigger[] transitionTriggers = GameManager.FindObjectsOfType<TransitionTrigger>();
		Debug.Log("transitionTriggers.Length" + transitionTriggers.Length);
		foreach (TransitionTrigger trigger in transitionTriggers)
		{
			Debug.Log(trigger.TransitionTriggerName);
			Debug.Log(GetCachedTransitionTriggerTargetName());
			if (trigger.TransitionTriggerName == GetCachedTransitionTriggerTargetName())
			{
				Debug.Log("True");
				Debug.Log("trigger.GetComponentInChildren<Transform>().name: " + trigger.EntryPoint);
				targetTransform = trigger.EntryPoint;
			}
		}
		Debug.Log("targetTransform:" + targetTransform);
		if (targetTransform != null)
		{
			GameObject.FindGameObjectsWithTag("Player")[0].gameObject.transform.position = targetTransform.position;
		}
		if (obj != null)
			obj.completed -= SetPlayerPositionOnSceneChange;
		CacheTransitionTriggerTargetName("");
	}

}
