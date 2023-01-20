using AYellowpaper;
using GramophoneUtils.Characters;
using GramophoneUtils.Items.Containers;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "Test Object References", menuName = "Testing/Test Object References")]
public class TestObjectReferences : ScriptableObject
{
    [BoxGroup("Test Characters")]
    [SerializeField] private Character _character1;
    public Character Character1 => _character1;

    [BoxGroup("Test Characters")]
    [SerializeField] private Character _character2;
    public Character Character2 => _character2;

    [BoxGroup("Test Characters")]
    [SerializeField] private Character _character3;
    public Character Character3 => _character3;

    [BoxGroup("Test Characters")]
    [SerializeField] private Character _enemy1;
    public Character Enemy1 => _enemy1;

    [BoxGroup("Test Characters")]
    [SerializeField] private Character _enemy2;
    public Character Enemy2 => _enemy2;

    [BoxGroup("Test Characters")]
    [SerializeField] private Character _enemy3;
    public Character Enemy3 => _enemy3;

    [BoxGroup("Character Classes")]
    [SerializeField] private CharacterClass _hearthPriest;
    public CharacterClass HearthPriest => _hearthPriest;

    [BoxGroup("Character Classes")]
    [SerializeField] private CharacterClass _duelist;
    public CharacterClass Duelist => _duelist;

    [BoxGroup("Character Classes")]
    [SerializeField] private CharacterClass _theRuneKnight;
    public CharacterClass TheRuneKnight => _theRuneKnight;

    [BoxGroup("Character Classes")]
    [SerializeField] private CharacterClass _musketeer;
    public CharacterClass Musketeer => _musketeer;

    [BoxGroup("Resource UID")]
    [SerializeField] private Resource _resource;
    public Resource Resource => _resource;

    [BoxGroup("Test Items")]
    [SerializeField] private EquipmentItem _equipmentItem1;
    public EquipmentItem EquipmentItem1 => _equipmentItem1;

    [BoxGroup("Battle Objects")]
    [SerializeField] private BattleDataModel _battleDataModel;
    public BattleDataModel BattleDataModel => _battleDataModel;

    [BoxGroup("Battle Objects")]
    [SerializeField] private BattleManager _battleManager;
    public BattleManager BattleManager => _battleManager;

    [BoxGroup("Battle Objects/Targeting")]
    [SerializeField] private TargetToSlots _targetToSlots_XXXX_OXXX;
    public TargetToSlots TargetToSlots_XXXX_OXXX => _targetToSlots_XXXX_OXXX;

    [BoxGroup("Battle Objects/Targeting")]
    [SerializeField] private TargetToSlots _targetToSlots_XXXX_OOXXandXXXX_XXOO;
    public TargetToSlots TargetToSlots_XXXX_OOXXandXXXX_XXOO => _targetToSlots_XXXX_OOXXandXXXX_XXOO;

    [BoxGroup("Battle Objects/Targeting")]
    [SerializeField] private TargetToSlots _targetToSlots_XXXX_OOXXandXXXX_XXOOandXXXX_XXOXandXXXX_XXXO;
    public TargetToSlots TargetToSlots_XXXX_OOXXandXXXX_XXOOandXXXX_XXOXandXXXX_XXXO => _targetToSlots_XXXX_OOXXandXXXX_XXOOandXXXX_XXOXandXXXX_XXXO;

    [BoxGroup("Battle Objects/Targeting")]
    [SerializeField] private UseFromSlot _OOOO;
    public UseFromSlot OOOO => _OOOO;

    [BoxGroup("Battle Objects/Targeting")]
    [SerializeField] private UseFromSlot _XXXO;
    public UseFromSlot XXXO => _XXXO;

    [BoxGroup("Battle Objects/AI")]
    [SerializeField] private RandomSkillObjectCollection _randomSkillObjectCollection;
    public RandomSkillObjectCollection RandomSkillObjectCollection => _randomSkillObjectCollection;

    [BoxGroup("Battle Objects/AI")]
    [SerializeField] private RandomCharacterClassObjectCollection _randomCharacterClassObjectCollection;
    public RandomCharacterClassObjectCollection RandomCharacterClassObjectCollection => _randomCharacterClassObjectCollection;

    [BoxGroup("Battle Objects/Skills")]
    [SerializeField] private InterfaceReference<ISkill> _cleave;
    public ISkill Cleave => _cleave.Value;

    [BoxGroup("Battle Objects/Skills")]
    [SerializeField] private InterfaceReference<ISkill> _bash;
    public ISkill Bash => _bash.Value;

    [BoxGroup("Battle Objects/Skills")]
    [SerializeField] private InterfaceReference<ISkill> _shoot;
    public ISkill Shoot => _shoot.Value;

    [BoxGroup("Battle Objects/Skills")]
    [SerializeField] private InterfaceReference<ISkill> _magicShield;
    public ISkill MagicShield => _magicShield.Value;

    [BoxGroup("Battle Objects/Battle")]
    [SerializeField] private Battle _battle1;
    public Battle Battle1 => _battle1;

    [BoxGroup("Test States")]
    [SerializeField] private State _testState;
    public State TestState => _testState;

    [BoxGroup("Items")]
    [BoxGroup("Items/Containers")]
    [SerializeField] private Inventory _partyInventory;
    public Inventory PartyInventory => _partyInventory;
}
