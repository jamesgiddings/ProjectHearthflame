using GramophoneUtils.Items;
using GramophoneUtils.Items.Containers;
using GramophoneUtils.Stats;
using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using GramophoneUtils.Characters;

[CreateAssetMenu(fileName = "New Battle Reward", menuName = "Battles/Battle Reward")]
public class BattleReward: Data
{
	[SerializeField] private IntReference _experience;
	[SerializeField] ItemSlot[] itemSlots;

	public void AddBattleReward(ICharacterModel player)
	{
		foreach (ItemSlot itemSlot in itemSlots)
		{
			var partyInventory = player.PartyInventory;

			if (partyInventory == null) { return; }
			partyInventory.AddItem(itemSlot);
		}

		foreach (Character character in player.PlayerCharacters)
		{
			character.LevelSystem.AddExperience(_experience.Value);
		}
	}

	public string GetRewardsInfoDisplayText()
	{
		StringBuilder builder = new StringBuilder();
		builder.Append("Experience: ").Append(_experience.Value.ToString()).AppendLine();

		foreach (ItemSlot itemSlot in itemSlots)
		{
			string color = HexConverter(itemSlot.item.Rarity.Colour);

			string colouredString = "<color=" + color + ">" + itemSlot.item.Name;

			builder.Append(colouredString).Append("</color>").AppendLine();
		}

		return builder.ToString();
	}

	private static string HexConverter(UnityEngine.Color c)
	{
		Color32 color32 = c;
		string hex = color32.r.ToString("X2") + color32.g.ToString("X2") + color32.b.ToString("X2");
		hex = "#" + hex;

		return hex;
	}


}
