using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GramophoneUtils.Utilities
{
    /// <summary>
    /// This class is used in conjunction with the
    /// ScriptableObjectThatCanRunCoroutines class, and is
    /// the gameObject which is temporarily created to allow for the 
    /// running of the coroutine and, when that is finished,
    /// it will handle its own deletion.
    /// </summary>
    public class CoWorker : MonoBehaviour
    {
        public void Work(IEnumerator _coroutine)
        {
            StartCoroutine(WorkCoroutine(_coroutine));
        }

        private IEnumerator WorkCoroutine(IEnumerator _coroutine)
        {
            yield return StartCoroutine(_coroutine);
            Destroy(this.gameObject);
        }
    }
}
