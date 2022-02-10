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
    [SerializeField] private Slider healthSlider;
    
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
        character.HealthSystem.OnHealthChanged += UpdateHealthSlider;
        character.HealthSystem.OnCharacterDeath += KillCharacter;
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

    public void UpdateHealthSlider()
	{
        if (character.HealthSystem.MaxHealth == 0)
		{
            healthSlider.value = 0;
		}
        else
        {
            Debug.Log("Current Health: " + character.HealthSystem.CurrentHealth);
            Debug.Log("Max Health: " + character.HealthSystem.MaxHealth);
            healthSlider.value = (float)character.HealthSystem.CurrentHealth / (float)character.HealthSystem.MaxHealth;
        }
	}

    private void KillCharacter()
	{
        spriteImage.GetComponent<Image>().color = new Color32(255, 255, 225, 100); // greys out the characters image
        healthSlider.gameObject.SetActive(false);
    }

	private void OnDestroy()
	{
        battleManager.OnCurrentActorChanged -= UpdateCurrentActorHighlightState;
        battleManager.TargetManager.OnCurrentTargetsChanged -= UpdateTargetCursor;
        character.HealthSystem.OnHealthChanged -= UpdateHealthSlider;
        character.HealthSystem.OnCharacterDeath -= KillCharacter;
    }
}
