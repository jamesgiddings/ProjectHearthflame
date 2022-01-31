using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using GramophoneUtils.Items.Containers;
using GramophoneUtils.Stats;

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
		moneyValue.text = inventory.Money.ToString();
	}

	public void SetInventory(Party party)
	{
		this.inventory = party.PartyInventory;
		UpdateMoneyDisplayUI();
	}
}
