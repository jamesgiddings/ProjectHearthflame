using GramophoneUtils.Maps;
using GramophoneUtils.SavingLoading;
using GramophoneUtils.Stats;
using Sirenix.OdinInspector;
using System;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class TransitionTrigger : MonoBehaviour, ISaveable
{
    [SerializeField, Required] private TransitionObject _transitionObject;
    
	[SerializeField] private Transform entryPoint;

	[SerializeField] private float sizeX = 5;
	[SerializeField] private float sizeY = 5;

	[SerializeField] private bool deactivateOnTrigger;

	private Rigidbody2D rb;
	private BoxCollider2D boxCollider;

    public TransitionObject TransitionObject => _transitionObject;

	public Transform EntryPoint
	{
		get 
		{
			if (entryPoint != null) 
			{
				return entryPoint;
			}
			entryPoint = gameObject.transform.GetChild(0);
			return entryPoint;
		}
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.green;
		Gizmos.DrawSphere(transform.position, 1);
	}

	private void Awake()
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
			_transitionObject.ChangeScene();

            if (deactivateOnTrigger)
			{
				gameObject.SetActive(false);
			}
		}
	}

	#region Utilities

#if UNITY_EDITOR

	private void OnValidate()
	{
		if (_transitionObject != null)
		{
            this.name = _transitionObject.name + "_Trigger";
        }
	}

#endif

	#endregion

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
