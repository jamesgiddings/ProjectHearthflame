using System.Collections;
using UnityEngine;

namespace GramophoneUtils.Utilities
{   
    /// <summary>
    /// This is a custom ScriptableObject to inherit from which provides
    /// a "StartCoroutine" function which is accessible from a system which 
    /// is asset based. Original use case was for the SceneController to live 
    /// as an asset, but allow it to call asynchronous scene change functions.
    /// </summary>
    public class ScriptableObjectThatCanRunCoroutines : ScriptableObject
    {
        protected void StartCoroutine(IEnumerator _task)
        {
            if (!Application.isPlaying)
            {
                Debug.LogError("Can not run coroutine outside of play mode.");
                return;
            }

            CoWorker coworker = new GameObject("CoWorker_" + _task.ToString()).AddComponent<CoWorker>();
            coworker.Work(_task);
        }
    }
}


