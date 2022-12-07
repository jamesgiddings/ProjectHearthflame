using GramophoneUtils.Stats;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using GramophoneUtils.Utilities;

[CreateAssetMenu(fileName = "Scene Controller", menuName = "Systems/Scene Controller")]
public class SceneController : ScriptableObjectThatCanRunCoroutines
{
	private string _transitionTargetNameCache;

	public IEnumerator ChangeSceneCoroutine(string destinationScene)
	{
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(destinationScene, LoadSceneMode.Single);

        asyncLoad.completed += (AsyncOperation) => { LoadSceneStateOnSceneChange(); };
        asyncLoad.completed += (AsyncOperation) => { SetPlayerPositionOnSceneChange(); };
        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return asyncLoad;
        }
    }

	public void ChangeScene(SceneConnectionObject sceneConnectionObject)
	{
		string destinationScene = GetDestinationSceneName(sceneConnectionObject);
        ServiceLocator.Instance.SavingSystem.SaveOnSceneChange();
        CacheTransitionTriggerTargetName(sceneConnectionObject.TriggerName);
        StartCoroutine(ChangeSceneCoroutine(destinationScene));
    }

	public void CacheTransitionTriggerTargetName(string triggerName)
	{
		_transitionTargetNameCache = triggerName;
	}

	public string GetCachedTransitionTriggerTargetName()
	{
		return _transitionTargetNameCache;
	}

	public void SetPlayerPositionOnSceneChange(AsyncOperation obj = null)
	{
		Transform targetTransform = null;
		TransitionTrigger[] transitionTriggers = GameManager.FindObjectsOfType<TransitionTrigger>();
		foreach (TransitionTrigger trigger in transitionTriggers)
		{
			if (trigger.OriginTransition == GetCachedTransitionTriggerTargetName())
			{
				targetTransform = trigger.EntryPoint;
			}
		}
		if (targetTransform != null)
		{
			GameObject.FindGameObjectsWithTag("Player")[0].gameObject.transform.position = targetTransform.position;
		}
		if (obj != null)
			obj.completed -= SetPlayerPositionOnSceneChange;
	}

    public void LoadSceneStateOnSceneChange(AsyncOperation obj = null) // at the moment, this isn't getting called
	{
        ServiceLocator.Instance.SavingSystem.LoadOnSceneChange();
        if (obj != null)
            obj.completed -= SetPlayerPositionOnSceneChange;
    }

    #region Utilities

	/// <summary>
	/// If the connection object's scene 1 is the active scene, then the destination is scene 2.
	/// Vice versa.
	/// </summary>
	/// <param name="sceneConnectionObject"></param>
	/// <returns></returns>
	private string GetDestinationSceneName(SceneConnectionObject sceneConnectionObject)
	{
		return sceneConnectionObject.Scene1Name.Equals(GetActiveSceneName()) ? sceneConnectionObject.Scene2Name : sceneConnectionObject.Scene1Name;
	}

    public string GetActiveSceneName()
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

	public int GetActiveSceneBuildIndex()
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
	public void UnloadScene(string sceneName)
    {
        SceneManager.UnloadSceneAsync(sceneName);
    }
    #endregion
}
