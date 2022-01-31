using GramophoneUtils.Stats;
using UnityEngine;

[CreateAssetMenu(fileName = "New Battle", menuName = "Battles/Battle")]
public class Battle : Data
{
	internal static object battleReward;
	[SerializeField] private Party enemyParty;
	[SerializeField] private BattleReward battleRewards;

	public Party EnemyParty => enemyParty;
	public BattleReward BattleReward => battleRewards;
}