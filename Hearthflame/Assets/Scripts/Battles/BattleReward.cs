using GramophoneUtils.Items;
using GramophoneUtils.Items.Containers;
using GramophoneUtils.Stats;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Battle Reward", menuName = "Battles/Battle Reward")]
public class BattleReward: Data
{
	[SerializeField] int experience;
	[SerializeField] ItemSlot[] itemSlots;

	public void AddBattleReward(Party party)
	{
		foreach (ItemSlot itemSlot in itemSlots)
		{
			var partyInventory = party.PartyInventory;

			if (partyInventory == null) { return; }
			partyInventory.AddItem(itemSlot);
		}

		foreach (PartyCharacter partyCharacter in party.PartyCharacters)
		{
			partyCharacter.Character.LevelSystem.AddExperience(experience);
		}
	}
}
