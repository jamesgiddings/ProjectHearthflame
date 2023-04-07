using GramophoneUtils.Items;
using GramophoneUtils.Items.Containers;
using GramophoneUtils.Stats;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using GramophoneUtils.Battles;
using GramophoneUtils.Characters;
using AYellowpaper;

[CreateAssetMenu(fileName = "New Battle", menuName = "Battles/Battle")]
public class Battle : Data
{
    #region Attributes/Fields

    [Header("Battle Trigger")]
    [SerializeField] private bool _deactivateOnComplete;

	[PreviewField(60), HideLabel]
	[HorizontalGroup("Split", 60)]
    [SerializeField] private Sprite _battleSprite;
    public Sprite BattleSprite => _battleSprite;

    [VerticalGroup("Split/Right"), LabelWidth(120)]
    [SerializeField] private BattleReward _battleReward;
    [VerticalGroup("Split/Right"), LabelWidth(120)]
    [SerializeField] private GameObject _battleMusic;

    [Header("Battle Opponents")]
    [SerializeField] private ItemSlot[] _itemSlots;
    [SerializeField] private InterfaceReference<ICharacter>[] _battleCharacterBlueprints;

	[SerializeField] private BattleWinConditions _battleWinConditions;

	[Header("Battle Rear")]
	[SerializeField] private BattleRear _battleRear;

    public BattleReward BattleReward => _battleReward;

	

    [SerializeField] private Inventory _enemyInventory;
    public Inventory EnemyInventory =>_enemyInventory;

    public bool IsComplete { get { return _isComplete; } set { _isComplete = value; } }
    
    private bool _isComplete = false;

	public bool DeactivateOnComplete => _deactivateOnComplete;

    public BattleWinConditions BattleWinConditions => _battleWinConditions;

    public BattleRear BattleRear => _battleRear;

	public List<ICharacter> BattleCharacters
	{
		get
		{
			return InstanceCharacters();
		}
	}

    #endregion

    #region Utilities

    private List<ICharacter> InstanceCharacters()
    {
        List<ICharacter> _battleCharacters = new List<ICharacter>();
        for (int i = 0; i < _battleCharacterBlueprints.Length; i++)
        {
            if (_battleCharacterBlueprints[i] != null)
            {
                _battleCharacters.Add(_battleCharacterBlueprints[i].Value.Instance());
                _battleCharacters[i].IsPlayer = false;
                _battleCharacters[i].PartyInventory = _enemyInventory;
            }
        }
        return _battleCharacters;
    }

    #endregion


}