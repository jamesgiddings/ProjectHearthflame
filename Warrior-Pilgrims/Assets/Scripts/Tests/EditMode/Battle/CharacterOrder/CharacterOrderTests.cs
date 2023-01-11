using GramophoneUtils.Characters;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GramophoneUtils.Tests
{
    public class CharacterOrderTests : BasicEditModeTest
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
            Size2EnemyCharacter4 = Size2EnemyCharacter2Blueprint.Instance();

            _OOOO = TestObjectReferences.OOOO;
            _XXXO = TestObjectReferences.XXXO;

            _targetToSlots_XXXX_OXXX = TestObjectReferences.TargetToSlots_XXXX_OXXX;
            _targetToSlots_XXXX_OOXXandXXXX_XXOO = TestObjectReferences.TargetToSlots_XXXX_OOXXandXXXX_XXOO;
            _targetToSlots_XXXX_OOXXandXXXX_XXOOandXXXX_XXOXandXXXX_XXXO = TestObjectReferences.TargetToSlots_XXXX_OOXXandXXXX_XXOOandXXXX_XXOXandXXXX_XXXO;

            _playerCharacterOrder = new CharacterOrder(new Character[] { Character1, Character2, Character3, Character4 });
            _enemyCharacterOrder = new CharacterOrder(new Character[] { EnemyCharacter1, Size2EnemyCharacter2, EnemyCharacter3 });
        }

        #endregion

        #region Constructors
        #endregion

        #region Callbacks
        #endregion

        #region Public Functions

        [Test]
        public void AssertCharacterOrderContainsTheCorrectNumberOfSize1CharactersWhenVariousNumbersAreAddedInTheConstructorByArray()
        {
            _playerCharacterOrder = new CharacterOrder(new Character[] { });
            Assert.AreEqual(0, _playerCharacterOrder.GetCharacters().Count);

            _playerCharacterOrder = new CharacterOrder(new Character[] { Character1 });
            Assert.AreEqual(1, _playerCharacterOrder.GetCharacters().Count);

            _playerCharacterOrder = new CharacterOrder(new Character[] { Character1, Character2 });
            Assert.AreEqual(2, _playerCharacterOrder.GetCharacters().Count);

            _playerCharacterOrder = new CharacterOrder(new Character[] { Character1, Character2, Character3 });
            Assert.AreEqual(3, _playerCharacterOrder.GetCharacters().Count);

            _playerCharacterOrder = new CharacterOrder(new Character[] { Character1, Character2, Character3, Character4 });
            Assert.AreEqual(4, _playerCharacterOrder.GetCharacters().Count);
        }

        [Test]
        public void AssertCharacterOrderContainsTheCorrectNumberOfSize2CharactersWhenVariousNumbersAreAddedInTheConstructorByArray()
        {
            Assert.AreEqual(2, Size2EnemyCharacter2.CharacterClass.Size);

            _playerCharacterOrder = new CharacterOrder(new Character[] { Character1, Size2EnemyCharacter2Blueprint.Instance(), Character2 });
            Assert.AreEqual(3, _playerCharacterOrder.GetCharacters().Count);

            _playerCharacterOrder = new CharacterOrder(new Character[] { Character1, Size2EnemyCharacter2Blueprint.Instance(), Character2, Character3, Character4 });
            Assert.AreEqual(3, _playerCharacterOrder.GetCharacters().Count);
        }

        [Test]
        public void AssertCharacterOrderContainsTheCorrectNumberOfCharactersWhenVariousSizedCharactersAreAddedInTheConstructorByArray()
        {
            Assert.AreEqual(2, Size2EnemyCharacter2.CharacterClass.Size);

            _playerCharacterOrder = new CharacterOrder(new Character[] { Size2EnemyCharacter2Blueprint.Instance() });
            Assert.AreEqual(1, _playerCharacterOrder.GetCharacters().Count);

            _playerCharacterOrder = new CharacterOrder(new Character[] { Size2EnemyCharacter2Blueprint.Instance(), Size2EnemyCharacter2Blueprint.Instance() });
            Assert.AreEqual(2, _playerCharacterOrder.GetCharacters().Count);

            _playerCharacterOrder = new CharacterOrder(new Character[] { Size2EnemyCharacter2Blueprint.Instance(), Size2EnemyCharacter2Blueprint.Instance(), Size2EnemyCharacter2Blueprint.Instance() });
            Assert.AreEqual(2, _playerCharacterOrder.GetCharacters().Count);

            _playerCharacterOrder = new CharacterOrder(new Character[] { });
            Assert.AreEqual(0, _playerCharacterOrder.GetCharacters().Count);
        }

        [Test]
        public void AssertCharacterOrderContainsTheCorrectCharactersInTheCorrectSlotsWhenCharactersAreAddedIndividually()
        {
            _playerCharacterOrder = new CharacterOrder(Character1, Character2, Character3, Character4);
            Assert.AreEqual(4, _playerCharacterOrder.GetCharacters().Count);

            Assert.AreEqual(Character1, _playerCharacterOrder.GetCharacterBySlotIndex(0));
            Assert.AreEqual(Character2, _playerCharacterOrder.GetCharacterBySlotIndex(1));
            Assert.AreEqual(Character3, _playerCharacterOrder.GetCharacterBySlotIndex(2));
            Assert.AreEqual(Character4, _playerCharacterOrder.GetCharacterBySlotIndex(3));

            _playerCharacterOrder = new CharacterOrder(Character1, Character2, Character3);
            Assert.AreEqual(3, _playerCharacterOrder.GetCharacters().Count);

            _playerCharacterOrder = new CharacterOrder(Character1, Character2);
            Assert.AreEqual(2, _playerCharacterOrder.GetCharacters().Count);

            _playerCharacterOrder = new CharacterOrder(Character1);
            Assert.AreEqual(1, _playerCharacterOrder.GetCharacters().Count);

            _playerCharacterOrder = new CharacterOrder(null, Character1);
            Assert.AreEqual(1, _playerCharacterOrder.GetCharacters().Count);

            _playerCharacterOrder = new CharacterOrder(null, Character1);
            Assert.AreEqual(Character1, _playerCharacterOrder.GetCharacterBySlotIndex(1));
            Assert.AreEqual(null, _playerCharacterOrder.GetCharacterBySlotIndex(0));

            _playerCharacterOrder = new CharacterOrder(null, Character1, null, Character2);
            Assert.AreEqual(2, _playerCharacterOrder.GetCharacters().Count);
        }

        [Test]
        public void AssertMoveCharacterToSlotSwapsCharactersCorrectly()
        {
            _playerCharacterOrder = new CharacterOrder(Character1, Character2, Character3, Character4);
            Assert.AreEqual(4, _playerCharacterOrder.GetCharacters().Count);

            Assert.AreEqual(Character1, _playerCharacterOrder.GetCharacterBySlotIndex(0));
            Assert.AreEqual(Character2, _playerCharacterOrder.GetCharacterBySlotIndex(1));
            Assert.AreEqual(Character3, _playerCharacterOrder.GetCharacterBySlotIndex(2));
            Assert.AreEqual(Character4, _playerCharacterOrder.GetCharacterBySlotIndex(3));

            _playerCharacterOrder.SwapCharacterIntoSlot(Character1, 3);
            Assert.AreEqual(4, _playerCharacterOrder.GetCharacters().Count);

            Assert.AreEqual(Character4, _playerCharacterOrder.GetCharacterBySlotIndex(0));
            Assert.AreEqual(Character2, _playerCharacterOrder.GetCharacterBySlotIndex(1));
            Assert.AreEqual(Character3, _playerCharacterOrder.GetCharacterBySlotIndex(2));
            Assert.AreEqual(Character1, _playerCharacterOrder.GetCharacterBySlotIndex(3));

            _playerCharacterOrder = new CharacterOrder(null, Character1);
            Assert.AreEqual(1, _playerCharacterOrder.GetCharacters().Count);

            _playerCharacterOrder.SwapCharacterIntoSlot(Character1, 3);
            Assert.AreEqual(1, _playerCharacterOrder.GetCharacters().Count);
            Assert.AreEqual(Character1, _playerCharacterOrder.GetCharacterBySlotIndex(3));

            // move a size 2 character from position 1 to 0

            _playerCharacterOrder = new CharacterOrder(null, Size2EnemyCharacter2, Size2EnemyCharacter2, null);
            Assert.AreEqual(1, _playerCharacterOrder.GetCharacters().Count);
            Assert.AreEqual(null, _playerCharacterOrder.GetCharacterBySlotIndex(0));
            Assert.AreEqual(Size2EnemyCharacter2, _playerCharacterOrder.GetCharacterBySlotIndex(1));
            Assert.AreEqual(Size2EnemyCharacter2, _playerCharacterOrder.GetCharacterBySlotIndex(2));
            Assert.AreEqual(null, _playerCharacterOrder.GetCharacterBySlotIndex(3));

            _playerCharacterOrder.SwapCharacterIntoSlot(Size2EnemyCharacter2, 0);

            Assert.AreEqual(1, _playerCharacterOrder.GetCharacters().Count);
            Assert.AreEqual(Size2EnemyCharacter2, _playerCharacterOrder.GetCharacterBySlotIndex(0));
            Assert.AreEqual(Size2EnemyCharacter2, _playerCharacterOrder.GetCharacterBySlotIndex(1));
            Assert.AreEqual(null, _playerCharacterOrder.GetCharacterBySlotIndex(2));
            Assert.AreEqual(null, _playerCharacterOrder.GetCharacterBySlotIndex(3));

            // move a size 2 character from position 2 to 0

            _playerCharacterOrder = new CharacterOrder(null, null, Size2EnemyCharacter2, Size2EnemyCharacter2);
            Assert.AreEqual(1, _playerCharacterOrder.GetCharacters().Count);
            Assert.AreEqual(null, _playerCharacterOrder.GetCharacterBySlotIndex(0));
            Assert.AreEqual(null, _playerCharacterOrder.GetCharacterBySlotIndex(1));
            Assert.AreEqual(Size2EnemyCharacter2, _playerCharacterOrder.GetCharacterBySlotIndex(2));
            Assert.AreEqual(Size2EnemyCharacter2, _playerCharacterOrder.GetCharacterBySlotIndex(3));

            _playerCharacterOrder.SwapCharacterIntoSlot(Size2EnemyCharacter2, 0);

            Assert.AreEqual(1, _playerCharacterOrder.GetCharacters().Count);
            Assert.AreEqual(Size2EnemyCharacter2, _playerCharacterOrder.GetCharacterBySlotIndex(0));
            Assert.AreEqual(Size2EnemyCharacter2, _playerCharacterOrder.GetCharacterBySlotIndex(1));
            Assert.AreEqual(null, _playerCharacterOrder.GetCharacterBySlotIndex(2));
            Assert.AreEqual(null, _playerCharacterOrder.GetCharacterBySlotIndex(3));

            _playerCharacterOrder = new CharacterOrder(Size2EnemyCharacter2, Size2EnemyCharacter2, null, null);
            Assert.AreEqual(1, _playerCharacterOrder.GetCharacters().Count);
            Assert.AreEqual(Size2EnemyCharacter2, _playerCharacterOrder.GetCharacterBySlotIndex(0));
            Assert.AreEqual(Size2EnemyCharacter2, _playerCharacterOrder.GetCharacterBySlotIndex(1));
            Assert.AreEqual(null, _playerCharacterOrder.GetCharacterBySlotIndex(2));
            Assert.AreEqual(null, _playerCharacterOrder.GetCharacterBySlotIndex(3));

            _playerCharacterOrder.SwapCharacterIntoSlot(Size2EnemyCharacter2, 1);

            Assert.AreEqual(1, _playerCharacterOrder.GetCharacters().Count);
            Assert.AreEqual(null, _playerCharacterOrder.GetCharacterBySlotIndex(0));
            Assert.AreEqual(Size2EnemyCharacter2, _playerCharacterOrder.GetCharacterBySlotIndex(1));
            Assert.AreEqual(Size2EnemyCharacter2, _playerCharacterOrder.GetCharacterBySlotIndex(2));
            Assert.AreEqual(null, _playerCharacterOrder.GetCharacterBySlotIndex(3));

            _playerCharacterOrder.SwapCharacterIntoSlot(Size2EnemyCharacter2, 2);

            Assert.AreEqual(1, _playerCharacterOrder.GetCharacters().Count);
            Assert.AreEqual(null, _playerCharacterOrder.GetCharacterBySlotIndex(0));
            Assert.AreEqual(null, _playerCharacterOrder.GetCharacterBySlotIndex(1));
            Assert.AreEqual(Size2EnemyCharacter2, _playerCharacterOrder.GetCharacterBySlotIndex(2));
            Assert.AreEqual(Size2EnemyCharacter2, _playerCharacterOrder.GetCharacterBySlotIndex(3));

            // Test that I can move a size 2 character by swap when I get the size two character from the second position it occupies

            _playerCharacterOrder = new CharacterOrder(null, Size2EnemyCharacter2, Size2EnemyCharacter2, null);

            Assert.AreEqual(1, _playerCharacterOrder.GetCharacters().Count);
            Assert.AreEqual(null, _playerCharacterOrder.GetCharacterBySlotIndex(0));
            Assert.AreEqual(Size2EnemyCharacter2, _playerCharacterOrder.GetCharacterBySlotIndex(1));
            Assert.AreEqual(Size2EnemyCharacter2, _playerCharacterOrder.GetCharacterBySlotIndex(2));
            Assert.AreEqual(null, _playerCharacterOrder.GetCharacterBySlotIndex(3));

            _playerCharacterOrder.SwapCharacterIntoSlot(_playerCharacterOrder.GetCharacterBySlotIndex(2), 2);

            Assert.AreEqual(1, _playerCharacterOrder.GetCharacters().Count);
            Assert.AreEqual(null, _playerCharacterOrder.GetCharacterBySlotIndex(0));
            Assert.AreEqual(null, _playerCharacterOrder.GetCharacterBySlotIndex(1));
            Assert.AreEqual(Size2EnemyCharacter2, _playerCharacterOrder.GetCharacterBySlotIndex(2));
            Assert.AreEqual(Size2EnemyCharacter2, _playerCharacterOrder.GetCharacterBySlotIndex(3));

        }

        [Test]
        public void AssertMoveForwardIntoSlotsWorksWithSize1CharactersOnly()
        {
            _playerCharacterOrder = new CharacterOrder(null, Character2);
            Assert.AreEqual(1, _playerCharacterOrder.GetCharacters().Count);
            Assert.AreEqual(Character2, _playerCharacterOrder.GetCharacterBySlotIndex(1));
            Assert.AreEqual(null, _playerCharacterOrder.GetCharacterBySlotIndex(0));

            _playerCharacterOrder.MoveCharactersForwardIntoSpaces();
            Assert.AreEqual(Character2, _playerCharacterOrder.GetCharacterBySlotIndex(0));
            Assert.AreEqual(null, _playerCharacterOrder.GetCharacterBySlotIndex(1));

            _playerCharacterOrder = new CharacterOrder(null, null, null, Character2);
            Assert.AreEqual(1, _playerCharacterOrder.GetCharacters().Count);
            Assert.AreEqual(null, _playerCharacterOrder.GetCharacterBySlotIndex(0));
            Assert.AreEqual(null, _playerCharacterOrder.GetCharacterBySlotIndex(1));
            Assert.AreEqual(null, _playerCharacterOrder.GetCharacterBySlotIndex(2));
            Assert.AreEqual(Character2, _playerCharacterOrder.GetCharacterBySlotIndex(3));

            _playerCharacterOrder.MoveCharactersForwardIntoSpaces();
            Assert.AreEqual(Character2, _playerCharacterOrder.GetCharacterBySlotIndex(0));
            Assert.AreEqual(null, _playerCharacterOrder.GetCharacterBySlotIndex(1));
            Assert.AreEqual(null, _playerCharacterOrder.GetCharacterBySlotIndex(2));
            Assert.AreEqual(null, _playerCharacterOrder.GetCharacterBySlotIndex(3));

            _playerCharacterOrder = new CharacterOrder(null, Character1, null, Character2);
            Assert.AreEqual(2, _playerCharacterOrder.GetCharacters().Count);
            Assert.AreEqual(null, _playerCharacterOrder.GetCharacterBySlotIndex(0));
            Assert.AreEqual(Character1, _playerCharacterOrder.GetCharacterBySlotIndex(1));
            Assert.AreEqual(null, _playerCharacterOrder.GetCharacterBySlotIndex(2));
            Assert.AreEqual(Character2, _playerCharacterOrder.GetCharacterBySlotIndex(3));

            _playerCharacterOrder.MoveCharactersForwardIntoSpaces();
            Assert.AreEqual(2, _playerCharacterOrder.GetCharacters().Count);
            Assert.AreEqual(Character1, _playerCharacterOrder.GetCharacterBySlotIndex(0));
            Assert.AreEqual(Character2, _playerCharacterOrder.GetCharacterBySlotIndex(1));
            Assert.AreEqual(null, _playerCharacterOrder.GetCharacterBySlotIndex(2));
            Assert.AreEqual(null, _playerCharacterOrder.GetCharacterBySlotIndex(3));
        }

        [Test]
        public void AssertMoveBackwardIntoSlotsWorksWithSize1CharactersOnly()
        {
            _playerCharacterOrder = new CharacterOrder(null, null, Character2, null);
            Assert.AreEqual(1, _playerCharacterOrder.GetCharacters().Count);
            Assert.AreEqual(null, _playerCharacterOrder.GetCharacterBySlotIndex(0));
            Assert.AreEqual(null, _playerCharacterOrder.GetCharacterBySlotIndex(1));
            Assert.AreEqual(Character2, _playerCharacterOrder.GetCharacterBySlotIndex(2));
            Assert.AreEqual(null, _playerCharacterOrder.GetCharacterBySlotIndex(3));

            _playerCharacterOrder.MoveCharactersBackwardIntoSpaces();
            Assert.AreEqual(Character2, _playerCharacterOrder.GetCharacterBySlotIndex(3));
            Assert.AreEqual(null, _playerCharacterOrder.GetCharacterBySlotIndex(2));
            Assert.AreEqual(null, _playerCharacterOrder.GetCharacterBySlotIndex(1));
            Assert.AreEqual(null, _playerCharacterOrder.GetCharacterBySlotIndex(0));

            _playerCharacterOrder = new CharacterOrder(null, null, null, Character2);
            Assert.AreEqual(1, _playerCharacterOrder.GetCharacters().Count);
            Assert.AreEqual(null, _playerCharacterOrder.GetCharacterBySlotIndex(0));
            Assert.AreEqual(null, _playerCharacterOrder.GetCharacterBySlotIndex(1));
            Assert.AreEqual(null, _playerCharacterOrder.GetCharacterBySlotIndex(2));
            Assert.AreEqual(Character2, _playerCharacterOrder.GetCharacterBySlotIndex(3));

            _playerCharacterOrder.MoveCharactersBackwardIntoSpaces();
            Assert.AreEqual(null, _playerCharacterOrder.GetCharacterBySlotIndex(0));
            Assert.AreEqual(null, _playerCharacterOrder.GetCharacterBySlotIndex(1));
            Assert.AreEqual(null, _playerCharacterOrder.GetCharacterBySlotIndex(2));
            Assert.AreEqual(Character2, _playerCharacterOrder.GetCharacterBySlotIndex(3));

            _playerCharacterOrder = new CharacterOrder(Character2, null, null, null);
            Assert.AreEqual(1, _playerCharacterOrder.GetCharacters().Count);
            Assert.AreEqual(Character2, _playerCharacterOrder.GetCharacterBySlotIndex(0));
            Assert.AreEqual(null, _playerCharacterOrder.GetCharacterBySlotIndex(1));
            Assert.AreEqual(null, _playerCharacterOrder.GetCharacterBySlotIndex(2));
            Assert.AreEqual(null, _playerCharacterOrder.GetCharacterBySlotIndex(3));

            _playerCharacterOrder.MoveCharactersBackwardIntoSpaces();
            Assert.AreEqual(null, _playerCharacterOrder.GetCharacterBySlotIndex(0));
            Assert.AreEqual(null, _playerCharacterOrder.GetCharacterBySlotIndex(1));
            Assert.AreEqual(null, _playerCharacterOrder.GetCharacterBySlotIndex(2));
            Assert.AreEqual(Character2, _playerCharacterOrder.GetCharacterBySlotIndex(3));

            _playerCharacterOrder = new CharacterOrder(null, Character1, null, Character2);
            Assert.AreEqual(2, _playerCharacterOrder.GetCharacters().Count);
            Assert.AreEqual(null, _playerCharacterOrder.GetCharacterBySlotIndex(0));
            Assert.AreEqual(Character1, _playerCharacterOrder.GetCharacterBySlotIndex(1));
            Assert.AreEqual(null, _playerCharacterOrder.GetCharacterBySlotIndex(2));
            Assert.AreEqual(Character2, _playerCharacterOrder.GetCharacterBySlotIndex(3));

            _playerCharacterOrder.MoveCharactersBackwardIntoSpaces();
            Assert.AreEqual(2, _playerCharacterOrder.GetCharacters().Count);
            Assert.AreEqual(null, _playerCharacterOrder.GetCharacterBySlotIndex(0));
            Assert.AreEqual(null, _playerCharacterOrder.GetCharacterBySlotIndex(1));
            Assert.AreEqual(Character1, _playerCharacterOrder.GetCharacterBySlotIndex(2));
            Assert.AreEqual(Character2, _playerCharacterOrder.GetCharacterBySlotIndex(3));
        }

        [Test]
        public void AssertMoveForwardIntoSlotsWorksWithSize2CharactersOnly()
        {
            _playerCharacterOrder = new CharacterOrder(null, Size2EnemyCharacter2, Size2EnemyCharacter2, null);
            Assert.AreEqual(1, _playerCharacterOrder.GetCharacters().Count);
            Assert.AreEqual(null, _playerCharacterOrder.GetCharacterBySlotIndex(0));
            Assert.AreEqual(Size2EnemyCharacter2, _playerCharacterOrder.GetCharacterBySlotIndex(1));
            Assert.AreEqual(Size2EnemyCharacter2, _playerCharacterOrder.GetCharacterBySlotIndex(2));
            Assert.AreEqual(null, _playerCharacterOrder.GetCharacterBySlotIndex(3));

            _playerCharacterOrder.MoveCharactersForwardIntoSpaces();

            Assert.AreEqual(1, _playerCharacterOrder.GetCharacters().Count);
            Assert.AreEqual(Size2EnemyCharacter2, _playerCharacterOrder.GetCharacterBySlotIndex(0));
            Assert.AreEqual(Size2EnemyCharacter2, _playerCharacterOrder.GetCharacterBySlotIndex(1));
            Assert.AreEqual(null, _playerCharacterOrder.GetCharacterBySlotIndex(2));
            Assert.AreEqual(null, _playerCharacterOrder.GetCharacterBySlotIndex(3));

            _playerCharacterOrder = new CharacterOrder(null, null, Size2EnemyCharacter2, Size2EnemyCharacter2);
            Assert.AreEqual(1, _playerCharacterOrder.GetCharacters().Count);
            Assert.AreEqual(null, _playerCharacterOrder.GetCharacterBySlotIndex(0));
            Assert.AreEqual(null, _playerCharacterOrder.GetCharacterBySlotIndex(1));
            Assert.AreEqual(Size2EnemyCharacter2, _playerCharacterOrder.GetCharacterBySlotIndex(2));
            Assert.AreEqual(Size2EnemyCharacter2, _playerCharacterOrder.GetCharacterBySlotIndex(3));

            _playerCharacterOrder.MoveCharactersForwardIntoSpaces();

            Assert.AreEqual(1, _playerCharacterOrder.GetCharacters().Count);
            Assert.AreEqual(Size2EnemyCharacter2, _playerCharacterOrder.GetCharacterBySlotIndex(0));
            Assert.AreEqual(Size2EnemyCharacter2, _playerCharacterOrder.GetCharacterBySlotIndex(1));
            Assert.AreEqual(null, _playerCharacterOrder.GetCharacterBySlotIndex(2));
            Assert.AreEqual(null, _playerCharacterOrder.GetCharacterBySlotIndex(3));
        }

        [Test]
        public void AssertMoveBackwardIntoSlotsWorksWithSize2CharactersOnly()
        {
            _playerCharacterOrder = new CharacterOrder(null, Size2EnemyCharacter2, Size2EnemyCharacter2, null);
            Assert.AreEqual(1, _playerCharacterOrder.GetCharacters().Count);
            Assert.AreEqual(null, _playerCharacterOrder.GetCharacterBySlotIndex(0));
            Assert.AreEqual(Size2EnemyCharacter2, _playerCharacterOrder.GetCharacterBySlotIndex(1));
            Assert.AreEqual(Size2EnemyCharacter2, _playerCharacterOrder.GetCharacterBySlotIndex(2));
            Assert.AreEqual(null, _playerCharacterOrder.GetCharacterBySlotIndex(3));

            _playerCharacterOrder.MoveCharactersBackwardIntoSpaces();

            Assert.AreEqual(1, _playerCharacterOrder.GetCharacters().Count);
            Assert.AreEqual(null, _playerCharacterOrder.GetCharacterBySlotIndex(0));
            Assert.AreEqual(null, _playerCharacterOrder.GetCharacterBySlotIndex(1));
            Assert.AreEqual(Size2EnemyCharacter2, _playerCharacterOrder.GetCharacterBySlotIndex(2));
            Assert.AreEqual(Size2EnemyCharacter2, _playerCharacterOrder.GetCharacterBySlotIndex(3));

            _playerCharacterOrder = new CharacterOrder(null, null, Size2EnemyCharacter2, Size2EnemyCharacter2);
            Assert.AreEqual(1, _playerCharacterOrder.GetCharacters().Count);
            Assert.AreEqual(null, _playerCharacterOrder.GetCharacterBySlotIndex(0));
            Assert.AreEqual(null, _playerCharacterOrder.GetCharacterBySlotIndex(1));
            Assert.AreEqual(Size2EnemyCharacter2, _playerCharacterOrder.GetCharacterBySlotIndex(2));
            Assert.AreEqual(Size2EnemyCharacter2, _playerCharacterOrder.GetCharacterBySlotIndex(3));

            _playerCharacterOrder.MoveCharactersBackwardIntoSpaces();

            Assert.AreEqual(1, _playerCharacterOrder.GetCharacters().Count);
            Assert.AreEqual(null, _playerCharacterOrder.GetCharacterBySlotIndex(0));
            Assert.AreEqual(null, _playerCharacterOrder.GetCharacterBySlotIndex(1));
            Assert.AreEqual(Size2EnemyCharacter2, _playerCharacterOrder.GetCharacterBySlotIndex(2));
            Assert.AreEqual(Size2EnemyCharacter2, _playerCharacterOrder.GetCharacterBySlotIndex(3));

            _playerCharacterOrder = new CharacterOrder(Size2EnemyCharacter4, Size2EnemyCharacter4, Size2EnemyCharacter2, Size2EnemyCharacter2);
            Assert.AreEqual(2, _playerCharacterOrder.GetCharacters().Count);
            Assert.AreEqual(Size2EnemyCharacter4, _playerCharacterOrder.GetCharacterBySlotIndex(0));
            Assert.AreEqual(Size2EnemyCharacter4, _playerCharacterOrder.GetCharacterBySlotIndex(1));
            Assert.AreEqual(Size2EnemyCharacter2, _playerCharacterOrder.GetCharacterBySlotIndex(2));
            Assert.AreEqual(Size2EnemyCharacter2, _playerCharacterOrder.GetCharacterBySlotIndex(3));

            _playerCharacterOrder.MoveCharactersBackwardIntoSpaces();

            Assert.AreEqual(2, _playerCharacterOrder.GetCharacters().Count);
            Assert.AreEqual(Size2EnemyCharacter4, _playerCharacterOrder.GetCharacterBySlotIndex(0));
            Assert.AreEqual(Size2EnemyCharacter4, _playerCharacterOrder.GetCharacterBySlotIndex(1));
            Assert.AreEqual(Size2EnemyCharacter2, _playerCharacterOrder.GetCharacterBySlotIndex(2));
            Assert.AreEqual(Size2EnemyCharacter2, _playerCharacterOrder.GetCharacterBySlotIndex(3));
        }

        [Test]
        public void AssertMoveBackwardIntoSlotsWorksWithMixedSizedCharacters()
        {
            _playerCharacterOrder = new CharacterOrder(null, Size2EnemyCharacter2, Size2EnemyCharacter2, Character1);
            Assert.AreEqual(2, _playerCharacterOrder.GetCharacters().Count);
            Assert.AreEqual(null, _playerCharacterOrder.GetCharacterBySlotIndex(0));
            Assert.AreEqual(Size2EnemyCharacter2, _playerCharacterOrder.GetCharacterBySlotIndex(1));
            Assert.AreEqual(Size2EnemyCharacter2, _playerCharacterOrder.GetCharacterBySlotIndex(2));
            Assert.AreEqual(Character1, _playerCharacterOrder.GetCharacterBySlotIndex(3));

            _playerCharacterOrder.MoveCharactersBackwardIntoSpaces();

            Assert.AreEqual(2, _playerCharacterOrder.GetCharacters().Count);
            Assert.AreEqual(null, _playerCharacterOrder.GetCharacterBySlotIndex(0));
            Assert.AreEqual(Size2EnemyCharacter2, _playerCharacterOrder.GetCharacterBySlotIndex(1));
            Assert.AreEqual(Size2EnemyCharacter2, _playerCharacterOrder.GetCharacterBySlotIndex(2));
            Assert.AreEqual(Character1, _playerCharacterOrder.GetCharacterBySlotIndex(3));

            _playerCharacterOrder = new CharacterOrder(null, Size2EnemyCharacter2, Size2EnemyCharacter2, Character1);
            Assert.AreEqual(2, _playerCharacterOrder.GetCharacters().Count);
            Assert.AreEqual(null, _playerCharacterOrder.GetCharacterBySlotIndex(0));
            Assert.AreEqual(Size2EnemyCharacter2, _playerCharacterOrder.GetCharacterBySlotIndex(1));
            Assert.AreEqual(Size2EnemyCharacter2, _playerCharacterOrder.GetCharacterBySlotIndex(2));
            Assert.AreEqual(Character1, _playerCharacterOrder.GetCharacterBySlotIndex(3));

            _playerCharacterOrder.MoveCharactersBackwardIntoSpaces();

            Assert.AreEqual(2, _playerCharacterOrder.GetCharacters().Count);
            Assert.AreEqual(null, _playerCharacterOrder.GetCharacterBySlotIndex(0));
            Assert.AreEqual(Size2EnemyCharacter2, _playerCharacterOrder.GetCharacterBySlotIndex(1));
            Assert.AreEqual(Size2EnemyCharacter2, _playerCharacterOrder.GetCharacterBySlotIndex(2));
            Assert.AreEqual(Character1, _playerCharacterOrder.GetCharacterBySlotIndex(3));

            _playerCharacterOrder = new CharacterOrder(Size2EnemyCharacter2, Size2EnemyCharacter2, null, Character1);
            Assert.AreEqual(2, _playerCharacterOrder.GetCharacters().Count);
            Assert.AreEqual(Size2EnemyCharacter2, _playerCharacterOrder.GetCharacterBySlotIndex(0));
            Assert.AreEqual(Size2EnemyCharacter2, _playerCharacterOrder.GetCharacterBySlotIndex(1));
            Assert.AreEqual(null, _playerCharacterOrder.GetCharacterBySlotIndex(2));
            Assert.AreEqual(Character1, _playerCharacterOrder.GetCharacterBySlotIndex(3));

            _playerCharacterOrder.MoveCharactersBackwardIntoSpaces();

            Assert.AreEqual(2, _playerCharacterOrder.GetCharacters().Count);
            Assert.AreEqual(null, _playerCharacterOrder.GetCharacterBySlotIndex(0));
            Assert.AreEqual(Size2EnemyCharacter2, _playerCharacterOrder.GetCharacterBySlotIndex(1));
            Assert.AreEqual(Size2EnemyCharacter2, _playerCharacterOrder.GetCharacterBySlotIndex(2));
            Assert.AreEqual(Character1, _playerCharacterOrder.GetCharacterBySlotIndex(3));
        }

        [Test]
        public void AssertMoveBackwardIntoSlotsWorksWithIncludeParameter()
        {
            _playerCharacterOrder = new CharacterOrder(Character1, null, null, null);

            Assert.AreEqual(1, _playerCharacterOrder.GetCharacters().Count);
            Assert.AreEqual(Character1, _playerCharacterOrder.GetCharacterBySlotIndex(0));
            Assert.AreEqual(null, _playerCharacterOrder.GetCharacterBySlotIndex(1));
            Assert.AreEqual(null, _playerCharacterOrder.GetCharacterBySlotIndex(2));
            Assert.AreEqual(null, _playerCharacterOrder.GetCharacterBySlotIndex(3));

            _playerCharacterOrder.MoveCharactersBackwardIntoSpaces(3); // include slot 3 and above

            Assert.AreEqual(1, _playerCharacterOrder.GetCharacters().Count);
            Assert.AreEqual(Character1, _playerCharacterOrder.GetCharacterBySlotIndex(0));
            Assert.AreEqual(null, _playerCharacterOrder.GetCharacterBySlotIndex(1));
            Assert.AreEqual(null, _playerCharacterOrder.GetCharacterBySlotIndex(2));
            Assert.AreEqual(null, _playerCharacterOrder.GetCharacterBySlotIndex(3));

            _playerCharacterOrder = new CharacterOrder(Character1, null, null, null);

            Assert.AreEqual(1, _playerCharacterOrder.GetCharacters().Count);
            Assert.AreEqual(Character1, _playerCharacterOrder.GetCharacterBySlotIndex(0));
            Assert.AreEqual(null, _playerCharacterOrder.GetCharacterBySlotIndex(1));
            Assert.AreEqual(null, _playerCharacterOrder.GetCharacterBySlotIndex(2));
            Assert.AreEqual(null, _playerCharacterOrder.GetCharacterBySlotIndex(3));

            _playerCharacterOrder.MoveCharactersBackwardIntoSpaces(2); // include slot 2 and above

            Assert.AreEqual(1, _playerCharacterOrder.GetCharacters().Count);
            Assert.AreEqual(Character1, _playerCharacterOrder.GetCharacterBySlotIndex(0));
            Assert.AreEqual(null, _playerCharacterOrder.GetCharacterBySlotIndex(1));
            Assert.AreEqual(null, _playerCharacterOrder.GetCharacterBySlotIndex(2));
            Assert.AreEqual(null, _playerCharacterOrder.GetCharacterBySlotIndex(3));

            _playerCharacterOrder = new CharacterOrder(Character1, null, null, null);

            Assert.AreEqual(1, _playerCharacterOrder.GetCharacters().Count);
            Assert.AreEqual(Character1, _playerCharacterOrder.GetCharacterBySlotIndex(0));
            Assert.AreEqual(null, _playerCharacterOrder.GetCharacterBySlotIndex(1));
            Assert.AreEqual(null, _playerCharacterOrder.GetCharacterBySlotIndex(2));
            Assert.AreEqual(null, _playerCharacterOrder.GetCharacterBySlotIndex(3));

            _playerCharacterOrder.MoveCharactersBackwardIntoSpaces(1); // include slot 1 and above

            Assert.AreEqual(1, _playerCharacterOrder.GetCharacters().Count);
            Assert.AreEqual(Character1, _playerCharacterOrder.GetCharacterBySlotIndex(0));
            Assert.AreEqual(null, _playerCharacterOrder.GetCharacterBySlotIndex(1));
            Assert.AreEqual(null, _playerCharacterOrder.GetCharacterBySlotIndex(2));
            Assert.AreEqual(null, _playerCharacterOrder.GetCharacterBySlotIndex(3));

            _playerCharacterOrder = new CharacterOrder(Character1, null, null, null);

            Assert.AreEqual(1, _playerCharacterOrder.GetCharacters().Count);
            Assert.AreEqual(Character1, _playerCharacterOrder.GetCharacterBySlotIndex(0));
            Assert.AreEqual(null, _playerCharacterOrder.GetCharacterBySlotIndex(1));
            Assert.AreEqual(null, _playerCharacterOrder.GetCharacterBySlotIndex(2));
            Assert.AreEqual(null, _playerCharacterOrder.GetCharacterBySlotIndex(3));

            _playerCharacterOrder.MoveCharactersBackwardIntoSpaces(0); // include slot 0 and above

            Assert.AreEqual(1, _playerCharacterOrder.GetCharacters().Count);
            Assert.AreEqual(null, _playerCharacterOrder.GetCharacterBySlotIndex(0));
            Assert.AreEqual(null, _playerCharacterOrder.GetCharacterBySlotIndex(1));
            Assert.AreEqual(null, _playerCharacterOrder.GetCharacterBySlotIndex(2));
            Assert.AreEqual(Character1, _playerCharacterOrder.GetCharacterBySlotIndex(3));

            _playerCharacterOrder = new CharacterOrder(null, Character1, null, null);

            Assert.AreEqual(1, _playerCharacterOrder.GetCharacters().Count);
            Assert.AreEqual(null, _playerCharacterOrder.GetCharacterBySlotIndex(0));
            Assert.AreEqual(Character1, _playerCharacterOrder.GetCharacterBySlotIndex(1));
            Assert.AreEqual(null, _playerCharacterOrder.GetCharacterBySlotIndex(2));
            Assert.AreEqual(null, _playerCharacterOrder.GetCharacterBySlotIndex(3));

            _playerCharacterOrder.MoveCharactersBackwardIntoSpaces(-1); // include all

            Assert.AreEqual(1, _playerCharacterOrder.GetCharacters().Count);
            Assert.AreEqual(null, _playerCharacterOrder.GetCharacterBySlotIndex(0));
            Assert.AreEqual(null, _playerCharacterOrder.GetCharacterBySlotIndex(1));
            Assert.AreEqual(null, _playerCharacterOrder.GetCharacterBySlotIndex(2));
            Assert.AreEqual(Character1, _playerCharacterOrder.GetCharacterBySlotIndex(3));

            _playerCharacterOrder = new CharacterOrder(Character1, null, Character2, null);

            Assert.AreEqual(2, _playerCharacterOrder.GetCharacters().Count);
            Assert.AreEqual(Character1, _playerCharacterOrder.GetCharacterBySlotIndex(0));
            Assert.AreEqual(null, _playerCharacterOrder.GetCharacterBySlotIndex(1));
            Assert.AreEqual(Character2, _playerCharacterOrder.GetCharacterBySlotIndex(2));
            Assert.AreEqual(null, _playerCharacterOrder.GetCharacterBySlotIndex(3));

            _playerCharacterOrder.MoveCharactersBackwardIntoSpaces(-1); // include all

            Assert.AreEqual(2, _playerCharacterOrder.GetCharacters().Count);
            Assert.AreEqual(null, _playerCharacterOrder.GetCharacterBySlotIndex(0));
            Assert.AreEqual(null, _playerCharacterOrder.GetCharacterBySlotIndex(1));
            Assert.AreEqual(Character1, _playerCharacterOrder.GetCharacterBySlotIndex(2));
            Assert.AreEqual(Character2, _playerCharacterOrder.GetCharacterBySlotIndex(3));

            _playerCharacterOrder = new CharacterOrder(Character1, Character2, Character3, null);

            Assert.AreEqual(3, _playerCharacterOrder.GetCharacters().Count);
            Assert.AreEqual(Character1, _playerCharacterOrder.GetCharacterBySlotIndex(0));
            Assert.AreEqual(Character2, _playerCharacterOrder.GetCharacterBySlotIndex(1));
            Assert.AreEqual(Character3, _playerCharacterOrder.GetCharacterBySlotIndex(2));
            Assert.AreEqual(null, _playerCharacterOrder.GetCharacterBySlotIndex(3));

            _playerCharacterOrder.MoveCharactersBackwardIntoSpaces(1); // include 1 and above

            Assert.AreEqual(3, _playerCharacterOrder.GetCharacters().Count);
            Assert.AreEqual(Character1, _playerCharacterOrder.GetCharacterBySlotIndex(0));
            Assert.AreEqual(null, _playerCharacterOrder.GetCharacterBySlotIndex(1));
            Assert.AreEqual(Character2, _playerCharacterOrder.GetCharacterBySlotIndex(2));
            Assert.AreEqual(Character3, _playerCharacterOrder.GetCharacterBySlotIndex(3));

            _playerCharacterOrder = new CharacterOrder(Size2EnemyCharacter2, Size2EnemyCharacter2, Character2, null);

            Assert.AreEqual(2, _playerCharacterOrder.GetCharacters().Count);
            Assert.AreEqual(Size2EnemyCharacter2, _playerCharacterOrder.GetCharacterBySlotIndex(0));
            Assert.AreEqual(Size2EnemyCharacter2, _playerCharacterOrder.GetCharacterBySlotIndex(1));
            Assert.AreEqual(Character2, _playerCharacterOrder.GetCharacterBySlotIndex(2));
            Assert.AreEqual(null, _playerCharacterOrder.GetCharacterBySlotIndex(3));

            _playerCharacterOrder.MoveCharactersBackwardIntoSpaces(2); // include none

            Assert.AreEqual(2, _playerCharacterOrder.GetCharacters().Count);
            Assert.AreEqual(Size2EnemyCharacter2, _playerCharacterOrder.GetCharacterBySlotIndex(0));
            Assert.AreEqual(Size2EnemyCharacter2, _playerCharacterOrder.GetCharacterBySlotIndex(1));
            Assert.AreEqual(null, _playerCharacterOrder.GetCharacterBySlotIndex(2));
            Assert.AreEqual(Character2, _playerCharacterOrder.GetCharacterBySlotIndex(3));

            _playerCharacterOrder = new CharacterOrder(Size2EnemyCharacter2, Size2EnemyCharacter2, Character2, null);

            Assert.AreEqual(2, _playerCharacterOrder.GetCharacters().Count);
            Assert.AreEqual(Size2EnemyCharacter2, _playerCharacterOrder.GetCharacterBySlotIndex(0));
            Assert.AreEqual(Size2EnemyCharacter2, _playerCharacterOrder.GetCharacterBySlotIndex(1));
            Assert.AreEqual(Character2, _playerCharacterOrder.GetCharacterBySlotIndex(2));
            Assert.AreEqual(null, _playerCharacterOrder.GetCharacterBySlotIndex(3));

            _playerCharacterOrder.MoveCharactersBackwardIntoSpaces(0); // include none

            Assert.AreEqual(2, _playerCharacterOrder.GetCharacters().Count);
            Assert.AreEqual(null, _playerCharacterOrder.GetCharacterBySlotIndex(0));
            Assert.AreEqual(Size2EnemyCharacter2, _playerCharacterOrder.GetCharacterBySlotIndex(1));
            Assert.AreEqual(Size2EnemyCharacter2, _playerCharacterOrder.GetCharacterBySlotIndex(2));
            Assert.AreEqual(Character2, _playerCharacterOrder.GetCharacterBySlotIndex(3));

            _playerCharacterOrder = new CharacterOrder(Character2, Size2EnemyCharacter2, Size2EnemyCharacter2, null);

            Assert.AreEqual(2, _playerCharacterOrder.GetCharacters().Count);
            Assert.AreEqual(Character2, _playerCharacterOrder.GetCharacterBySlotIndex(0));
            Assert.AreEqual(Size2EnemyCharacter2, _playerCharacterOrder.GetCharacterBySlotIndex(1));
            Assert.AreEqual(Size2EnemyCharacter2, _playerCharacterOrder.GetCharacterBySlotIndex(2));
            Assert.AreEqual(null, _playerCharacterOrder.GetCharacterBySlotIndex(3));

            _playerCharacterOrder.MoveCharactersBackwardIntoSpaces(0); // include none

            Assert.AreEqual(2, _playerCharacterOrder.GetCharacters().Count);
            Assert.AreEqual(null, _playerCharacterOrder.GetCharacterBySlotIndex(0));
            Assert.AreEqual(Character2, _playerCharacterOrder.GetCharacterBySlotIndex(1));
            Assert.AreEqual(Size2EnemyCharacter2, _playerCharacterOrder.GetCharacterBySlotIndex(2));
            Assert.AreEqual(Size2EnemyCharacter2, _playerCharacterOrder.GetCharacterBySlotIndex(3));
        }

        [Test]
        public void AssertMoveForwardIntoSlotsWorksWithIncludeParameter()
        {
            _playerCharacterOrder = new CharacterOrder(null, null, null, Character1);

            Assert.AreEqual(1, _playerCharacterOrder.GetCharacters().Count);
            Assert.AreEqual(null, _playerCharacterOrder.GetCharacterBySlotIndex(0));
            Assert.AreEqual(null, _playerCharacterOrder.GetCharacterBySlotIndex(1));
            Assert.AreEqual(null, _playerCharacterOrder.GetCharacterBySlotIndex(2));
            Assert.AreEqual(Character1, _playerCharacterOrder.GetCharacterBySlotIndex(3));

            _playerCharacterOrder.MoveCharactersForwardIntoSpaces(3); // include slot 3 and below

            Assert.AreEqual(1, _playerCharacterOrder.GetCharacters().Count);
            Assert.AreEqual(Character1, _playerCharacterOrder.GetCharacterBySlotIndex(0));
            Assert.AreEqual(null, _playerCharacterOrder.GetCharacterBySlotIndex(1));
            Assert.AreEqual(null, _playerCharacterOrder.GetCharacterBySlotIndex(2));
            Assert.AreEqual(null, _playerCharacterOrder.GetCharacterBySlotIndex(3));

            _playerCharacterOrder = new CharacterOrder(null, null, Character1, null);

            Assert.AreEqual(1, _playerCharacterOrder.GetCharacters().Count);
            Assert.AreEqual(null, _playerCharacterOrder.GetCharacterBySlotIndex(0));
            Assert.AreEqual(null, _playerCharacterOrder.GetCharacterBySlotIndex(1));
            Assert.AreEqual(Character1, _playerCharacterOrder.GetCharacterBySlotIndex(2));
            Assert.AreEqual(null, _playerCharacterOrder.GetCharacterBySlotIndex(3));

            _playerCharacterOrder.MoveCharactersForwardIntoSpaces(1); // include slot 1 and below

            Assert.AreEqual(1, _playerCharacterOrder.GetCharacters().Count);
            Assert.AreEqual(null, _playerCharacterOrder.GetCharacterBySlotIndex(0));
            Assert.AreEqual(null, _playerCharacterOrder.GetCharacterBySlotIndex(1));
            Assert.AreEqual(Character1, _playerCharacterOrder.GetCharacterBySlotIndex(2));
            Assert.AreEqual(null, _playerCharacterOrder.GetCharacterBySlotIndex(3));

            _playerCharacterOrder = new CharacterOrder(Character1, null, Character2, null);

            Assert.AreEqual(2, _playerCharacterOrder.GetCharacters().Count);
            Assert.AreEqual(Character1, _playerCharacterOrder.GetCharacterBySlotIndex(0));
            Assert.AreEqual(null, _playerCharacterOrder.GetCharacterBySlotIndex(1));
            Assert.AreEqual(Character2, _playerCharacterOrder.GetCharacterBySlotIndex(2));
            Assert.AreEqual(null, _playerCharacterOrder.GetCharacterBySlotIndex(3));

            _playerCharacterOrder.MoveCharactersForwardIntoSpaces(2); // include all

            Assert.AreEqual(2, _playerCharacterOrder.GetCharacters().Count);
            Assert.AreEqual(Character1, _playerCharacterOrder.GetCharacterBySlotIndex(0));
            Assert.AreEqual(Character2, _playerCharacterOrder.GetCharacterBySlotIndex(1));
            Assert.AreEqual(null, _playerCharacterOrder.GetCharacterBySlotIndex(2));
            Assert.AreEqual(null, _playerCharacterOrder.GetCharacterBySlotIndex(3));

            _playerCharacterOrder = new CharacterOrder(null, Character1, Character2, Character3);

            Assert.AreEqual(3, _playerCharacterOrder.GetCharacters().Count);
            Assert.AreEqual(null, _playerCharacterOrder.GetCharacterBySlotIndex(0));
            Assert.AreEqual(Character1, _playerCharacterOrder.GetCharacterBySlotIndex(1));
            Assert.AreEqual(Character2, _playerCharacterOrder.GetCharacterBySlotIndex(2));
            Assert.AreEqual(Character3, _playerCharacterOrder.GetCharacterBySlotIndex(3));

            _playerCharacterOrder.MoveCharactersForwardIntoSpaces(2); // include 1 and below

            Assert.AreEqual(3, _playerCharacterOrder.GetCharacters().Count);
            Assert.AreEqual(Character1, _playerCharacterOrder.GetCharacterBySlotIndex(0));
            Assert.AreEqual(Character2, _playerCharacterOrder.GetCharacterBySlotIndex(1));
            Assert.AreEqual(null, _playerCharacterOrder.GetCharacterBySlotIndex(2));
            Assert.AreEqual(Character3, _playerCharacterOrder.GetCharacterBySlotIndex(3));

            _playerCharacterOrder = new CharacterOrder(null, Size2EnemyCharacter2, Size2EnemyCharacter2, null);

            Assert.AreEqual(1, _playerCharacterOrder.GetCharacters().Count);
            Assert.AreEqual(null, _playerCharacterOrder.GetCharacterBySlotIndex(0));
            Assert.AreEqual(Size2EnemyCharacter2, _playerCharacterOrder.GetCharacterBySlotIndex(1));
            Assert.AreEqual(Size2EnemyCharacter2, _playerCharacterOrder.GetCharacterBySlotIndex(2));
            Assert.AreEqual(null, _playerCharacterOrder.GetCharacterBySlotIndex(3));

            _playerCharacterOrder.MoveCharactersForwardIntoSpaces(2); // include none

            Assert.AreEqual(1, _playerCharacterOrder.GetCharacters().Count);
            Assert.AreEqual(Size2EnemyCharacter2, _playerCharacterOrder.GetCharacterBySlotIndex(0));
            Assert.AreEqual(Size2EnemyCharacter2, _playerCharacterOrder.GetCharacterBySlotIndex(1));
            Assert.AreEqual(null, _playerCharacterOrder.GetCharacterBySlotIndex(2));
            Assert.AreEqual(null, _playerCharacterOrder.GetCharacterBySlotIndex(3));

            _playerCharacterOrder = new CharacterOrder(null, Size2EnemyCharacter2, Size2EnemyCharacter2, Character2);

            Assert.AreEqual(2, _playerCharacterOrder.GetCharacters().Count);
            Assert.AreEqual(null, _playerCharacterOrder.GetCharacterBySlotIndex(0));
            Assert.AreEqual(Size2EnemyCharacter2, _playerCharacterOrder.GetCharacterBySlotIndex(1));
            Assert.AreEqual(Size2EnemyCharacter2, _playerCharacterOrder.GetCharacterBySlotIndex(2));
            Assert.AreEqual(Character2, _playerCharacterOrder.GetCharacterBySlotIndex(3));

            _playerCharacterOrder.MoveCharactersForwardIntoSpaces(3); // include none

            Assert.AreEqual(2, _playerCharacterOrder.GetCharacters().Count);
            Assert.AreEqual(Size2EnemyCharacter2, _playerCharacterOrder.GetCharacterBySlotIndex(0));
            Assert.AreEqual(Size2EnemyCharacter2, _playerCharacterOrder.GetCharacterBySlotIndex(1));
            Assert.AreEqual(Character2, _playerCharacterOrder.GetCharacterBySlotIndex(2));
            Assert.AreEqual(null, _playerCharacterOrder.GetCharacterBySlotIndex(3));
        }

        [Test]
        public void AssertMoveCharacterWorksWithSize1Characters()
        {
            _playerCharacterOrder = new CharacterOrder(Character1, Character2, Character3, Character4);

            Assert.AreEqual(4, _playerCharacterOrder.GetCharacters().Count);
            Assert.AreEqual(Character1, _playerCharacterOrder.GetCharacterBySlotIndex(0));
            Assert.AreEqual(Character2, _playerCharacterOrder.GetCharacterBySlotIndex(1));
            Assert.AreEqual(Character3, _playerCharacterOrder.GetCharacterBySlotIndex(2));
            Assert.AreEqual(Character4, _playerCharacterOrder.GetCharacterBySlotIndex(3));

            _playerCharacterOrder.MoveCharacter(Character1, 1); // Move character1 to slot 1. 

            Assert.AreEqual(4, _playerCharacterOrder.GetCharacters().Count);
            Assert.AreEqual(Character2, _playerCharacterOrder.GetCharacterBySlotIndex(0));
            Assert.AreEqual(Character1, _playerCharacterOrder.GetCharacterBySlotIndex(1));
            Assert.AreEqual(Character3, _playerCharacterOrder.GetCharacterBySlotIndex(2));
            Assert.AreEqual(Character4, _playerCharacterOrder.GetCharacterBySlotIndex(3));

            _playerCharacterOrder = new CharacterOrder(Character1, Character2, Character3, Character4);

            Assert.AreEqual(4, _playerCharacterOrder.GetCharacters().Count);
            Assert.AreEqual(Character1, _playerCharacterOrder.GetCharacterBySlotIndex(0));
            Assert.AreEqual(Character2, _playerCharacterOrder.GetCharacterBySlotIndex(1));
            Assert.AreEqual(Character3, _playerCharacterOrder.GetCharacterBySlotIndex(2));
            Assert.AreEqual(Character4, _playerCharacterOrder.GetCharacterBySlotIndex(3));

            _playerCharacterOrder.MoveCharacter(Character3, -1); // Move character3 to slot 2. 

            Assert.AreEqual(4, _playerCharacterOrder.GetCharacters().Count);
            Assert.AreEqual(Character1, _playerCharacterOrder.GetCharacterBySlotIndex(0));
            Assert.AreEqual(Character3, _playerCharacterOrder.GetCharacterBySlotIndex(1));
            Assert.AreEqual(Character2, _playerCharacterOrder.GetCharacterBySlotIndex(2));
            Assert.AreEqual(Character4, _playerCharacterOrder.GetCharacterBySlotIndex(3));

            _playerCharacterOrder = new CharacterOrder(Character1, Character2, Character3, Character4);

            Assert.AreEqual(4, _playerCharacterOrder.GetCharacters().Count);
            Assert.AreEqual(Character1, _playerCharacterOrder.GetCharacterBySlotIndex(0));
            Assert.AreEqual(Character2, _playerCharacterOrder.GetCharacterBySlotIndex(1));
            Assert.AreEqual(Character3, _playerCharacterOrder.GetCharacterBySlotIndex(2));
            Assert.AreEqual(Character4, _playerCharacterOrder.GetCharacterBySlotIndex(3));

            _playerCharacterOrder.MoveCharacter(Character1, 13); // Move character3 to the very back. 

            Assert.AreEqual(4, _playerCharacterOrder.GetCharacters().Count);
            Assert.AreEqual(Character2, _playerCharacterOrder.GetCharacterBySlotIndex(0));
            Assert.AreEqual(Character3, _playerCharacterOrder.GetCharacterBySlotIndex(1));
            Assert.AreEqual(Character4, _playerCharacterOrder.GetCharacterBySlotIndex(2));
            Assert.AreEqual(Character1, _playerCharacterOrder.GetCharacterBySlotIndex(3));

            _playerCharacterOrder = new CharacterOrder(Character1, Character2, Character3, Character4);

            Assert.AreEqual(4, _playerCharacterOrder.GetCharacters().Count);
            Assert.AreEqual(Character1, _playerCharacterOrder.GetCharacterBySlotIndex(0));
            Assert.AreEqual(Character2, _playerCharacterOrder.GetCharacterBySlotIndex(1));
            Assert.AreEqual(Character3, _playerCharacterOrder.GetCharacterBySlotIndex(2));
            Assert.AreEqual(Character4, _playerCharacterOrder.GetCharacterBySlotIndex(3));

            _playerCharacterOrder.MoveCharacter(Character4, -13); // Move character3 to the very front. 

            Assert.AreEqual(4, _playerCharacterOrder.GetCharacters().Count);
            Assert.AreEqual(Character4, _playerCharacterOrder.GetCharacterBySlotIndex(0));
            Assert.AreEqual(Character1, _playerCharacterOrder.GetCharacterBySlotIndex(1));
            Assert.AreEqual(Character2, _playerCharacterOrder.GetCharacterBySlotIndex(2));
            Assert.AreEqual(Character3, _playerCharacterOrder.GetCharacterBySlotIndex(3));
        }

        [Test]
        public void AssertMoveCharacterWorksWithSize2Characters()
        {
            _playerCharacterOrder = new CharacterOrder(Character1, Character2, Size2EnemyCharacter2, Size2EnemyCharacter2);

            Assert.AreEqual(3, _playerCharacterOrder.GetCharacters().Count);
            Assert.AreEqual(Character1, _playerCharacterOrder.GetCharacterBySlotIndex(0));
            Assert.AreEqual(Character2, _playerCharacterOrder.GetCharacterBySlotIndex(1));
            Assert.AreEqual(Size2EnemyCharacter2, _playerCharacterOrder.GetCharacterBySlotIndex(2));
            Assert.AreEqual(Size2EnemyCharacter2, _playerCharacterOrder.GetCharacterBySlotIndex(3));

            _playerCharacterOrder.MoveCharacter(Size2EnemyCharacter2, -2); // Move Size2EnemyCharacter2 to slot 0. 

            Assert.AreEqual(3, _playerCharacterOrder.GetCharacters().Count);
            Assert.AreEqual(Size2EnemyCharacter2, _playerCharacterOrder.GetCharacterBySlotIndex(0));
            Assert.AreEqual(Size2EnemyCharacter2, _playerCharacterOrder.GetCharacterBySlotIndex(1));
            Assert.AreEqual(Character1, _playerCharacterOrder.GetCharacterBySlotIndex(2));
            Assert.AreEqual(Character2, _playerCharacterOrder.GetCharacterBySlotIndex(3));

            _playerCharacterOrder = new CharacterOrder(Character1, Character2, Size2EnemyCharacter2, Size2EnemyCharacter2);

            Assert.AreEqual(3, _playerCharacterOrder.GetCharacters().Count);
            Assert.AreEqual(Character1, _playerCharacterOrder.GetCharacterBySlotIndex(0));
            Assert.AreEqual(Character2, _playerCharacterOrder.GetCharacterBySlotIndex(1));
            Assert.AreEqual(Size2EnemyCharacter2, _playerCharacterOrder.GetCharacterBySlotIndex(2));
            Assert.AreEqual(Size2EnemyCharacter2, _playerCharacterOrder.GetCharacterBySlotIndex(3));

            _playerCharacterOrder.MoveCharacter(Size2EnemyCharacter2, -12); // Move Size2EnemyCharacter2 to slot 0, even though number overshoots. 

            Assert.AreEqual(3, _playerCharacterOrder.GetCharacters().Count);
            Assert.AreEqual(Size2EnemyCharacter2, _playerCharacterOrder.GetCharacterBySlotIndex(0));
            Assert.AreEqual(Size2EnemyCharacter2, _playerCharacterOrder.GetCharacterBySlotIndex(1));
            Assert.AreEqual(Character1, _playerCharacterOrder.GetCharacterBySlotIndex(2));
            Assert.AreEqual(Character2, _playerCharacterOrder.GetCharacterBySlotIndex(3));

            _playerCharacterOrder = new CharacterOrder(Size2EnemyCharacter4, Size2EnemyCharacter4, Size2EnemyCharacter2, Size2EnemyCharacter2);

            Assert.AreEqual(2, _playerCharacterOrder.GetCharacters().Count);
            Assert.AreEqual(Size2EnemyCharacter4, _playerCharacterOrder.GetCharacterBySlotIndex(0));
            Assert.AreEqual(Size2EnemyCharacter4, _playerCharacterOrder.GetCharacterBySlotIndex(1));
            Assert.AreEqual(Size2EnemyCharacter2, _playerCharacterOrder.GetCharacterBySlotIndex(2));
            Assert.AreEqual(Size2EnemyCharacter2, _playerCharacterOrder.GetCharacterBySlotIndex(3));

            _playerCharacterOrder.MoveCharacter(Size2EnemyCharacter4, 12); // Move Size2EnemyCharacter2 to very back, even though number overshoots. 

            Assert.AreEqual(2, _playerCharacterOrder.GetCharacters().Count);
            Assert.AreEqual(Size2EnemyCharacter2, _playerCharacterOrder.GetCharacterBySlotIndex(0));
            Assert.AreEqual(Size2EnemyCharacter2, _playerCharacterOrder.GetCharacterBySlotIndex(1));
            Assert.AreEqual(Size2EnemyCharacter4, _playerCharacterOrder.GetCharacterBySlotIndex(2));
            Assert.AreEqual(Size2EnemyCharacter4, _playerCharacterOrder.GetCharacterBySlotIndex(3));
        }

            #endregion

            #region Protected Functions
            #endregion

            #region Private Functions
            #endregion

            #region Inner Classes
            #endregion
        }
}