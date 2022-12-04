using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	public float moveSpeed = 5f;
	private Rigidbody2D rb;

	private Animator animator;

	public Rigidbody2D Rigidbody => rb;

	private void Start()
	{
		rb = gameObject.GetComponent<Rigidbody2D>();
		animator = gameObject.GetComponent<Animator>();
	}

	private void Update()
	{
        if (animator != null) // TODO, hack, this if statement is just so that the player behaviour doesn't break when the animator is missing
        {
            animator.SetFloat("Horizontal", GameManager.Instance.Movement.x);
            animator.SetFloat("Vertical", GameManager.Instance.Movement.y);
            animator.SetFloat("Speed", GameManager.Instance.Movement.sqrMagnitude);

            if (GameManager.Instance.Movement.x == 1 || GameManager.Instance.Movement.x == -1 || GameManager.Instance.Movement.y == 1 || GameManager.Instance.Movement.y == -1)
            {
                animator.SetFloat("LastX", GameManager.Instance.Movement.x);
                animator.SetFloat("LastY", GameManager.Instance.Movement.y);
            }
        }
            
	}

	private void FixedUpdate()
	{
		// Movement
		rb.MovePosition(rb.position + GameManager.Instance.MovementNormalized * moveSpeed * Time.fixedDeltaTime);
	}
}
