using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class GetCameraBoundingBox : MonoBehaviour
{
    [SerializeField] CompositeCollider2D cameraBoundingBox;
    private CinemachineConfiner cinemachineConfiner;
    private void Start()
    {
        cinemachineConfiner = GetComponent<CinemachineConfiner>(); 
        if (cinemachineConfiner != null)
        {
            cinemachineConfiner.m_BoundingShape2D = cameraBoundingBox;
        }
    }
}
