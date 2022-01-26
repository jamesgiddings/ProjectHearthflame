using GramophoneUtils.Stats;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneController
{
	public static void AdditiveLoadScene(Battle battle = null, PlayerBehaviour player = null)
	{
		if (battle != null && player != null)
		{
			BattleManager battleManager = new BattleManager(battle, player);
		}
		SceneManager.LoadScene("BattleScene", LoadSceneMode.Additive);
	}
}
