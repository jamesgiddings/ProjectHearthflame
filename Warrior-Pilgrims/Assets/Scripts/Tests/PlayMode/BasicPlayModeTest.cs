using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using GramophoneUtils.Stats;
using GramophoneUtils.Items;
using GramophoneUtils.Items.Containers;
using GramophoneUtils.Characters;
using UnityEngine.SceneManagement;
using GramophoneUtils.SavingLoading;

[TestFixture]
public class BasicPlayModeTest
{
    protected ServiceLocatorObject ServiceLocatorObject;
    protected TestObjectReferences TestObjectReferences;
    protected Constants Constants;

    protected ServiceLocator MockServiceLocator;

    protected ICharacterModel CharacterModel;

    protected Character Character1Blueprint;
    protected Character Character2Blueprint;
    protected Character Character3Blueprint;

    protected Character EnemyCharacter1Blueprint;
    protected Character Size2EnemyCharacter2Blueprint;
    protected Character EnemyCharacter3Blueprint;

    protected Character Character1;
    protected Character Character2;
    protected Character Character3;
    protected Character Character4;

    protected Character EnemyCharacter1;
    protected Character Size2EnemyCharacter2;
    protected Character EnemyCharacter3;
    protected Character Size2EnemyCharacter4;

    protected CharacterClass HearthPriest;
    protected CharacterClass Duelist;
    protected CharacterClass TheRuneKnight;
    protected CharacterClass Musketeer;

    protected EquipmentItem EquipmentItem1;

    protected ISaveable MockSaveable;
    protected object testObject;



    [SetUp]
    public virtual void SetUp()
    {
        ServiceLocatorObject = AssetDatabase.LoadAssetAtPath<ServiceLocatorObject>(ServiceLocatorObject.PathToServiceLocatorObject);
        TestObjectReferences = ServiceLocatorObject.TestObjectReferences;
        Constants = ServiceLocatorObject.Constants;

        CharacterModel = ServiceLocatorObject.CharacterModel;

        Character1Blueprint = TestObjectReferences.Character1;
        Character2Blueprint = TestObjectReferences.Character2;
        Character3Blueprint = TestObjectReferences.Character3;

        EnemyCharacter1Blueprint = TestObjectReferences.Enemy1;
        Size2EnemyCharacter2Blueprint = TestObjectReferences.Enemy2;
        EnemyCharacter3Blueprint = TestObjectReferences.Enemy3;

        HearthPriest = TestObjectReferences.HearthPriest;
        Duelist = TestObjectReferences.Duelist;
        TheRuneKnight = TestObjectReferences.TheRuneKnight;
        Musketeer = TestObjectReferences.Musketeer;

        EquipmentItem1 = TestObjectReferences.EquipmentItem1;
        testObject = new object();
        SceneManager.LoadScene("Test_Scene", LoadSceneMode.Single);
    }

    [TearDown]
    public void TearDown()
    {
        Debug.Log("We are here -------------<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<");
        SceneManager.LoadScene("Test_Scene", LoadSceneMode.Single);
    }

    #region Utilities
    protected void StopSlot1CharacterMoving()
    {
        ServiceLocator.Instance.CharacterGameObjectManager.CharacterBattlerDictionary[ServiceLocator.Instance.CharacterModel.Slot1PlayerCharacter].GetComponent<ExternalMover>().enabled = false;
    }

    protected void StartSlot1CharacterMoving()
    {
        ServiceLocator.Instance.CharacterGameObjectManager.CharacterBattlerDictionary[ServiceLocator.Instance.CharacterModel.Slot1PlayerCharacter].GetComponent<ExternalMover>().SetMove(1f);
        ServiceLocator.Instance.CharacterGameObjectManager.CharacterBattlerDictionary[ServiceLocator.Instance.CharacterModel.Slot1PlayerCharacter].GetComponent<ExternalMover>().enabled = true;
    }

    #endregion

}
