using GramophoneUtils.Items;
using GramophoneUtils.Items.Containers;
using GramophoneUtils.Stats;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using GramophoneUtils.Battles;

[CreateAssetMenu(fileName = "New Battle", menuName = "Battles/Battle")]
public class Battle : Data
{
    [Header("Battle Trigger")]
    [SerializeField] private bool _deactivateOnComplete;

	[PreviewField(60), HideLabel]
	[HorizontalGroup("Split", 60)]
    [SerializeField] private Sprite _battleSprite;

    [VerticalGroup("Split/Right"), LabelWidth(120)]
    [SerializeField] private BattleReward _battleReward;
    [VerticalGroup("Split/Right"), LabelWidth(120)]
    [SerializeField] private GameObject _battleMusic;

    [Header("Battle Opponents")]
    [TableList(ShowIndexLabels = true)]
    [SerializeField] private ItemSlot[] _itemSlots;
    [TableList(DrawScrollView = true, MaxScrollViewHeight = 200, MinScrollViewHeight = 100)]
    [SerializeField] private CharacterTemplate[] _battleCharacterTemplates;

	[SerializeField] private BattleWinConditions _battleWinConditions;

	[Header("Battle Rear")]
	[SerializeField] private BattleRear _battleRear;

    public BattleReward BattleReward => _battleReward;

	public Sprite BattleSprite => _battleSprite;

	private Inventory _enemyInventory = new Inventory(20, 10000);

    public bool IsComplete { get { return _isComplete; } set { _isComplete = value; } }
    
    private bool _isComplete = false;

	public bool DeactivateOnComplete => _deactivateOnComplete;

    private List<Character> _battleCharacters;

	public CharacterTemplate[] BattleCharacterTemplates => _battleCharacterTemplates; // getter

    public BattleWinConditions BattleWinConditions => _battleWinConditions;

    public BattleRear BattleRear => _battleRear;

    public Inventory EnemyInventory => _enemyInventory; // getter

	public List<Character> BattleCharacters
	{
		get
		{
			if (_battleCharacters != null) { return _battleCharacters; }
			_battleCharacters = InstanceCharacters();
			return _battleCharacters;
		}
	}

	public List<Character> InstanceCharacters()
	{
		_battleCharacters = new List<Character>();
		for (int i = 0; i < BattleCharacterTemplates.Length; i++)
		{
			if (_battleCharacterTemplates[i] != null)
			{
				_battleCharacters.Add(new Character(_battleCharacterTemplates[i], _enemyInventory));
				_battleCharacters[i].IsPlayer = false;
			}
		}
		return _battleCharacters;
	}


}