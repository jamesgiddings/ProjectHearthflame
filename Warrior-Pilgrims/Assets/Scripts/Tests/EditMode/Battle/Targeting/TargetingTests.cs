using GramophoneUtils.Characters;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using Character = GramophoneUtils.Characters.Character;

public class TargetingTests : BasicEditModeTest
{

    private BattleDataModel _battleDataModel;

    private UseFromSlot _OOOO;
    private UseFromSlot _XXXO;

    private TargetToSlots _targetToSlots_XXXX_OXXX;
    private TargetToSlots _targetToSlots_XXXX_OOXXandXXXX_XXOO;
    private TargetToSlots _targetToSlots_XXXX_OOXXandXXXX_XXOOandXXXX_XXOXandXXXX_XXXO;

    private CharacterOrder _playerCharacterOrder;
    private CharacterOrder _enemyCharacterOrder;

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
    }


    [Test]
    public void AssertTargetsObjectNotNull()
    {
        Assert.NotNull(Character1);
        Assert.NotNull(_playerCharacterOrder);
        Assert.NotNull(_enemyCharacterOrder);
        ITargets targetsObject = _targetToSlots_XXXX_OXXX.GetTargetsObject(Character1, _playerCharacterOrder, _enemyCharacterOrder);
        Assert.NotNull(targetsObject);
    }

    [Test]
    public void AssertTargetsObjetesGetCurrentlyTargetedReturnsListOfCount1WhenOnly1TargetPossible()
    {
        ITargets targetsObject = _targetToSlots_XXXX_OXXX.GetTargetsObject(Character1, _playerCharacterOrder, _enemyCharacterOrder);
        Assert.AreEqual(1 ,targetsObject.GetCurrentlyTargeted().Count);
    }

    [Test]
    public void AssertTargetsObjetesGetCurrentlyTargetedReturnsCharacterInEnemySlot1WhenOnlyEnemySlot1Possible()
    {
        ITargets targetsObject = _targetToSlots_XXXX_OXXX.GetTargetsObject(Character1, _playerCharacterOrder, _enemyCharacterOrder);
        Assert.NotNull(_enemyCharacterOrder.GetCharacterBySlotIndex(0));
        Assert.AreEqual("Avienne", Character1.Name);
        Assert.AreEqual("Brown Spider", targetsObject.GetCurrentlyTargeted()[0].Name);
    }

    [Test]
    public void AssertTargetsObjetesGetCurrentlyTargetedReturnsListOfCount2WhenOnly2TargetsInSkill()
    {
        ITargets targetsObject = _targetToSlots_XXXX_OOXXandXXXX_XXOO.GetTargetsObject(Character1, _playerCharacterOrder, _enemyCharacterOrder);
        Assert.AreEqual(2, targetsObject.GetCurrentlyTargeted().Count);
    }

    [Test]
    public void AssertTargetsObjetesGetCurrentlyTargetedReturnsListOfCount2WhenOnly2TargetsInSkillAndOnlyTwoCombinationsAvailableAndChangeDirectionCalledOnceWith10()
    {
        _playerCharacterOrder = new CharacterOrder(new Character[] { Character1, Character2, Character3, Character4 });
        _enemyCharacterOrder = new CharacterOrder(new Character[] { EnemyCharacter1Blueprint.Instance(), EnemyCharacter1Blueprint.Instance(), EnemyCharacter1Blueprint.Instance(), EnemyCharacter1Blueprint.Instance() });

        ITargets targetsObject = _targetToSlots_XXXX_OOXXandXXXX_XXOO.GetTargetsObject(Character1, _playerCharacterOrder, _enemyCharacterOrder);
        targetsObject.ChangeCurrentlyTargeted(new Vector2(1, 0));
        Assert.AreEqual(2, targetsObject.GetCurrentlyTargeted().Count);
    }

    [Test]
    public void AssertTargetsAreDifferentWhenChangeCurrentlyTargetedCalledWhenXXOOandOOXX()
    {
        _playerCharacterOrder = new CharacterOrder(new Character[] { Character1, Character2, Character3, Character4 });
        _enemyCharacterOrder = new CharacterOrder(new Character[] { EnemyCharacter1Blueprint.Instance(), EnemyCharacter1Blueprint.Instance(), EnemyCharacter1Blueprint.Instance(), EnemyCharacter1Blueprint.Instance() });

        ITargets targetsObject = _targetToSlots_XXXX_OOXXandXXXX_XXOO.GetTargetsObject(Character1, _playerCharacterOrder, _enemyCharacterOrder);
        List<Character> list1 = targetsObject.GetCurrentlyTargeted();
        List<Character> list2 = targetsObject.ChangeCurrentlyTargeted(new Vector2(1, 0));

        Assert.AreNotEqual(list1[0], list2[0]);
        Assert.AreNotEqual(list1[1], list2[1]);
        Assert.AreNotEqual(list1[0], list2[1]);
        Assert.AreNotEqual(list1[1], list2[0]);
    }

    [Test]
    public void AssertTargetsAreTheSameWhenChangeCurrentlyTargetedCalledTwiceWhenXXOOandOOXXSoShouldHaveCycledRound()
    {
        _playerCharacterOrder = new CharacterOrder(new Character[] { Character1, Character2, Character3, Character4 });
        _enemyCharacterOrder = new CharacterOrder(new Character[] { EnemyCharacter1Blueprint.Instance(), EnemyCharacter1Blueprint.Instance(), EnemyCharacter1Blueprint.Instance(), EnemyCharacter1Blueprint.Instance() });

        ITargets targetsObject = _targetToSlots_XXXX_OOXXandXXXX_XXOO.GetTargetsObject(Character1, _playerCharacterOrder, _enemyCharacterOrder);
        List<Character> list1 = targetsObject.GetCurrentlyTargeted();
        List<Character> list2 = targetsObject.ChangeCurrentlyTargeted(new Vector2(1, 0));
        List<Character> list3 = targetsObject.ChangeCurrentlyTargeted(new Vector2(1, 0));
        Assert.AreEqual(list1[0], list3[0]);
        Assert.AreEqual(list1[1], list3[1]);
    }

}
