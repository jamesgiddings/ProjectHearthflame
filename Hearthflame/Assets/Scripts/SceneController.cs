using GramophoneUtils.Stats;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneController
{
	public static void AdditiveLoadScene(Battle battle = null, Party party = null)
	{
		if (battle != null && party != null)
		{
			BattleManager battleManager = new BattleManager(battle, party);
		}
		SceneManager.LoadScene("BattleScene", LoadSceneMode.Additive);
	}
}
