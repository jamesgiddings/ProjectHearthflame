using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class LevelSystemTests : BasicEditModeTest
{
    [Test]
    public void LevelSystemTest()
    {
        Character1 = Character1Blueprint.Instance();
        Character1.LevelSystem.AddExperience(50);
        Assert.AreEqual(0, Character1.LevelSystem.GetLevel());
        Character1.LevelSystem.AddExperience(2000);
        Assert.AreEqual(1, Character1.LevelSystem.GetLevel());
        Character1.LevelSystem.AddExperience(100000000);
        Assert.AreEqual(19, Character1.LevelSystem.GetLevel());
    }
}
