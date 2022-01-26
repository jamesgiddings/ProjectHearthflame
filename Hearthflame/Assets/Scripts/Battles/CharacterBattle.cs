using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBattle : MonoBehaviour
{
    private CharacterBase characterBase;

	private void Awake()
	{
		characterBase = GetComponent<CharacterBase>();
	}
	public void Setup(bool isPlayerTeam)
	{
		if (isPlayerTeam)
		{
			//set something to do with player team
		}
	}
}
