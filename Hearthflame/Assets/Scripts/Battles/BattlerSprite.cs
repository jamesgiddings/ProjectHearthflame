using GramophoneUtils.Stats;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BattlerSprite : MonoBehaviour
{
    [SerializeField] private Image spriteImage;
    [SerializeField] private Image targetCursor;
    [SerializeField] private Image currentActorHighlight;
    [SerializeField] private Slider healthSlider;
    [SerializeField] private TextMeshProUGUI floatingTextNotification;
    
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
        character.HealthSystem.OnHealthChanged += DisplayFloatingTexts;
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

    public void UpdateHealthSlider(int value)
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

    public void DisplayFloatingTexts(int value)
	{
        StartCoroutine(DisplayFloatingTextsCoroutine(value));
    }

    public IEnumerator DisplayFloatingTextsCoroutine(int value)
	{
        Debug.Log("Display Floating texts NOW ----------------------------------------------------------");
        while (character.GetIsAnyNotificationInQueue())
        {
            floatingTextNotification.gameObject.SetActive(true);
            DisplayFloatingText(character.DequeueBattlerNoticiation());
            yield return new WaitForSeconds(0.4f);
        }
        yield return new WaitForSeconds(1f);
        floatingTextNotification.gameObject.SetActive(false);
    }

    public void DisplayFloatingText(int value)
	{
        Color textColor = (value >= 0) ? Color.green : Color.red;
        floatingTextNotification.color = textColor;
        floatingTextNotification.text = value.ToString();
	}

    private void KillCharacter()
	{
        spriteImage.GetComponent<Image>().color = new Color32(255, 255, 225, 100); // greys out the characters image
        healthSlider.gameObject.SetActive(false);
    }

	private void OnDisable()
	{
        battleManager.OnCurrentActorChanged -= UpdateCurrentActorHighlightState;
        battleManager.TargetManager.OnCurrentTargetsChanged -= UpdateTargetCursor;
        character.HealthSystem.OnHealthChanged -= UpdateHealthSlider;
        character.HealthSystem.OnHealthChanged -= DisplayFloatingTexts;
        character.HealthSystem.OnCharacterDeath -= KillCharacter;
    }
}
