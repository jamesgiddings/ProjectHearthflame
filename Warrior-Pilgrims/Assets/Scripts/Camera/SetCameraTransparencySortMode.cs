using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetCameraTransparencySortMode : MonoBehaviour
{
    Camera camera;

    private void Awake()
    {
        camera = gameObject.GetComponent<Camera>();
    }

    private void Start()
    {
        camera.transparencySortMode = TransparencySortMode.Orthographic;
    }
}
