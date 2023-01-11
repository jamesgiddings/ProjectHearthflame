using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

[TestFixture]
public class ItemPickupTest : BasicPlayModeTest
{    
    [UnityTest]
    [Explicit, Category("integration")]
    public IEnumerator TestThatPartyInventoryContainsItemsAfterCollidingWithTrigger()
    {
        yield return new WaitUntil(() => ServiceLocator.Instance.GameStateManager.State == ServiceLocator.Instance.ExplorationState);
        StartSlot1CharacterMoving();
        yield return new WaitUntil(() => ServiceLocator.Instance.CharacterModel.PartyInventory.GetAllUniqueItems().Count > 0);
        StopSlot1CharacterMoving();
        Assert.AreEqual(6, ServiceLocator.Instance.CharacterModel.PartyInventory.GetAllUniqueItems().Count);
    }
}
