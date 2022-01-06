using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using GramophoneUtils.Items.Containers;

public class MoneyDisplayUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI moneyValue;
    [SerializeField] private Inventory inventory;

	private void OnEnable()
	{
		UpdateMoneyDisplayUI();
	}
	private void Start()
	{
		UpdateMoneyDisplayUI();
	}

	public void UpdateMoneyDisplayUI()
	{
		Debug.Log("UpdateMoneyDisplayUI()");
		moneyValue.text = inventory.Money.ToString();
	}
}
