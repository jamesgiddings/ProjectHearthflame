using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	public float moveSpeed = 5f;
	public Rigidbody2D rb;

	private Vector2 movementNormalized;
	private Animator animator;

	private void Start()
	{
		rb = gameObject.GetComponent<Rigidbody2D>();
		animator = gameObject.GetComponent<Animator>();
	}

	private void Update()
	{
		// Input
		Vector2 movement;
		movement.x = Input.GetAxisRaw("Horizontal");
		movement.y = Input.GetAxisRaw("Vertical");

		movementNormalized = movement.normalized;

		animator.SetFloat("Horizontal", movement.x);
		animator.SetFloat("Vertical", movement.y);
		animator.SetFloat("Speed", movement.sqrMagnitude);
	}

	private void FixedUpdate()
	{
		// Movement
		rb.MovePosition(rb.position + movementNormalized * moveSpeed * Time.fixedDeltaTime);
	}
}
