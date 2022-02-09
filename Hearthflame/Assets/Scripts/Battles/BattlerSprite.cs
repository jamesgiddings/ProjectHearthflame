using GramophoneUtils.Stats;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattlerSprite : MonoBehaviour
{
    [SerializeField] private Image spriteImage;
    [SerializeField] private Image targetCursor;
    [SerializeField] private Image currentActorHighlight;
    
    private Character character;
    private BattleManager battleManager;

    private bool isBeingTargeted;

    public bool IsBeingTargeted { get { return isBeingTargeted; } set { isBeingTargeted = value; } }

    public void Initialise(BattleManager battleManager, Character character)
	{
        this.character = character;
        this.battleManager = battleManager;
        this.spriteImage.sprite = character.PartyCharacterTemplate.Icon;
        battleManager.OnCurrentActorChanged += UpdateCurrentActorHighlightState;
        battleManager.TargetManager.OnCurrentTargetsChanged += UpdateTargetCursor;
	}

	private void UpdateTargetCursor(List<Character> targets)
	{
        if (targets.Contains(character))
        {
            targetCursor.gameObject.SetActive(true);
		}
		else
		{
            targetCursor.gameObject.SetActive(false);
        }

    }

	public void UpdateCurrentActorHighlightState()
	{
        currentActorHighlight.gameObject.SetActive(character.IsCurrentActor);
    }

	private void OnDestroy()
	{
        battleManager.OnCurrentActorChanged -= UpdateCurrentActorHighlightState;
        battleManager.TargetManager.OnCurrentTargetsChanged -= UpdateTargetCursor;
    }
}
