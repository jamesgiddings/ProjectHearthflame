using GramophoneUtils.Items;
using GramophoneUtils.Stats;
using NUnit.Framework;

public class ItemTests : BasicEditModeTest
{
    [Test]
    public void ItemStatsTestsSimplePasses()
    {
        StatType statType = Constants.Strength;
        StatModifier testModifier = new StatModifier(statType, ModifierNumericType.Flat, StatModifierType.Physical, 13f);
        Character1 = Character1Blueprint.Instance();

        float statBeforeEquip = Character1.StatSystem.GetStatValue(statType);

        EquipmentItem weaponItem = EquipmentItem1;
        Assert.That(weaponItem, Is.Not.Null);
        Assert.That(Character1, Is.Not.Null);
        weaponItem.Equip(Character1);

        Assert.AreEqual(statBeforeEquip + 5f, Character1.StatSystem.GetStat(statType).Value);
        weaponItem.Unequip(Character1);
        Assert.AreEqual(statBeforeEquip, Character1.StatSystem.GetStat(statType).Value);
    }
}
