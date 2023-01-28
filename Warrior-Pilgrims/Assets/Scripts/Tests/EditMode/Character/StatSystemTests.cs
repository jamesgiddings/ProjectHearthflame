using System.Collections;
using System.Collections.Generic;
using GramophoneUtils.Characters;
using GramophoneUtils.Stats;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;

public class StatSystemTests : BasicEditModeTest
{
    protected override void SetUp()
    {
        base.SetUp();
    }

    // A Test behaves as an ordinary method
    [Test]
    public void StatSystemTestsSimplePasses()
    {
        // Use the Assert class to test conditions
    }

    [Test]
    public void StatsTestsSimplePasses()
    {
        Character character = Character1Blueprint.Instance();
        StatModifier testModifier = new StatModifier(Constants.Strength, StatModifierTypes.Flat, 13f, 5);

        Assert.NotNull(character);
        Assert.NotNull(Constants.Strength);

        Assert.AreEqual(0, character.StatSystem.GetStat(Constants.Strength).StatModifiers.Count); // the new strength stat in statSystem should have no modifiers so far.

        character.StatSystem.AddModifier(testModifier);

        Assert.AreEqual(1, character.StatSystem.GetStat(Constants.Strength).StatModifiers.Count); // the new strength stat in statSystem should have 1 modifier now.

        int iterations = 0;
        while (iterations < 5)
        {
            Turn.AdvanceTurn();
            iterations++;
        }
        Assert.AreEqual(0, character.StatSystem.GetStat(Constants.Strength).StatModifiers.Count); // the new strength stat in statSystem should have had its modifier removed after it timed out.

        float currentStatValue = character.StatSystem.GetStat(Constants.Strength).Value;
        StatModifier testModifier2 = new StatModifier(Constants.Strength, StatModifierTypes.Flat, 15f, -1);
        StatModifier testModifier3 = new StatModifier(Constants.Strength, StatModifierTypes.Flat, 8f, -1);
        character.StatSystem.AddModifier(testModifier2);
        character.StatSystem.AddModifier(testModifier3);

        Assert.AreEqual(currentStatValue + 15f + 8f, character.StatSystem.GetStat(Constants.Strength).Value); // the new strength stat in statSystem should have had its modifier removed after it timed out.


        currentStatValue = character.StatSystem.GetStat(Constants.Strength).Value;
        testModifier = new StatModifier(Constants.Strength, StatModifierTypes.PercentAdditive, 0.50f, 5);
        testModifier2 = new StatModifier(Constants.Strength, StatModifierTypes.PercentAdditive, 0.50f, 5);

        character.StatSystem.AddModifier(testModifier);
        character.StatSystem.AddModifier(testModifier2);

        Assert.AreEqual(currentStatValue * 2, character.StatSystem.GetStat(Constants.Strength).Value); // check that two additive 50% percent mods behave as expected

        iterations = 0;
        while (iterations < 5)
        {
            Turn.AdvanceTurn();
            iterations++;
        }

        Assert.AreEqual(currentStatValue, character.StatSystem.GetStat(Constants.Strength).Value); // check the two additive buffs have ticked off.

        testModifier = new StatModifier(Constants.Strength, StatModifierTypes.Flat, 10f, 5);

        testModifier3 = new StatModifier(Constants.Strength, StatModifierTypes.PercentMultiplicative, 1f, 5);
        character.StatSystem.AddModifier(testModifier);
        character.StatSystem.AddModifier(testModifier2);
        character.StatSystem.AddModifier(testModifier3);

        Assert.AreEqual(((currentStatValue + 10f) * 1.5f) * 2f, character.StatSystem.GetStat(Constants.Strength).Value); // check the value of modifiers when flat and additive and multiplicative are used is as expected 

        iterations = 0;
        while (iterations < 5)
        {
            Turn.AdvanceTurn();
            iterations++;
        }

        Assert.AreEqual(currentStatValue, character.StatSystem.GetStat(Constants.Strength).Value); // check all modifiers have been removed following 5 ticks
    }


    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    [UnityTest]
    public IEnumerator StatSystemTestsWithEnumeratorPasses()
    {
        // Use the Assert class to test conditions.
        // Use yield to skip a frame.
        yield return null;
    }
}
