using GramophoneUtils.SavingLoading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public abstract class Trigger : MonoBehaviour
{
    [SerializeField] protected bool deactivateOnTrigger;
    [SerializeField] private float colliderSizeX = 5;
    [SerializeField] private float colliderSizeY = 5;

    private Rigidbody2D rb;
    private BoxCollider2D boxCollider;
    
    public bool DeactivateOnTrigger => deactivateOnTrigger;

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
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        boxCollider = GetComponent<BoxCollider2D>();
        boxCollider.isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            TriggerAction();
            if (deactivateOnTrigger)
            {
                this.gameObject.SetActive(false);
            }
        }
    }

    protected abstract void TriggerAction();
}
