using GramophoneUtils.Items.Containers;
using GramophoneUtils.Stats;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Battle", menuName = "Battles/Battle")]
public class Battle : Data
{
	//[SerializeField] public UnityEvent onStatsChanged;

	//[SerializeField] public UnityEvent onInventoryItemsUpdated;

	private Inventory enemyInventory = new Inventory(20, 10000);
	private int partyInventorySize;
	private int startingScrip;

	[SerializeField] private CharacterTemplate[] battleCharacterTemplates;

	private List<Character> battleCharacters;

	public CharacterTemplate[] BattleCharacterTemplates => battleCharacterTemplates; // getter

	public Inventory EnemyInventory => enemyInventory; // getter

	public List<Character> BattleCharacters
	{
		get
		{
			Debug.Log("GIO");
			Debug.Log("battleCharacters != null: " + battleCharacters != null);
			if (battleCharacters != null) { return battleCharacters; }
			battleCharacters = InstanceCharacters();
			return battleCharacters;
		}
	}

	public List<Character> InstanceCharacters()
	{
		Debug.Log("Instancing battle characters.");
		battleCharacters = new List<Character>();
		for (int i = 0; i < BattleCharacterTemplates.Length; i++)
		{
			if (battleCharacterTemplates[i] != null)
			{
				battleCharacters.Add(new Character(battleCharacterTemplates[i], enemyInventory));
				battleCharacters[i].IsPlayer = false;
				//battleCharacters[i].IsRear = (i > 3) ? true : false;
			}
		}
		return battleCharacters;
	}

	[SerializeField] private BattleReward battleReward;

	public BattleReward BattleReward => battleReward;
}