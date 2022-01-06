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
    [SerializeField] StatSystemBehaviour statSystemBehaviour;

	private void OnEnable()
	{
		UpdateStatDisplay();
	}

	public void UpdateStatDisplay()
	{
		statLabel.text = statType.Name;
		statValue.text = statSystemBehaviour.StatSystem.GetStatValue(statType).ToString();
		Debug.Log("update in stat display");
	}
}
