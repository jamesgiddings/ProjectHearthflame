using GramophoneUtils.Stats;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class BattleTrigger : MonoBehaviour
{
	[SerializeField] private Battle battle;

	[SerializeField] private float sizeX = 5;
	[SerializeField] private float sizeY = 5;
	
	private Rigidbody2D rb;
	private BoxCollider2D boxCollider;

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.yellow;
		Gizmos.DrawSphere(transform.position, 1);
	}

	private void Start()
	{
		Initialize();
	}

	private void Initialize()
	{
		rb = GetComponent<Rigidbody2D>();
		rb.gravityScale = 0;
		boxCollider = GetComponent<BoxCollider2D>();
		boxCollider.isTrigger = true;
		boxCollider.size = new Vector2(sizeX, sizeY);
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag == "Player")
		{
			PlayerBehaviour player = other.gameObject.GetComponent<PlayerBehaviour>();
			SceneController.AdditiveLoadScene(battle, player.Party);
		}
	}
}
