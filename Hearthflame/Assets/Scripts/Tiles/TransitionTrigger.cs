using GramophoneUtils.SavingLoading;
using GramophoneUtils.Stats;
using System;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class TransitionTrigger : MonoBehaviour, ISaveable
{
	[SerializeField] private string targetSceneName;
	[SerializeField] private string targetTransitionTriggerName;
	[SerializeField] private string transitionTriggerName;
	
	[SerializeField] private Transform entryPoint;

	[SerializeField] private float sizeX = 5;
	[SerializeField] private float sizeY = 5;

	[SerializeField] private bool deactivateOnTrigger;

	private Rigidbody2D rb;
	private BoxCollider2D boxCollider;

	public bool DeactivateOnTrigger => deactivateOnTrigger;
	public string TransitionTriggerName => transitionTriggerName;
	public string TargetTransitionTriggerName => targetTransitionTriggerName;

	public Transform EntryPoint
	{
		get 
		{
			if (entryPoint != null) return entryPoint;
			entryPoint = gameObject.transform.GetChild(0);
			return entryPoint;
		}
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.green;
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

	public void SetTargetScene(string sceneName)
	{
		targetSceneName = sceneName;
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag == "Player")
		{
			PlayerBehaviour player = other.GetComponent<PlayerBehaviour>();
			SceneController.CacheTransitionTriggerTargetName(targetTransitionTriggerName);
			StartCoroutine(SceneController.ChangeScene(targetSceneName, player));
			if (deactivateOnTrigger)
			{
				gameObject.SetActive(false);
			}
		}
	}

	#region SavingLoading
	public object CaptureState()
	{
		return new SaveData
		{
			IsActive = gameObject.activeInHierarchy
		};
	}

	public void RestoreState(object state)
	{
		var saveData = (SaveData)state;
		gameObject.SetActive(saveData.IsActive);
	}

	[Serializable]
	public struct SaveData
	{
		public bool IsActive;
	}
	#endregion
}
