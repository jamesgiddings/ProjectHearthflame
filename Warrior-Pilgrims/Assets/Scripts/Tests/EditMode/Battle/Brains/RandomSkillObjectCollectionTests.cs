using GramophoneUtils.Characters;
using NSubstitute.ExceptionExtensions;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class RandomSkillObjectCollectionTests : BasicEditModeTest
{
    #region Attributes/Fields/Properties

    private BattleDataModel _battleDataModel;

    private UseFromSlot _OOOO;
    private UseFromSlot _XXXO;

    private TargetToSlots _targetToSlots_XXXX_OXXX;
    private TargetToSlots _targetToSlots_XXXX_OOXXandXXXX_XXOO;
    private TargetToSlots _targetToSlots_XXXX_OOXXandXXXX_XXOOandXXXX_XXOXandXXXX_XXXO;

    private CharacterOrder _playerCharacterOrder;
    private CharacterOrder _enemyCharacterOrder;

    private RandomSkillObjectCollection _randomSkillObjectCollection;

    #endregion

    #region Constructors
    #endregion

    #region Callbacks
    #endregion

    #region Public Functions

    [SetUp]
    protected override void SetUp()
    {
        base.SetUp();

        Character1 = Character1Blueprint.Instance();
        Character1.IsPlayer = true;
        Character2 = Character2Blueprint.Instance();
        Character2.IsPlayer = true;
        Character3 = Character3Blueprint.Instance();
        Character3.IsPlayer = true;
        Character4 = Character3Blueprint.Instance();
        Character4.IsPlayer = true;

        EnemyCharacter1 = EnemyCharacter1Blueprint.Instance();
        Size2EnemyCharacter2 = Size2EnemyCharacter2Blueprint.Instance();
        EnemyCharacter3 = EnemyCharacter3Blueprint.Instance();

        _OOOO = TestObjectReferences.OOOO;
        _XXXO = TestObjectReferences.XXXO;

        _targetToSlots_XXXX_OXXX = TestObjectReferences.TargetToSlots_XXXX_OXXX;
        _targetToSlots_XXXX_OOXXandXXXX_XXOO = TestObjectReferences.TargetToSlots_XXXX_OOXXandXXXX_XXOO;
        _targetToSlots_XXXX_OOXXandXXXX_XXOOandXXXX_XXOXandXXXX_XXXO = TestObjectReferences.TargetToSlots_XXXX_OOXXandXXXX_XXOOandXXXX_XXOXandXXXX_XXXO;

        _playerCharacterOrder = new CharacterOrder(new ICharacter[] { Character1, Character2, Character3, Character4 });
        _enemyCharacterOrder = new CharacterOrder(new ICharacter[] { EnemyCharacter1, Size2EnemyCharacter2, EnemyCharacter3 });

        _randomSkillObjectCollection = TestObjectReferences.RandomSkillObjectCollection;
    }

    [Test]
    public void AssertRandomSkillObjectCollectionNotNull()
    {
        Assert.NotNull(_randomSkillObjectCollection);
    }

    [Test]
    public void AssertRandomSkillObjectCollectionRandomSkillObjectsNotNull()
    {
        Assert.NotNull(_randomSkillObjectCollection.RandomSkillObjects);
    }

    [Test]
    public void AssertRandomSkillObjectCollectionGetRandomObjectNotNull()
    {
        Assert.NotNull(_randomSkillObjectCollection.GetRandomObject());
    }

    [Test]
    public void AssertRandomSkillObjectCollectionBlacklistWorks()
    {
        List<string> skills = new List<string>();

        for (int i = 0; i < _randomSkillObjectCollection.RandomSkillObjects.Count; i++)
        {
            skills.Add(_randomSkillObjectCollection.RandomSkillObjects[i].WeightedObject.Name);
            Debug.Log(_randomSkillObjectCollection.RandomSkillObjects[i].WeightedObject.Name);
        }

        Assert.NotNull(Bash);

        Assert.AreEqual(3, _randomSkillObjectCollection.RandomSkillObjects.Count);

        Assert.True(skills.Contains(Bash.Name));
        Assert.True(skills.Contains(Cleave.Name));
        Assert.True(skills.Contains(Shoot.Name));

        Assert.AreEqual(Bash, _randomSkillObjectCollection.GetRandomObject(new List<ISkill>() { Cleave, Shoot }).WeightedObject);
    }

    [Test]
    public void AssertRandomSkillObjectCollectionWhitelistWorks()
    {
        List<string> skills = new List<string>();

        for (int i = 0; i < _randomSkillObjectCollection.RandomSkillObjects.Count; i++)
        {
            skills.Add(_randomSkillObjectCollection.RandomSkillObjects[i].WeightedObject.Name);
            Debug.Log(_randomSkillObjectCollection.RandomSkillObjects[i].WeightedObject.Name);
        }

        Assert.NotNull(Bash);

        Assert.AreEqual(3, _randomSkillObjectCollection.RandomSkillObjects.Count);

        Assert.True(skills.Contains(Bash.Name));
        Assert.True(skills.Contains(Cleave.Name));
        Assert.True(skills.Contains(Shoot.Name));

        Assert.AreEqual(Shoot, _randomSkillObjectCollection.GetRandomObject(null, new List<ISkill>() { Shoot }).WeightedObject);
    }

    [Test]
    public void AssertRandomSkillObjectCollectionWhitelistAndBlacklistWorkTogether()
    {
        List<string> skills = new List<string>();

        for (int i = 0; i < _randomSkillObjectCollection.RandomSkillObjects.Count; i++)
        {
            skills.Add(_randomSkillObjectCollection.RandomSkillObjects[i].WeightedObject.Name);
            Debug.Log(_randomSkillObjectCollection.RandomSkillObjects[i].WeightedObject.Name);
        }

        Assert.NotNull(Bash);

        Assert.AreEqual(3, _randomSkillObjectCollection.RandomSkillObjects.Count);

        Assert.True(skills.Contains(Bash.Name));
        Assert.True(skills.Contains(Cleave.Name));
        Assert.True(skills.Contains(Shoot.Name));

        Assert.AreEqual(Cleave, _randomSkillObjectCollection.GetRandomObject(new List<ISkill>() { Shoot }, new List<ISkill>() { Cleave, Shoot }).WeightedObject);
    }

    [Test]
    public void AssertRandomSkillObjectCollectionThrowsExceptions()
    {
        List<string> skills = new List<string>();

        for (int i = 0; i < _randomSkillObjectCollection.RandomSkillObjects.Count; i++)
        {
            skills.Add(_randomSkillObjectCollection.RandomSkillObjects[i].WeightedObject.Name);
            Debug.Log(_randomSkillObjectCollection.RandomSkillObjects[i].WeightedObject.Name);
        }

        Assert.NotNull(Bash);

        Assert.AreEqual(3, _randomSkillObjectCollection.RandomSkillObjects.Count);

        Assert.True(skills.Contains(Bash.Name));
        Assert.True(skills.Contains(Cleave.Name));
        Assert.True(skills.Contains(Shoot.Name));

        Assert.Throws<Exception>(() => _randomSkillObjectCollection.GetRandomObject(null, new List<ISkill>() { })); // throws exception when whitelist is empty
    }
    #endregion

    #region Protected Functions
    #endregion

    #region Private Functions

    #endregion

    #region Inner Classes
    #endregion
}
