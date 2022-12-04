using GramophoneUtils.Items;
using GramophoneUtils.Items.Containers;
using GramophoneUtils.Stats;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "New Battle", menuName = "Battles/Battle")]
public class Battle : Data
{
    [Header("Battle Trigger")]
    [SerializeField] private bool deactivateOnComplete;

	[PreviewField(60), HideLabel]
	[HorizontalGroup("Split", 60)]
    [SerializeField] private Sprite battleSprite;

    [VerticalGroup("Split/Right"), LabelWidth(120)]
    [SerializeField] private BattleReward battleReward;
    [VerticalGroup("Split/Right"), LabelWidth(120)]
    [SerializeField] private GameObject battleMusic;

    [Header("Battle Opponents")]
    [TableList(ShowIndexLabels = true)]
    [SerializeField] private ItemSlot[] itemSlots;
    [TableList(DrawScrollView = true, MaxScrollViewHeight = 200, MinScrollViewHeight = 100)]
    [SerializeField] private CharacterTemplate[] battleCharacterTemplates;

    public BattleReward BattleReward => battleReward;

	public Sprite BattleSprite => battleSprite;

	private Inventory enemyInventory = new Inventory(20, 10000);

    public bool IsComplete { get { return isComplete; } set { isComplete = value; } }
    
    private bool isComplete = false;

	public bool DeactivateOnComplete => deactivateOnComplete;

    private List<Character> battleCharacters;

	public CharacterTemplate[] BattleCharacterTemplates => battleCharacterTemplates; // getter

	public Inventory EnemyInventory => enemyInventory; // getter

	public List<Character> BattleCharacters
	{
		get
		{
			if (battleCharacters != null) { return battleCharacters; }
			battleCharacters = InstanceCharacters();
			return battleCharacters;
		}
	}

	public List<Character> InstanceCharacters()
	{
		battleCharacters = new List<Character>();
		for (int i = 0; i < BattleCharacterTemplates.Length; i++)
		{
			if (battleCharacterTemplates[i] != null)
			{
				battleCharacters.Add(new Character(battleCharacterTemplates[i], enemyInventory));
				battleCharacters[i].IsPlayer = false;
			}
		}
		return battleCharacters;
	}


}