using NUnit.Framework;
using System.Collections;
using UnityEngine;
using UnityEngine.TestTools;

[TestFixture]
public class BattleStartsWithCorrectCharactersTest : BasicPlayModeTest
{
    [UnityTest]
    [Explicit, Category("integration")]
    public IEnumerator TestBattleLoadsWithCorrectCharacters()
    {
        yield return new WaitUntil(() => ServiceLocator.Instance.GameStateManager.State == ServiceLocator.Instance.ExplorationState); // WaitForSceneToFullyLoadIn
        ServiceLocator.Instance.CharacterGameObjectManager.CharacterBattlerDictionary[ServiceLocator.Instance.CharacterModel.Slot1PlayerCharacter].GetComponent<ExternalMover>().SetMove(1f);
        ServiceLocator.Instance.CharacterGameObjectManager.CharacterBattlerDictionary[ServiceLocator.Instance.CharacterModel.Slot1PlayerCharacter].GetComponent<ExternalMover>().enabled = true;
        yield return new WaitUntil(() => ServiceLocator.Instance.GameStateManager.State == ServiceLocator.Instance.BattleState);
        ServiceLocator.Instance.CharacterGameObjectManager.CharacterBattlerDictionary[ServiceLocator.Instance.CharacterModel.Slot1PlayerCharacter].GetComponent<ExternalMover>().enabled = false;
        yield return new WaitUntil(() => ServiceLocator.Instance.BattleStateManager.State == ServiceLocator.Instance.BattleStartState);
        Assert.True(ServiceLocator.Instance.BattleStateManager.State == ServiceLocator.Instance.BattleStartState);
        Assert.AreEqual(6, ServiceLocator.Instance.BattleDataModel.OrderedBattlersList.Count);
        yield return null;
    }
}
