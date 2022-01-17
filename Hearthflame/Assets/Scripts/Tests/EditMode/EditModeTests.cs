using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using GramophoneUtils.Stats;
using GramophoneUtils.Items;
using GramophoneUtils.Items.Containers;

public class EditModeTests
{
	[Test]
	public void StatsTestsSimplePasses()
	{
		string relativePath = "Assets/GameData/Stats/Strength.asset";
		StatType statType = AssetDatabase.LoadAssetAtPath<StatType>(relativePath);
		StatModifier testModifier = new StatModifier(statType, StatModifierTypes.Flat, 13f, 5);
		Character character = new Character();

		Assert.AreEqual(0, character.StatSystem.GetStat(statType).Modifiers.Count); // the new strength stat in statSystem should have no modifiers so far.

		character.StatSystem.AddModifier(testModifier);

		Assert.AreEqual(1, character.StatSystem.GetStat(statType).Modifiers.Count); // the new strength stat in statSystem should have 1 modifier now.

		int iterations = 0;
		while (iterations < 5)
		{
			Turn.AdvanceTurn();
			iterations++;
		}
		Assert.AreEqual(0, character.StatSystem.GetStat(statType).Modifiers.Count); // the new strength stat in statSystem should have had its modifier removed after it timed out.

		float currentStatValue = character.StatSystem.GetStat(statType).Value;
		StatModifier testModifier2 = new StatModifier(statType, StatModifierTypes.Flat, 15f, -1);
		StatModifier testModifier3 = new StatModifier(statType, StatModifierTypes.Flat, 8f, -1);
		character.StatSystem.AddModifier(testModifier2);
		character.StatSystem.AddModifier(testModifier3);

		Assert.AreEqual(currentStatValue + 15f + 8f, character.StatSystem.GetStat(statType).Value); // the new strength stat in statSystem should have had its modifier removed after it timed out.

		relativePath = "Assets/GameData/Stats/Magic.asset";
		statType = AssetDatabase.LoadAssetAtPath<StatType>(relativePath);

		currentStatValue = character.StatSystem.GetStat(statType).Value;
		testModifier = new StatModifier(statType, StatModifierTypes.PercentAdditive, 0.50f, 5);
		testModifier2 = new StatModifier(statType, StatModifierTypes.PercentAdditive, 0.50f, 5);

		character.StatSystem.AddModifier(testModifier);
		character.StatSystem.AddModifier(testModifier2);

		Assert.AreEqual(currentStatValue * 2, character.StatSystem.GetStat(statType).Value); // check that two additive 50% percent mods behave as expected

		iterations = 0;
		while (iterations < 5)
		{
			Turn.AdvanceTurn();
			iterations++;
		}

		Assert.AreEqual(currentStatValue, character.StatSystem.GetStat(statType).Value); // check the two additive buffs have ticked off.

		testModifier = new StatModifier(statType, StatModifierTypes.Flat, 10f, 5);

		testModifier3 = new StatModifier(statType, StatModifierTypes.PercentMultiplicative, 1f, 5);
		character.StatSystem.AddModifier(testModifier);
		character.StatSystem.AddModifier(testModifier2);
		character.StatSystem.AddModifier(testModifier3);

		Assert.AreEqual(((currentStatValue + 10f) * 1.5f) * 2f, character.StatSystem.GetStat(statType).Value); // check the value of modifiers when flat and additive and multiplicative are used is as expected 

		iterations = 0;
		while (iterations < 5)
		{
			Turn.AdvanceTurn();
			iterations++;
		}

		Assert.AreEqual(currentStatValue, character.StatSystem.GetStat(statType).Value); // check all modifiers have been removed following 5 ticks
	}

	[Test]
	public void ItemStatsTestsSimplePasses()
	{
		string relativePath = "Assets/GameData/Stats/Strength.asset";
		StatType statType = AssetDatabase.LoadAssetAtPath<StatType>(relativePath);
		StatModifier testModifier = new StatModifier(statType, StatModifierTypes.Flat, 13f, 5);
		Character statSystem = new Character();

		float statBeforeEquip = statSystem.StatSystem.GetStatValue(statType);
		
		string relativePathWeapon = "Assets/Resources/Items/Weapons/Sword of Nipsying.asset";
		EquipmentItem weaponItem = AssetDatabase.LoadAssetAtPath<EquipmentItem>(relativePathWeapon);
		weaponItem.Equip(statSystem);

		Assert.AreEqual(statBeforeEquip + 5f, statSystem.StatSystem.GetStat(statType).Value);
		weaponItem.Unequip(statSystem);
		Assert.AreEqual(statBeforeEquip, statSystem.StatSystem.GetStat(statType).Value);
	}

	[Test]
	public void ResourceIDTestSimplePasses()
	{

		string relativePath = "Assets/Scripts/Tests/EditMode/de301b3a5de18c7428d78e832f4900a2.asset";
		Test resource = AssetDatabase.LoadAssetAtPath<Test>(relativePath);
		Debug.Log(resource.UID);
		Assert.AreEqual(resource.UID, "de301b3a5de18c7428d78e832f4900a2");
	}

	[Test]
	public void LevelSystemTests()
	{
		string relativePath = "Assets/GameData/Classes/HearthPriest.asset";
		CharacterClass characterClass = AssetDatabase.LoadAssetAtPath<CharacterClass>(relativePath);
		Character character = new Character();
		LevelSystem levelSystem = new LevelSystem(characterClass, character);
		Debug.Log(levelSystem.GetLevel());
		levelSystem.AddExperience(50);
		Debug.Log(levelSystem.GetLevel());
		levelSystem.AddExperience(60);
		Debug.Log(levelSystem.GetLevel());
	}
}
