using GramophoneUtils.Items.Containers;
using GramophoneUtils.Stats;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ItemTests : BasicEditModeTest
{
    [Test]
    public void ItemStatsTestsSimplePasses()
    {
        StatType statType = Constants.Strength;
        StatModifier testModifier = new StatModifier(statType, StatModifierTypes.Flat, 13f, 5);
        Character1 = Character1Blueprint.Instance();

        float statBeforeEquip = Character1.StatSystem.GetStatValue(statType);

        EquipmentItem weaponItem = EquipmentItem1;

        weaponItem.Equip(Character1);

        Assert.AreEqual(statBeforeEquip + 5f, Character1.StatSystem.GetStat(statType).Value);
        weaponItem.Unequip(Character1);
        Assert.AreEqual(statBeforeEquip, Character1.StatSystem.GetStat(statType).Value);
    }
}
