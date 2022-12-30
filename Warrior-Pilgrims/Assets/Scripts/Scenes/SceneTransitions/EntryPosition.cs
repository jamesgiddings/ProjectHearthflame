using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A class which ensures that the player enters at a position which matches other objects in the scene.
/// </summary>
public class EntryPosition : MonoBehaviour
{
    #region Attributes/Fields/Properties
    
    [SerializeField] private float zPos = 0f;

    #endregion

    #region Callbacks

    private void Start()
    {
        float xPos = this.gameObject.transform.position.x;
        float yPos = this.gameObject.transform.position.y;
        this.gameObject.transform.position = new Vector3(xPos, yPos, zPos);
    }

    #endregion

    #region API

    #endregion
}
