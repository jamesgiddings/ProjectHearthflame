using GramophoneUtils.Items.Containers;
using UnityEngine;
using UnityEngine.Events;

namespace GramophoneUtils.Stats
{
	[CreateAssetMenu(fileName = "New Party", menuName = "Characters/Party")]
	public class Party : Data
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
	}
}