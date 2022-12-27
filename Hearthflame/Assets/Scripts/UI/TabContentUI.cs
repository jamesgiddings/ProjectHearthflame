using GramophoneUtils.Characters;
using GramophoneUtils.Items;
using GramophoneUtils.Items.Containers;
using GramophoneUtils.Stats;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabContentUI : MonoBehaviour
{
	[SerializeField] Transform statsHolder;
	[SerializeField] Transform healthHolder;
	[SerializeField] Transform equipmentSlotsHolder;
	[SerializeField] Transform inventorySlotsHolder;
	[SerializeField] MoneyDisplayUI moneyDisplayUI;
	[SerializeField] LevelDisplayUI levelDisplayUI;

	private Character character;
	private ItemDestroyer itemDestroyer;

	public void Initialise(Character character, GramophoneUtils.Items.ItemDestroyer itemDestroyer)
	{
		this.character = character;
		this.itemDestroyer = itemDestroyer;
		
		InitialiseEquipmentSlots();
		InitialiseInventorySlots();
		InitialiseStatsDisplay();
		InitialiseHealthDisplay();
		InitialiseMoneyDisplay();
		InitialiseLevelDisplay();
	}

	private void InitialiseLevelDisplay()
	{
		levelDisplayUI.SetCharacter(character);
	}

	private void InitialiseMoneyDisplay()
	{
		moneyDisplayUI.SetInventory(character.PartyInventory);
	}

	private void InitialiseStatsDisplay()
	{
		for (int i = 1; i < statsHolder.childCount; i++) // i == 1, as Health is 0;
			statsHolder.GetChild(i).gameObject.GetComponent<StatDisplay>().Initialise(character);
	}	
	
	private void InitialiseHealthDisplay()
	{
		for (int i = 0; i < statsHolder.childCount; i++)
			healthHolder.GetComponent<HealthDisplay>().Initialise(character);
	}

	private void InitialiseInventorySlots()
	{
		for (int i = 0; i < inventorySlotsHolder.childCount; i++)

			inventorySlotsHolder.GetChild(i).gameObject.GetComponent<InventorySlotUI>().Initialise(character, itemDestroyer);
	}

	private void InitialiseEquipmentSlots()
	{
		for (int i = 0; i < equipmentSlotsHolder.childCount; i++)

			equipmentSlotsHolder.GetChild(i).gameObject.GetComponent<EquipmentSlotUI>().Initialise(character, itemDestroyer);
	}
}
