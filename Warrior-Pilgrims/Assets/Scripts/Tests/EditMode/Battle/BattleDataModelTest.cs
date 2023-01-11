using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[TestFixture]
public class BattleDataModelTest : BasicEditModeTest
{
    private BattleDataModel _battleDataModel;

    [SetUp]
    protected override void SetUp()
    {
        base.SetUp();
        _battleDataModel = TestObjectReferences.BattleDataModel;
    }

    [Test]
    public void TestBattleDataModelIsNotNull()
    {
        Assert.NotNull(_battleDataModel);
    }

    [Test]
    public void TestMockObject()
    {
        Assert.AreEqual(testObject, MockSaveable.CaptureState());
    }
}