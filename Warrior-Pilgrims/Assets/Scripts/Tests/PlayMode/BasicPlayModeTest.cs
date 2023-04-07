
using NUnit.Framework;
using UnityEngine;
using UnityEditor;
using GramophoneUtils.Stats;
using GramophoneUtils.Items;
using UnityEngine.SceneManagement;
using GramophoneUtils.SavingLoading;

[TestFixture]
public class BasicPlayModeTest : MonoBehaviour
{
    protected ServiceLocatorObject ServiceLocatorObject;
    protected TestObjectReferences TestObjectReferences;
    protected Constants Constants;

    protected ServiceLocator MockServiceLocator;

    protected ICharacterModel CharacterModel;

    protected ICharacter Character1Blueprint;
    protected ICharacter Character2Blueprint;
    protected ICharacter Character3Blueprint;

    protected ICharacter EnemyCharacter1Blueprint;
    protected ICharacter Size2EnemyCharacter2Blueprint;
    protected ICharacter EnemyCharacter3Blueprint;

    protected ICharacter Character1;
    protected ICharacter Character2;
    protected ICharacter Character3;
    protected ICharacter Character4;

    protected ICharacter EnemyCharacter1;
    protected ICharacter Size2EnemyCharacter2;
    protected ICharacter EnemyCharacter3;
    protected ICharacter Size2EnemyCharacter4;

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

        ServiceLocatorObject.SavingSystem.Load(TestObjectReferences.TestSaveFile);
    }

    [TearDown]
    public async void TearDown()
    {
/*        ServiceLocator.Destroy(ServiceLocator.Instance.gameObject);
        AsyncOperation asyncOperation = SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().name, UnloadSceneOptions.UnloadAllEmbeddedSceneObjects);
        await Task.Run(() => UnloadSceneWait(5000));
        SceneManager.OpenScene("Blank_Scene");*/
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
