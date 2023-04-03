using GramophoneUtils.Stats;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using GramophoneUtils.Utilities;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Scene Controller", menuName = "Systems/Scene Controller")]
public class SceneController : ScriptableObjectThatCanRunCoroutines
{
	[SerializeField] private List<string> _listOfNoPlayerScenes;

	private TransitionObject _cachedTransitionObject;

    #region API

    public IEnumerator ChangeSceneCoroutine(string destinationScene)
	{
        // Wait until the fade has completed
        yield return new WaitUntil(() => ServiceLocator.Instance.LoadingStateManager.State == ServiceLocator.Instance.UnloadingOldScene); 
		
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(destinationScene, LoadSceneMode.Single);

        asyncLoad.completed += (AsyncOperation) => { LoadSceneStateOnSceneChange(); };
        asyncLoad.completed += (AsyncOperation) => { SetPlayerPositionOnSceneChange(); };

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return asyncLoad;
        }
    }

	public void ChangeScene(TransitionObject transitionObject)
	{
		if (ServiceLocator.Instance != null)
		{
            ServiceLocator.Instance.GameStateManager.ChangeState(ServiceLocator.Instance.LoadingState);
            ServiceLocator.Instance.SavingSystem.SaveOnSceneChange();
        }
        
        CacheTransitionObject(transitionObject);
        StartCoroutine(ChangeSceneCoroutine(transitionObject.DestinationSceneName));
    }

	public bool GetShouldLoadPlayerGameObjects(string sceneName)
	{
		return _listOfNoPlayerScenes.Contains(sceneName);
	}

    public bool GetShouldHandleInstancedPlayerGameObjectsThisScene()
    {
        return !_listOfNoPlayerScenes.Contains(GetActiveSceneName());
    }

    public void CacheTransitionObject(TransitionObject transitionObject)
	{
		_cachedTransitionObject = transitionObject;
	}

    public TransitionObject CachedTransitionObject()
    {
        return _cachedTransitionObject;
    }

    public void SetPlayerPositionOnSceneChange(AsyncOperation obj = null)
	{
		Transform targetTransform = null;
		TransitionTrigger[] transitionTriggers = GameManager.FindObjectsOfType<TransitionTrigger>();
		foreach (TransitionTrigger trigger in transitionTriggers)
		{
			if (trigger.TransitionObject == CachedTransitionObject())
			{
				targetTransform = trigger.EntryPoint;
				break;
			}
		}
		if (targetTransform != null)
		{
			ServiceLocator.Instance.CharacterGameObjectManager.SetPlayerStartTransform(targetTransform);
		}
		if (obj != null)
		{
            obj.completed -= SetPlayerPositionOnSceneChange;
        }
			
	}

    public void LoadSceneStateOnSceneChange(AsyncOperation obj = null) // at the moment, this isn't getting called
	{
        ServiceLocator.Instance.SavingSystem.LoadOnSceneChange();
        if (obj != null)
            obj.completed -= SetPlayerPositionOnSceneChange;
    }


    /// <summary>
    /// If the connection object's scene 1 is the active scene, then the destination is scene 2.
    /// Vice versa.
    /// </summary>
    /// <param name="transitionObject"></param>
    /// <returns></returns>
    public string GetDestinationSceneName(TransitionObject transitionObject)
    {
        return transitionObject.Scene1Name.Equals(GetActiveSceneName()) ? transitionObject.Scene2Name : transitionObject.Scene1Name;
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
