using GramophoneUtils.Stats;
using UnityEngine;

[CreateAssetMenu(fileName = "New Battle", menuName = "Battles/Battle")]
public class Battle : Data
{
	internal static object battleReward;
	[SerializeField] private PartyTemplate enemyPartyTemplate;
	[SerializeField] private BattleReward battleRewards;

	private Party enemyParty;

	public Party EnemyParty
	{
		get
		{
			if (enemyParty != null) { return enemyParty; }
			enemyParty = enemyPartyTemplate.CreateBlueprintInstance<Party>();
			return enemyParty;
		}
	}
	public BattleReward BattleReward => battleRewards;
}