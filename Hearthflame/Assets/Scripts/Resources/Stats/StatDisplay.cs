using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using GramophoneUtils.Stats;

public class StatDisplay : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI statLabel;
    [SerializeField] TextMeshProUGUI statValue;
    [SerializeField] StatType statType;
    [SerializeField] Character character;

	private void OnEnable()
	{
		UpdateStatDisplay();
	}

	public void Initialise(Character character)
	{
		this.character = character;
	}

	public void UpdateStatDisplay()
	{
		statLabel.text = statType.Name;
		statValue.text = character.StatSystem.GetStatValue(statType).ToString();
	}
}
