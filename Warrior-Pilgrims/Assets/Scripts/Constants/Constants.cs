using GramophoneUtils.Magic;
using GramophoneUtils.Stats;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "Constants Object", menuName = "Constants/Global Constants")]
public class Constants : ScriptableObject
{
    [BoxGroup("Characters")]
    [SerializeField] private int _numberOfFrontCharacters = 3;
    public int NumberOfFrontCharacters => _numberOfFrontCharacters;

    [BoxGroup("Characters")]
    [SerializeField] private int _numberOfRearCharacters = 3;
    public int NumberOfRearCharacters => _numberOfRearCharacters;

    [BoxGroup("Stats")]
    [BoxGroup("Stats/StatTypes")]
    [BoxGroup("Stats/StatTypes/Base Stats")]
    [SerializeField] private StatType _dexterity;
    public StatType Dexterity => _dexterity;

    [BoxGroup("Stats/StatTypes/Base Stats")]
    [SerializeField] private StatType _magic;
    public StatType Magic => _magic;

    [BoxGroup("Stats/StatTypes/Base Stats")]
    [SerializeField] private StatType _resilience;
    public StatType Resilience => _resilience;

    [BoxGroup("Stats/StatTypes/Base Stats")]
    [SerializeField] private StatType _speed;
    public StatType Speed => _speed;

    [BoxGroup("Stats/StatTypes/Base Stats")]
    [SerializeField] private StatType _strength;
    public StatType Strength => _strength;

    [BoxGroup("Stats/StatTypes/Base Stats")]
    [SerializeField] private StatType _wits;
    public StatType Wits => _wits;

    [BoxGroup("Stats/StatTypes/Defence")]
    [SerializeField] private StatType _fireArmour;
    public StatType FireArmour => _fireArmour;

    [BoxGroup("Stats/StatTypes/Defence")]
    [SerializeField] private StatType _magicEvasion;
    public StatType MagicEvasion => _magicEvasion;

    [BoxGroup("Stats/StatTypes/Defence")]
    [SerializeField] private StatType _meleeEvasion;
    public StatType MeleeEvasion => _meleeEvasion;

    [BoxGroup("Stats/StatTypes/Defence")]
    [SerializeField] private StatType _physicalArmour;
    public StatType PhysicalArmour => _physicalArmour;

    [BoxGroup("Stats/StatTypes/Defence")]
    [SerializeField] private StatType _rangedEvasion;
    public StatType RangedEvasion => _rangedEvasion;

    [BoxGroup("Stats/StatTypes/Offence")]
    [SerializeField] private StatType _magicAccuracy;
    public StatType MagicAccuracy => _magicAccuracy;

    [BoxGroup("Stats/StatTypes/Offence")]
    [SerializeField] private StatType _meleeAccuracy;
    public StatType MeleeAccuracy => _meleeAccuracy;

    [BoxGroup("Stats/StatTypes/Offence")]
    [SerializeField] private StatType _rangedAccuracy;
    public StatType RangedAccuracy => _rangedAccuracy;

    [BoxGroup("Stats/Damage Types")]
    [SerializeField] private Element _air;
    public Element Air => _air;

    [BoxGroup("Stats/Damage Types")]
    [SerializeField] private Element _arcane;
    public Element Arcane => _arcane;

    [BoxGroup("Stats/Damage Types")]
    [SerializeField] private Element _dark;
    public Element Dark => _dark;

    [BoxGroup("Stats/Damage Types")]
    [SerializeField] private Element _earth;
    public Element Earth => _earth;

    [BoxGroup("Stats/Damage Types")]
    [SerializeField] private Element _fire;
    public Element Fire => _fire;

    [BoxGroup("Stats/Damage Types")]
    [SerializeField] private Element _light;
    public Element Light => _light;

    [BoxGroup("Stats/Damage Types")]
    [SerializeField] private Element _physical;
    public Element Physical => _physical;

    [BoxGroup("Stats/Damage Types")]
    [SerializeField] private Element _water;
    public Element Water => _water;

    [BoxGroup("Battle/Battle Delays")]
    [SerializeField] private float _battleShortDelay = 0.5f;
    public float BattleShortDelay => _battleShortDelay;

    [BoxGroup("Battle/Battle Delays")]
    [SerializeField] private float _battleMediumDelay = 1f;
    public float BattleMediumDelay => _battleMediumDelay;

    [BoxGroup("Battle/Battle Delays")]
    [SerializeField] private float _battleLongDelay = 2f;
    public float BattleLongDelay => _battleLongDelay;

    [BoxGroup("Battle")]
    [BoxGroup("Battle/Floating Text")]
    [SerializeField] private string _magicEvasionText = "Resist!";
    public string MagicEvasionText => _magicEvasionText;

    [BoxGroup("Battle/Floating Text")]
    [SerializeField] private string _physicalEvasionText = "Miss!";
    public string PhysicalEvasionText => _physicalEvasionText;
}
