using NUnit.Framework;
using NSubstitute;
using UnityEditor;
using GramophoneUtils.Items;
using GramophoneUtils.Characters;
using GramophoneUtils.SavingLoading;
using GramophoneUtils.Stats;

public class BasicEditModeTest
{
    protected ServiceLocatorObject ServiceLocatorObject;
    protected TestObjectReferences TestObjectReferences;
	protected Constants Constants;

	protected ServiceLocator MockServiceLocator;

	protected CharacterModel CharacterModel;

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

    protected ISkill Shoot;
    protected ISkill Bash;
    protected ISkill Cleave;
    protected ISkill MagicShield;


	protected ISaveable MockSaveable;
	protected object testObject;

	[SetUp]
    protected virtual void SetUp()
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

        Shoot = TestObjectReferences.Shoot;
        Cleave = TestObjectReferences.Cleave;
        Bash = TestObjectReferences.Bash;
		MagicShield = TestObjectReferences.MagicShield;

        EquipmentItem1 = TestObjectReferences.EquipmentItem1;
        testObject = new object();
		CreateMockSaveable();
    }

	private void CreateMockSaveable()
	{
		MockSaveable = Substitute.For<ISaveable>();
		
		
		MockSaveable.CaptureState().Returns(testObject);
	}
}
