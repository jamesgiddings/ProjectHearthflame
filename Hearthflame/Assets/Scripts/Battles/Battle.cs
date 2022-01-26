using GramophoneUtils.Stats;
using UnityEngine;

[CreateAssetMenu(fileName = "New Battle", menuName = "Battles/Battle")]
public class Battle : Data
{
	internal static object battleReward;
	[SerializeField] private CharacterTemplate[] enemyBattlers;
	[SerializeField] private BattleReward battleRewards;

	public CharacterTemplate[] EnemyBattlers => enemyBattlers;
	public BattleReward BattleReward => battleRewards;
}