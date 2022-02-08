using GramophoneUtils.Items.Containers;
using UnityEngine;
using UnityEngine.Events;

namespace GramophoneUtils.Stats
{
	public class Party
	{
		[SerializeField] public UnityEvent onStatsChanged;
		[SerializeField] public UnityEvent onInventoryItemsUpdated;

		private Inventory partyInventory = new Inventory(20, 10000);

		[SerializeField] private PartyCharactersTemplateObject partyCharactersTemplateObject;

		private PartyCharacter[] partyCharacters;

		public Inventory PartyInventory => partyInventory; //getter

		public PartyCharacter[] PartyCharacters
		{
			get
			{
				if (partyCharacters != null) { return partyCharacters; }
				partyCharacters = new PartyCharacter[partyCharactersTemplateObject.PartyCharacterTemplates.Length];
				for (int i = 0; i < partyCharactersTemplateObject.PartyCharacterTemplates.Length; i++)
				{
					PartyCharacterTemplate partyCharacterTemplate = partyCharactersTemplateObject.PartyCharacterTemplates[i];
					partyCharacters[i] = new PartyCharacter(partyCharacterTemplate, this);
				}
				return partyCharacters;
			}
		}

		public Party(
			PartyCharactersTemplateObject partyCharactersTemplateObject,
			UnityEvent onStatsChanged,
			UnityEvent onInventoryItemsUpdated,
			int partyInventorySize,
			int startingScrip
			) //constructor 
		{
			this.partyCharactersTemplateObject = partyCharactersTemplateObject;
			this.onStatsChanged = onStatsChanged;
			this.onInventoryItemsUpdated = onInventoryItemsUpdated;
			partyInventory = new Inventory(partyInventorySize, startingScrip);
		}
	}
}