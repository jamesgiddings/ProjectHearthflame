using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	public float moveSpeed = 5f;
	private Rigidbody2D rb;

	private Vector2 movementNormalized;
	private Animator animator;

	public Rigidbody2D Rigidbody => rb;

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

		if (movement.x == 1 || movement.x == -1 || movement.y == 1 || movement.y == -1)
		{
			animator.SetFloat("LastX", movement.x);
			animator.SetFloat("LastY", movement.y);
		}
	}

	private void FixedUpdate()
	{
		// Movement
		rb.MovePosition(rb.position + movementNormalized * moveSpeed * Time.fixedDeltaTime);
	}
}
