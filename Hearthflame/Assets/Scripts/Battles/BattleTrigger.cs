using GramophoneUtils.SavingLoading;
using GramophoneUtils.Stats;
using System;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]

public class BattleTrigger : MonoBehaviour, ISaveable
{
	[SerializeField] private Battle battle;

	[SerializeField] private float sizeX = 5;
	[SerializeField] private float sizeY = 5;

	[SerializeField] bool deactivateOnTrigger;
	
	private Rigidbody2D rb;
	private BoxCollider2D boxCollider;

	public Battle Battle => battle;
	public bool DeactivateOnTrigger => deactivateOnTrigger;

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
			battle.InstanceCharacters();
			StartCoroutine(SceneController.AdditiveLoadScene(battle, player));
			if (deactivateOnTrigger)
			{
				this.gameObject.SetActive(false);
			}
		}
	}

	#region SavingLoading
	public object CaptureState()
	{
		Debug.Log("gameObject.activeInHierarchy while saving: " + gameObject.activeInHierarchy);
		return new SaveData
		{
			IsActive = gameObject.activeInHierarchy
			
		};
	}

	public void RestoreState(object state)
	{
		var saveData = (SaveData)state;
		Debug.Log("saveData.IsActive while loading: " + saveData.IsActive);
		gameObject.SetActive(saveData.IsActive);
	}

	[Serializable]
	public struct SaveData
	{
		public bool IsActive;
	}
	#endregion
}
