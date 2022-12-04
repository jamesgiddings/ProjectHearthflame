using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CorridorPlayerInput : MonoBehaviour
{
    [SerializeField] private float speed;
    private Vector2 movement;
    private Rigidbody2D rigidbody2D;
    
    void Awake() => rigidbody2D = GetComponent<Rigidbody2D>();
    
    void FixedUpdate() => rigidbody2D.velocity = movement
        * speed
        * Time.fixedDeltaTime;

    public void OnMove(InputValue value) => movement = value.Get<Vector2>();
    
}
