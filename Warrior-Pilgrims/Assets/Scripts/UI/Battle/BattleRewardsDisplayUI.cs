using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BattleRewardsDisplayUI : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI rewardText;
	[SerializeField] private BattleManager battleManager;

	public void Initialise(BattleManager battleManager)
	{
		this.battleManager = battleManager;
		battleManager.BattleDataModel.OnBattleRewardsEarned += UpdateDisplay;
	}

	public void OnDestroy()
	{
		battleManager.BattleDataModel.OnBattleRewardsEarned -= UpdateDisplay;
	}

	public void UpdateDisplay(BattleReward battleReward)
	{
		rewardText.text = battleReward.GetRewardsInfoDisplayText();
		gameObject.SetActive(true);
	}
}
