using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A class which ensures that the player enters at a position which matches other objects in the scene.
/// </summary>
public class EntryPosition : MonoBehaviour
{
    [SerializeField] private float yPos = -7.3f;
    [SerializeField] private float zPos = 0f;

    private void Start()
    {
        float xPos = this.gameObject.transform.position.x;
        this.gameObject.transform.position = new Vector3(xPos, yPos, zPos);
    }
}
