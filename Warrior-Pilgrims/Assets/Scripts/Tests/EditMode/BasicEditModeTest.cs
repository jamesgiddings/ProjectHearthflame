using NUnit.Framework;
using NSubstitute;
using UnityEditor;
using GramophoneUtils.Items;
using GramophoneUtils.SavingLoading;
using GramophoneUtils.Stats;

public class BasicEditModeTest
{
    protected ServiceLocatorObject ServiceLocatorObject;
    protected TestObjectReferences TestObjectReferences;
	protected Constants Constants;

	protected ServiceLocator MockServiceLocator;

	protected CharacterModel CharacterModel;

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
