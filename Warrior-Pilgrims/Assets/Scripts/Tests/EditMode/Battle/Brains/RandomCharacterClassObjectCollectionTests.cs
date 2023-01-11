using GramophoneUtils.Characters;
using NSubstitute.ExceptionExtensions;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class RandomCharacterClassObjectCollectionTests : BasicEditModeTest
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
    private RandomCharacterClassObjectCollection _randomCharacterClassObjectCollection;

    private ISkill _shoot;
    private ISkill _bash;
    private ISkill _cleave;

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

        _playerCharacterOrder = new CharacterOrder(new Character[] { Character1, Character2, Character3, Character4 });
        _enemyCharacterOrder = new CharacterOrder(new Character[] { EnemyCharacter1, Size2EnemyCharacter2, EnemyCharacter3 });

        _randomSkillObjectCollection = TestObjectReferences.RandomSkillObjectCollection;
        _randomCharacterClassObjectCollection = TestObjectReferences.RandomCharacterClassObjectCollection;

        _shoot = TestObjectReferences.Shoot;
        _cleave = TestObjectReferences.Cleave;
        _bash = TestObjectReferences.Bash;
    }

    [Test]
    public void AssertCharacterClassObjectCollectionNotNull()
    {
        Assert.NotNull(_randomCharacterClassObjectCollection);
    }

    [Test]
    public void AssertCharacterClassObjectCollectionObjectsNotNull()
    {
        Assert.NotNull(_randomCharacterClassObjectCollection.RandomCharacterClassObjects);
    }

    [Test]
    public void Assert_randomCharacterClassObjectCollectionGetRandomCharacterClassObjectNotNull()
    {
        Assert.NotNull(_randomCharacterClassObjectCollection.GetRandomObject());
    }

    [Test]
    public void AssertRandomCharacterClassObjectCollectionBlacklistWorks()
    {
        List<string> characterClasses = new List<string>();

        for (int i = 0; i < _randomCharacterClassObjectCollection.RandomCharacterClassObjects.Count; i++)
        {
            characterClasses.Add(_randomCharacterClassObjectCollection.RandomCharacterClassObjects[i].WeightedObject.Name);
            Debug.Log(_randomCharacterClassObjectCollection.RandomCharacterClassObjects[i].WeightedObject.Name);
        }

        Assert.NotNull(Musketeer);

        Assert.AreEqual(4, _randomCharacterClassObjectCollection.RandomCharacterClassObjects.Count);

        Assert.True(characterClasses.Contains(TheRuneKnight.Name));
        Assert.True(characterClasses.Contains(HearthPriest.Name));
        Assert.True(characterClasses.Contains(Duelist.Name));
        Assert.True(characterClasses.Contains(Musketeer.Name));

        Assert.AreEqual(Musketeer, _randomCharacterClassObjectCollection.GetRandomObject(new List<CharacterClass>() { TheRuneKnight, HearthPriest, Duelist  }).WeightedObject);
    }

    [Test]
    public void AssertRandomCharacterClassObjectCollectionWhitelistWorks()
    {
        List<string> characterClasses = new List<string>();

        for (int i = 0; i < _randomCharacterClassObjectCollection.RandomCharacterClassObjects.Count; i++)
        {
            characterClasses.Add(_randomCharacterClassObjectCollection.RandomCharacterClassObjects[i].WeightedObject.Name);
            Debug.Log(_randomCharacterClassObjectCollection.RandomCharacterClassObjects[i].WeightedObject.Name);
        }

        Assert.NotNull(Musketeer);
        Assert.NotNull(TheRuneKnight);
        Assert.NotNull(Duelist);
        Assert.NotNull(HearthPriest);

        Assert.AreEqual(4, _randomCharacterClassObjectCollection.RandomCharacterClassObjects.Count);

        Assert.True(characterClasses.Contains(TheRuneKnight.Name));
        Assert.True(characterClasses.Contains(HearthPriest.Name));
        Assert.True(characterClasses.Contains(Duelist.Name));
        Assert.True(characterClasses.Contains(Musketeer.Name));

        Assert.AreEqual(TheRuneKnight, _randomCharacterClassObjectCollection.GetRandomObject(null, new List<CharacterClass>() { TheRuneKnight }).WeightedObject);
    }

    [Test]
    public void AssertRandomCharacterClassObjectCollectionWhitelistAndBlacklistWorkTogether()
    {
        List<string> characterClasses = new List<string>();

        for (int i = 0; i < _randomCharacterClassObjectCollection.RandomCharacterClassObjects.Count; i++)
        {
            characterClasses.Add(_randomCharacterClassObjectCollection.RandomCharacterClassObjects[i].WeightedObject.Name);
            Debug.Log(_randomCharacterClassObjectCollection.RandomCharacterClassObjects[i].WeightedObject.Name);
        }

        Assert.NotNull(Musketeer);
        Assert.NotNull(TheRuneKnight);
        Assert.NotNull(Duelist);
        Assert.NotNull(HearthPriest);

        Assert.AreEqual(4, _randomCharacterClassObjectCollection.RandomCharacterClassObjects.Count);

        Assert.True(characterClasses.Contains(TheRuneKnight.Name));
        Assert.True(characterClasses.Contains(HearthPriest.Name));
        Assert.True(characterClasses.Contains(Duelist.Name));
        Assert.True(characterClasses.Contains(Musketeer.Name));

        Assert.AreEqual(HearthPriest, _randomCharacterClassObjectCollection.GetRandomObject(new List<CharacterClass>() { Duelist }, new List<CharacterClass>() { HearthPriest, Duelist }).WeightedObject);
    }

    [Test]
    public void AssertRandomCharacterClassObjectCollectionThrowsExceptions()
    {

        List<string> characterClasses = new List<string>();

        for (int i = 0; i < _randomCharacterClassObjectCollection.RandomCharacterClassObjects.Count; i++)
        {
            characterClasses.Add(_randomCharacterClassObjectCollection.RandomCharacterClassObjects[i].WeightedObject.Name);
            Debug.Log(_randomCharacterClassObjectCollection.RandomCharacterClassObjects[i].WeightedObject.Name);
        }

        Assert.NotNull(Musketeer);
        Assert.NotNull(TheRuneKnight);
        Assert.NotNull(Duelist);
        Assert.NotNull(HearthPriest);

        Assert.AreEqual(4, _randomCharacterClassObjectCollection.RandomCharacterClassObjects.Count);

        Assert.True(characterClasses.Contains(TheRuneKnight.Name));
        Assert.True(characterClasses.Contains(HearthPriest.Name));
        Assert.True(characterClasses.Contains(Duelist.Name));
        Assert.True(characterClasses.Contains(Musketeer.Name));

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
