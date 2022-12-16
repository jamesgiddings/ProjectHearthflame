using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneLayer : MonoBehaviour
{
    [SerializeField] private float z = 0;

    #region Callbacks

#if UNITY_EDITOR
    private void OnValidate()
    {
        transform.position = new Vector3(0, 0, z);
    }
#endif

    #endregion
}
