using NUnit.Framework;
using System.Collections;
using UnityEngine;
using UnityEngine.TestTools;

[TestFixture]
public class MovementTests : BasicPlayModeTest
{
    [UnityTest]
    [Explicit, Category("integration")]
    public IEnumerator TestPlayerCanMoveTowardsBattle()
    {
        yield return new WaitUntil(() => ServiceLocator.Instance.GameStateManager.State == ServiceLocator.Instance.ExplorationState);
        StartSlot1CharacterMoving();
        yield return new WaitUntil(() => ServiceLocator.Instance.GameStateManager.State == ServiceLocator.Instance.BattleState);
        StopSlot1CharacterMoving();
        Assert.True(ServiceLocator.Instance.GameStateManager.State == ServiceLocator.Instance.BattleState);
        yield return null;
    }
}
