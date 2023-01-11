using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class InventoriesStartEmptyTest : BasicPlayModeTest
{
    [UnityTest]
    public IEnumerator TestThatPartyInventoryStartsEmpty()
    {
        yield return new WaitUntil(() => ServiceLocator.Instance.GameStateManager.State == ServiceLocator.Instance.ExplorationState);
        Assert.AreEqual(0, ServiceLocator.Instance.CharacterModel.PartyInventory.GetAllUniqueItems().Count);
    }
}
