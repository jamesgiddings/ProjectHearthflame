using NUnit.Framework;

public class UIDTest : BasicEditModeTest
{
    [Test]
    public void ResourceIDTestSimplePasses()
    {
        Assert.AreEqual(TestObjectReferences.Resource.UID, "de301b3a5de18c7428d78e832f4900a2");
    }
}
