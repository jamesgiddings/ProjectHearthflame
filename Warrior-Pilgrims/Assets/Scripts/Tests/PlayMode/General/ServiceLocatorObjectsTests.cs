using System.Collections;
using UnityEngine;
using NUnit.Framework;
using UnityEngine.TestTools;
public class ServiceLocatorObjectsTests : BasicPlayModeTest
{
    [UnityTest]
    public IEnumerator Assert_That_Service_Locator_Objects_Are_Not_Null()
    {
        Assert.NotNull(ServiceLocator.Instance);

        yield return new WaitUntil(() => ServiceLocator.Instance.GameStateManager.State == ServiceLocator.Instance.ExplorationState); // WaitForSceneToFullyLoadIn

        Assert.NotNull(ServiceLocator.Instance.CharacterGameObjectManager);
        Assert.NotNull(ServiceLocator.Instance.CharacterModel);
        yield return null;
    }
}
