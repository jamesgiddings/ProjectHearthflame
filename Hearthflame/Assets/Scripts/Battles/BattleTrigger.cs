using GramophoneUtils.SavingLoading;
using GramophoneUtils.Stats;
using System;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(SaveableEntity))]

public class BattleTrigger : StatefulTrigger
{
	[SerializeField] private Battle battle;

	public Battle Battle => battle;
	private SpriteRenderer spriteRenderer;
    
	protected override void Start()
	{
        base.Start();
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = battle.BattleSprite;
        if (battle.DeactivateOnComplete && battle.IsComplete)
        {
            this.gameObject.SetActive(false);
        }
    }

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.yellow;
		Gizmos.DrawSphere(transform.position, 1);
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag == "Player")
		{
			TriggerAction();
		}
	}

    protected override void TriggerAction()
    {
        battle.InstanceCharacters();
        StartCoroutine(SceneController.AdditiveLoadScene(battle, ServiceLocator.Instance.PlayerModel));
        if (deactivateOnTrigger)
        {
            this.gameObject.SetActive(false);
        }
    }

    #region SavingLoading
    public override object CaptureState()
	{
		return new SaveData
		{
			IsActive = gameObject.activeInHierarchy,
            IsComplete = battle.IsComplete
        };
	}

	public override void RestoreState(object state)
	{
		var saveData = (SaveData)state;
		gameObject.SetActive(saveData.IsActive);
		battle.IsComplete = battle.IsComplete;
	}

	[Serializable]
	public struct SaveData
	{
		public bool IsActive;
		public bool IsComplete;
	}
	#endregion
}
