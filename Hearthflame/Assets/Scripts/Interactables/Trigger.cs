using GramophoneUtils.SavingLoading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public abstract class Trigger : MonoBehaviour
{
    [SerializeField] protected bool _deactivateOnTrigger;
    [SerializeField] private float _colliderSizeX = 5;
    [SerializeField] private float _colliderSizeY = 5;

    protected Rigidbody2D RigidBody;
    private BoxCollider2D _boxCollider;
    
    public bool DeactivateOnTrigger => _deactivateOnTrigger;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, 1);
    }

    protected virtual void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        RigidBody = GetComponent<Rigidbody2D>();
        RigidBody.gravityScale = 0;
        _boxCollider = GetComponent<BoxCollider2D>();
        _boxCollider.isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            TriggerAction();
            if (_deactivateOnTrigger)
            {
                this.gameObject.SetActive(false);
            }
        }
    }

    protected abstract void TriggerAction();
}
