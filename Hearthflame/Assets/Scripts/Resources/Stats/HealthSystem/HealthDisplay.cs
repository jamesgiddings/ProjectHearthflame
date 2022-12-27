using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using GramophoneUtils.Stats;
using GramophoneUtils.Characters;

public class HealthDisplay : MonoBehaviour
{
	[SerializeField] TextMeshProUGUI statLabel;
	[SerializeField] TextMeshProUGUI statValue;
	[SerializeField] Character character;

	private void OnEnable()
	{
		character.HealthSystem.OnHealthChanged += UpdateHealthDisplay;
		UpdateHealthDisplay();
	}

	public void Initialise(Character character)
	{
		this.character = character;
	}

	public void UpdateHealthDisplay(int value = 0)
	{
		statLabel.text = "Health: ";
		statValue.text = character.HealthSystem.CurrentHealth.ToString() + "/" + character.HealthSystem.MaxHealth.ToString();
	}
}