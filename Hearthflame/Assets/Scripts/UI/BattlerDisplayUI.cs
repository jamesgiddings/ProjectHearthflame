using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattlerDisplayUI : MonoBehaviour
{
	[SerializeField]
	
	private BattleManager battleManager;

	private CharacterInventory battlersListNew;
	private CharacterInventory orderedBattlersListNew;
	private CharacterInventory enemyBattlersList;
	private CharacterInventory playerBattlersList;

	
	public void Initialise(BattleManager battleManager)
	{

	}
}
