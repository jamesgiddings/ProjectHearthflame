using GramophoneUtils.Items.Containers;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace GramophoneUtils.Stats
{
	[CreateAssetMenu(fileName = "New Party Template", menuName = "Characters/Party Template")]
	public class PartyTemplate : Data, IBlueprint
	{
		[SerializeField] public UnityEvent onStatsChanged;
		[SerializeField] public UnityEvent onInventoryItemsUpdated;

		[SerializeField] private int partyInventorySize;
		[SerializeField] private int partyInventoryStartingScrip;

		[SerializeField] private PartyCharactersTemplateObject partyCharactersTemplateObject;

		private PartyCharacter[] partyCharacters;

		public T CreateBlueprintInstance<T>(object source = null)
		{
			Party party = new Party(
				partyCharactersTemplateObject,
				onStatsChanged,
				onInventoryItemsUpdated,
				partyInventorySize,
				partyInventoryStartingScrip
				);
			return (T)Convert.ChangeType(party, typeof(T));
		}
	}
}