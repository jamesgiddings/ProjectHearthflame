using System.Threading;
using GramophoneUtils.Characters;
using GramophoneUtils.Stats;
using NUnit.Framework;
using UnityEngine;

public class StatSystemTests : BasicEditModeTest
{
    protected override void SetUp()
    {
        base.SetUp();
    }

/*    [Test]
    public void StatsTestsSimplePasses()
    { 
    
    }*/

    [Test]
    public void StatsTestsSimplePasses()
    {
        Character character = Character1Blueprint.Instance();
        StatModifier testModifier = new StatModifier(
            "",
            "",
            null,
            Constants.Strength, 
            ModifierNumericType.Flat, 
            StatModifierType.Physical, 
            13f);

        Assert.NotNull(character);
        Assert.NotNull(Constants.Strength);

        Assert.AreEqual(0, character.StatSystem.GetStat(Constants.Strength).StatModifiers.Count); // the new strength stat in statSystem should have no modifiers so far.

        character.StatSystem.AddModifier(testModifier);

        Assert.AreEqual(1, character.StatSystem.GetStat(Constants.Strength).StatModifiers.Count); // the new strength stat in statSystem should have 1 modifier now.

        int iterations = 0;
        while (iterations < 5)
        {
            character.AdvanceCharacterTurn();
            iterations++;
        }
        Assert.AreEqual(1, character.StatSystem.GetStat(Constants.Strength).StatModifiers.Count); // the new strength stat in statSystem should have had its modifier removed after it timed out.

        float currentStatValue = character.StatSystem.GetStat(Constants.Strength).Value;
        StatModifier testModifier2 = new StatModifier(
            "",
            "",
            null,
            Constants.Strength, 
            ModifierNumericType.Flat, 
            StatModifierType.Physical, 
            15f
            );
        StatModifier testModifier3 = new StatModifier(
            "",
            "",
            null, 
            Constants.Strength, 
            ModifierNumericType.Flat, 
            StatModifierType.Physical, 
            8f
            );
        character.StatSystem.AddModifier(testModifier2);
        character.StatSystem.AddModifier(testModifier3);

        Assert.AreEqual(currentStatValue + 15f + 8f, character.StatSystem.GetStat(Constants.Strength).Value); // the new strength stat in statSystem should have had its modifier removed after it timed out.


        currentStatValue = character.StatSystem.GetStat(Constants.Strength).Value;
        testModifier = new StatModifier(
            "",
            "",
            null, 
            Constants.Strength, 
            ModifierNumericType.PercentAdditive, 
            StatModifierType.Physical, 
            0.50f);
        testModifier2 = new StatModifier(
            "",
            "",
            null, 
            Constants.Strength, 
            ModifierNumericType.PercentAdditive, 
            StatModifierType.Physical, 0.50f
            );

        character.StatSystem.AddModifier(testModifier);
        character.StatSystem.AddModifier(testModifier2);

        Assert.AreEqual(currentStatValue * 2, character.StatSystem.GetStat(Constants.Strength).Value); // check that two additive 50% percent mods behave as expected

        character.StatSystem.RemoveModifier(testModifier);
        character.StatSystem.RemoveModifier(testModifier2);

        Assert.AreEqual(currentStatValue, character.StatSystem.GetStat(Constants.Strength).Value); // check the two additive buffs have ticked off.

        testModifier = new StatModifier(
            "",
            "",
            null, 
            Constants.Strength, 
            ModifierNumericType.Flat, 
            StatModifierType.Physical, 
            10f
            );

        testModifier3 = new StatModifier(
            "",
            "",
            null, 
            Constants.Strength, 
            ModifierNumericType.PercentMultiplicative, 
            StatModifierType.Physical, 
            1f
            );

        character.StatSystem.AddModifier(testModifier);
        character.StatSystem.AddModifier(testModifier2);
        character.StatSystem.AddModifier(testModifier3);

        Assert.AreEqual(((currentStatValue + 10f) * 1.5f) * 2f, character.StatSystem.GetStat(Constants.Strength).Value); // check the value of modifiers when flat and additive and multiplicative are used is as expected 

        character.StatSystem.RemoveModifier(testModifier);
        character.StatSystem.RemoveModifier(testModifier2);
        character.StatSystem.RemoveModifier(testModifier3);

        Assert.AreEqual(currentStatValue, character.StatSystem.GetStat(Constants.Strength).Value); // check all modifiers have been removed following 5 ticks
    }

    [Test]
    public void Test_PercentageStatModifierCorrectlyReducesDexBy50Percent()
    {
        CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

        // Given StatModifier
        Character character = Character2Blueprint.Instance();
        IStatModifier fiftyPercentDexMinusStatModifier = ServiceLocatorObject.Instance.StatModifierFactory.CreateStatModifierFromValue(
            "",
            "",
            null,
            Constants.Dexterity,
            ModifierNumericType.PercentAdditive,
            StatModifierType.Physical,
            -0.5f
            );
        float currentDexterity = character.StatSystem.GetStat(Constants.Dexterity).Value;
        Assert.That(character, Is.Not.Null);
        Assert.That(fiftyPercentDexMinusStatModifier, Is.Not.Null);
        Assert.That(fiftyPercentDexMinusStatModifier.Value, Is.EqualTo(-0.5f));
        Assert.That(fiftyPercentDexMinusStatModifier.ModifierNumericType, Is.EqualTo(ModifierNumericType.PercentAdditive));
        Assert.That(fiftyPercentDexMinusStatModifier.StatType, Is.EqualTo(Constants.Dexterity));

        // When
        character.StatSystem.AddModifier(fiftyPercentDexMinusStatModifier);

        // Then
        Assert.That(character.StatSystem.GetStat(Constants.Dexterity).Value, Is.EqualTo(currentDexterity / 2f));
    }

    [Test]
    public void Test_PercentageStatModifierCorrectlyIncreasesSpeedBy100Percent()
    {
        CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

        // Given StatModifier
        Character character = Character2Blueprint.Instance();
        IStatModifier fiftyPercentDexMinusStatModifier = ServiceLocatorObject.Instance.StatModifierFactory.CreateStatModifierFromValue(
            "",
            "",
            null,
            Constants.Speed,
            ModifierNumericType.PercentAdditive,
            StatModifierType.Physical,
            1f
            );
        float currentDexterity = character.StatSystem.GetStat(Constants.Speed).Value;
        Assert.That(character, Is.Not.Null);
        Assert.That(fiftyPercentDexMinusStatModifier, Is.Not.Null);
        Assert.That(fiftyPercentDexMinusStatModifier.Value, Is.EqualTo(1f));
        Assert.That(fiftyPercentDexMinusStatModifier.ModifierNumericType, Is.EqualTo(ModifierNumericType.PercentAdditive));
        Assert.That(fiftyPercentDexMinusStatModifier.StatType, Is.EqualTo(Constants.Speed));

        // When
        character.StatSystem.AddModifier(fiftyPercentDexMinusStatModifier);

        // Then
        Assert.That(character.StatSystem.GetStat(Constants.Speed).Value, Is.EqualTo(currentDexterity * 2f));
    }
}
