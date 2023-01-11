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

[TestFixture]
public class BasicPlayModeTest
{
	protected Character Character1Blueprint;
	protected Character Character2Blueprint;
	protected Character Character3Blueprint;

	protected Character Character1;
	protected Character Character2;
	protected Character Character3;


    [SetUp]
    public void SetUp()
    {
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
