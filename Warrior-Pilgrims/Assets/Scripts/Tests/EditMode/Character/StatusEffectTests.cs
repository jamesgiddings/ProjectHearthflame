using GramophoneUtils.Characters;
using GramophoneUtils.Stats;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading;

public class StatusEffectTests : BasicEditModeTest
{
    #region Attributes/Fields/Properties

    private StatusEffectBlueprint _testStatusEffectBlueprint;
    private StatusEffectBlueprint _testStatusEffectBlueprint1;

    private Character _character1;
    private Character _character2;

    #endregion

    #region Constructors
    #endregion

    #region Callbacks
    #endregion

    #region Public Functions

    protected override void SetUp()
    {
        base.SetUp();
        _testStatusEffectBlueprint = TestObjectReferences.TestStatusEffect;
        _testStatusEffectBlueprint1 = TestObjectReferences.TestStatusEffect1;
        _character1 = Character1Blueprint.Instance();
        _character2 = Character2Blueprint.Instance();
    }

    [Test]
    public void Given_StatusEffectBlueprintWhichIsNotNull_WhenInstanceBlueprint_ThenReturnsStatusEffectWhichIsNotNull()
    {
        IStatusEffect testStatusEffect = ServiceLocatorObject.Instance.StatusEffectFactory.CreateStatusEffectFromBlueprint(_testStatusEffectBlueprint, (_character1));
        Assert.That(_testStatusEffectBlueprint, Is.Not.Null);
        Assert.That(testStatusEffect, Is.Not.Null);
    }

    [Test]
    public void Given_StatusEffectBlueprintWithDuration_WhenInstanceBlueprintCalled_ThenReturnsStatusEffectWithSameDuration()
    {
        Assert.That(_testStatusEffectBlueprint.Duration, Is.Not.Null);
        IStatusEffect testStatusEffect = ServiceLocatorObject.Instance.StatusEffectFactory.CreateStatusEffectFromBlueprint(_testStatusEffectBlueprint, (_character1));
        Assert.AreEqual(_testStatusEffectBlueprint.Duration, testStatusEffect.Duration);
    }

    [Test]
    public void Given_StatusEffectBlueprint_WhenInstanceBlueprintCalled_ThenReturnsStatusEffectWithSameValueForDamageMustLandForOtherEffectsToLand()
    {
        IStatusEffect testStatusEffect = ServiceLocatorObject.Instance.StatusEffectFactory.CreateStatusEffectFromBlueprint(_testStatusEffectBlueprint, (_character1));
        Assert.AreEqual(_testStatusEffectBlueprint.DamageMustLandForOtherEffectsToLand, testStatusEffect.DamageMustLandForOtherEffectsToLand);
    }

    [Test]
    public void Given_StatusEffectBlueprintWithStatusEffectType_WhenInstanceBlueprintCalled_ThenReturnsStatusEffectWithSameStatusEffectType()
    {
        Assert.That(_testStatusEffectBlueprint.StatusEffectTypeFlag, Is.Not.EqualTo(StatusEffectType.None));
        IStatusEffect testStatusEffect = ServiceLocatorObject.Instance.StatusEffectFactory.CreateStatusEffectFromBlueprint(_testStatusEffectBlueprint, (_character1));
        Assert.AreEqual(_testStatusEffectBlueprint.StatusEffectTypeFlag, testStatusEffect.StatusEffectTypeWrapper.StatusEffectTypeFlag);
    }

    [Test]
    public void Given_StatusEffectBlueprintWithTurnDamageAndHealingEffects_WhenInstanceBlueprintCalled_ThenReturnsStatusEffectWithSameNumberOrLessOfTurnDamageAndHealingEffects()
    {
        Assert.That(_testStatusEffectBlueprint.TurnDamageAndHealingEffects, Is.Not.Null);
        IStatusEffect testStatusEffect = ServiceLocatorObject.Instance.StatusEffectFactory.CreateStatusEffectFromBlueprint(_testStatusEffectBlueprint, (_character1));
        Assert.That(testStatusEffect.TurnHealingEffects.Length, Is.LessThanOrEqualTo(_testStatusEffectBlueprint.TurnDamageAndHealingEffects.Length));
        Assert.That(testStatusEffect.TurnDamageEffects.Length, Is.LessThanOrEqualTo(_testStatusEffectBlueprint.TurnDamageAndHealingEffects.Length));
    }

    [Test]
    public void Given_StatusEffectBlueprintWithTurnDamageAndHealingEffects_WhenInstanceBlueprintCalled_ThenReturnsStatusEffectWithSameTurnDamageAndHealingEffectsOnceReversed()
    {
        IStatusEffect testStatusEffect = ServiceLocatorObject.Instance.StatusEffectFactory.CreateStatusEffectFromBlueprint(_testStatusEffectBlueprint, (_character1));

        Assert.That(_testStatusEffectBlueprint.TurnDamageAndHealingEffects, Is.Not.Null);

        // Compare TurnHealingEffects
        Assert.That(testStatusEffect.TurnHealingEffects.Length, Is.LessThanOrEqualTo(_testStatusEffectBlueprint.TurnDamageAndHealingEffects.Length));
        int n = 0;
        for (int i =
            _testStatusEffectBlueprint.TurnDamageAndHealingEffects.Length > testStatusEffect.TurnHealingEffects.Length ? testStatusEffect.TurnHealingEffects.Length - 1 : _testStatusEffectBlueprint.TurnDamageAndHealingEffects.Length - 1;
            i >= 0; i--)
        {
            for (int j = 0; j < _testStatusEffectBlueprint.TurnDamageAndHealingEffects[i].TurnHealingEffects.Length; j++)
            {
                if (j >= testStatusEffect.TurnHealingEffects[n].Length) { break; }
                Assert.That(_testStatusEffectBlueprint.TurnDamageAndHealingEffects[i].TurnHealingEffects[j].Value, Is.EqualTo(testStatusEffect.TurnHealingEffects[n][j].Value));
            }
            n++;
        }
       
        // Compare TurnDamageEffects
        Assert.That(testStatusEffect.TurnDamageEffects.Length, Is.LessThanOrEqualTo(_testStatusEffectBlueprint.TurnDamageAndHealingEffects.Length));

        n = 0;
        for (int i = 
            _testStatusEffectBlueprint.TurnDamageAndHealingEffects.Length > testStatusEffect.TurnDamageEffects.Length ? testStatusEffect.TurnDamageEffects.Length - 1 : _testStatusEffectBlueprint.TurnDamageAndHealingEffects.Length - 1; 
            i >= 0; i--)
        {
            for (int j = 0; j < _testStatusEffectBlueprint.TurnDamageAndHealingEffects[i].TurnDamageEffects.Length; j++)
            {
                if (j >= testStatusEffect.TurnHealingEffects[n].Length) { break; }
                Assert.That(_testStatusEffectBlueprint.TurnDamageAndHealingEffects[i].TurnDamageEffects[j].Value, Is.EqualTo(testStatusEffect.TurnDamageEffects[n][j].Value));
                Assert.That(_testStatusEffectBlueprint.TurnDamageAndHealingEffects[i].TurnDamageEffects[j].Element, Is.EqualTo(testStatusEffect.TurnDamageEffects[n][j].Element));
                Assert.That(_testStatusEffectBlueprint.TurnDamageAndHealingEffects[i].TurnDamageEffects[j].AttackType, Is.EqualTo(testStatusEffect.TurnDamageEffects[n][j].AttackType));
            }
            n++;
        }
    }

    [Test]
    public void Given_StatusEffectBlueprintWithStatModifiers_WhenInstanceBlueprintCalled_ThenReturnsStatusEffectWithSameNumberOfStatModifiersWithTheSameValues()
    {
        Assert.That(_testStatusEffectBlueprint.StatModifiers.Count, Is.GreaterThan(0));
        IStatusEffect testStatusEffect = ServiceLocatorObject.Instance.StatusEffectFactory.CreateStatusEffectFromBlueprint(_testStatusEffectBlueprint, _character1);
        Assert.AreEqual(_testStatusEffectBlueprint.StatModifiers[0].Value, testStatusEffect.StatModifiers[0].Value);
        Assert.AreEqual(_testStatusEffectBlueprint.StatModifiers[0].StatType, testStatusEffect.StatModifiers[0].StatType);
        Assert.AreEqual(_testStatusEffectBlueprint.StatModifiers[0].ModifierNumericType, testStatusEffect.StatModifiers[0].ModifierNumericType);
        Assert.AreEqual(_testStatusEffectBlueprint.StatModifiers[0].StatModifierType, testStatusEffect.StatModifiers[0].StatModifierType);
    }

    [Test]
    public async void Given_StatusEffect_WhenStatusEffectIsAddedToCharacterWithNoStatusEffects_ThenReturnsStatusEffectWithSameNumberOfStatModifiersWithTheSameValues()
    {
        CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

        // Given StatusEffect
        IStatusEffect testStatusEffect = ServiceLocatorObject.Instance.StatusEffectFactory.CreateStatusEffectFromBlueprint(_testStatusEffectBlueprint, null);
        Assert.That(testStatusEffect, Is.Not.Null);

        // When StatusEffectIsAddedToCharacterWithNoStatusEffects
        Assert.That(_character1.StatSystem.StatusEffects.Count, Is.EqualTo(0));
        await testStatusEffect.Apply(_character1, _character2, cancellationTokenSource);
        Assert.That(_character1.StatSystem.StatusEffects.Count, Is.EqualTo(1));
        Assert.AreEqual(testStatusEffect, _character1.StatSystem.StatusEffects[0]);
    }

    [Test]
    public async void Given_StatusEffect_WhenStatusEffectIsAddedToCharacterWithNoStatusEffects_ThenCharactersStatsIncreaseByTheCorrectValues()
    {
        CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

        // Given StatusEffect
        IStatusEffect testStatusEffect = ServiceLocatorObject.Instance.StatusEffectFactory.CreateStatusEffectFromBlueprint(_testStatusEffectBlueprint, null);
        Assert.That(testStatusEffect, Is.Not.Null);
        float _character1Dexterity = _character1.StatSystem.GetStat(Constants.Dexterity).Value;

        // When StatusEffectIsAddedToCharacterWithNoStatusEffects
        Assert.That(_character1.StatSystem.StatusEffects.Count, Is.EqualTo(0));
        await testStatusEffect.Apply(_character1, _character2, cancellationTokenSource);
        Assert.That(_character1.StatSystem.GetStat(Constants.Dexterity).Value, Is.EqualTo(_character1Dexterity/2));
        Assert.AreEqual(testStatusEffect, _character1.StatSystem.StatusEffects[0]);
    }

    [Test]
    public async void Given_TwoStatusEffectsThatAreTheSame_WhenTwoAdditivePercentageStatusEffectsAreAddedToCharacterWithNoStatusEffects_ThenCharactersStatsIncreaseByTheCorrectValues()
    {
        CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

        // Given StatusEffect
        IStatusEffect testStatusEffect = ServiceLocatorObject.Instance.StatusEffectFactory.CreateStatusEffectFromBlueprint(_testStatusEffectBlueprint, null);
        Assert.That(testStatusEffect, Is.Not.Null);
        float _character1Dexterity = _character1.StatSystem.GetStat(Constants.Dexterity).Value;

        // When StatusEffectIsAddedToCharacterWithNoStatusEffects
        Assert.That(_character1.StatSystem.StatusEffects.Count, Is.EqualTo(0));
        await testStatusEffect.Apply(_character1, _character2, cancellationTokenSource);
        await testStatusEffect.Apply(_character1, _character2, cancellationTokenSource);
        Assert.That(_character1.StatSystem.GetStat(Constants.Dexterity).Value, Is.EqualTo(0f));
        Assert.AreEqual(testStatusEffect, _character1.StatSystem.StatusEffects[0]);
        Assert.AreEqual(testStatusEffect, _character1.StatSystem.StatusEffects[1]);
    }

    [Test]
    public async void Given_TwoStatusEffectsThatAreTheSame_WhenTwoMultiplicativePercentageStatusEffectsAreAddedToCharacterWithNoStatusEffects_ThenCharactersStatsIncreaseByTheCorrectValues()
    {
        CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

        // Given StatusEffect
        IStatusEffect testStatusEffect = ServiceLocatorObject.Instance.StatusEffectFactory.CreateStatusEffectFromBlueprint(_testStatusEffectBlueprint1, null);
        Assert.That(testStatusEffect, Is.Not.Null);
        float _character1Speed = _character1.StatSystem.GetStat(Constants.Speed).Value;

        // When StatusEffectIsAddedToCharacterWithNoStatusEffects
        Assert.That(_character1.StatSystem.StatusEffects.Count, Is.EqualTo(0));
        await testStatusEffect.Apply(_character1, _character2, cancellationTokenSource);
        await testStatusEffect.Apply(_character1, _character2, cancellationTokenSource);
        Assert.That(_character1.StatSystem.GetStat(Constants.Speed).Value, Is.EqualTo(2.5f));
        Assert.AreEqual(testStatusEffect, _character1.StatSystem.StatusEffects[0]);
        Assert.AreEqual(testStatusEffect, _character1.StatSystem.StatusEffects[1]);
    }

    [Test]
    public async void Given_StatusEffectsWithStatusTypeBleed_WhenAddedToCharacterWithActiveStatusEffectTypeOfNoneAndNoResistances_ThenCharacterGetsActiveStatusTypeOfBleed()
    {
        CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

        // Given StatusEffect
        IStatusEffect testStatusEffect = ServiceLocatorObject.Instance.StatusEffectFactory.CreateStatusEffectFromBlueprint(_testStatusEffectBlueprint1, null);
        Assert.That(testStatusEffect, Is.Not.Null);
        Assert.That(testStatusEffect.StatusEffectTypeWrapper.HasStatusEffectType(StatusEffectType.Bleed));
        StatusEffectType characterStatusEffectType = _character1.StatSystem.ActiveStatusEffectTypes;
        Assert.That(characterStatusEffectType, Is.Not.Null);
        Assert.That(characterStatusEffectType, Is.EqualTo(StatusEffectType.None));

        // When StatusEffectIsAddedToCharacterWithNoStatusEffects
        Assert.That(_character1.StatSystem.StatusEffects.Count, Is.EqualTo(0));
        await testStatusEffect.Apply(_character1, _character2, cancellationTokenSource);
        characterStatusEffectType = _character1.StatSystem.ActiveStatusEffectTypes;
        Assert.That(characterStatusEffectType, Is.EqualTo(StatusEffectType.Bleed));
        Assert.AreEqual(testStatusEffect, _character1.StatSystem.StatusEffects[0]);
    }

    [Test]
    public async void Given_StatusEffectsWithStatusTypeBleed_WhenAddedToCharacterWithActiveStatusEffectTypeOfBurnAndNoResistances_ThenCharacterHasActiveStatusTypeOfBleedAndBurn()
    {
        CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

        // Given StatusEffect
        IStatusEffect testStatusEffect = ServiceLocatorObject.Instance.StatusEffectFactory.CreateStatusEffectFromBlueprint(_testStatusEffectBlueprint, null);
        IStatusEffect testStatusEffect1 = ServiceLocatorObject.Instance.StatusEffectFactory.CreateStatusEffectFromBlueprint(_testStatusEffectBlueprint1, null);

        Assert.That(testStatusEffect, Is.Not.Null);
        StatusEffectType characterStatusEffectType = _character1.StatSystem.ActiveStatusEffectTypes;
        Assert.That(characterStatusEffectType, Is.Not.Null);
        Assert.That(characterStatusEffectType, Is.EqualTo(StatusEffectType.None));

        // When StatusEffectIsAddedToCharacterWithNoStatusEffects
        Assert.That(_character1.StatSystem.StatusEffects.Count, Is.EqualTo(0));
        await testStatusEffect.Apply(_character1, _character2, cancellationTokenSource);
        characterStatusEffectType = _character1.StatSystem.ActiveStatusEffectTypes;
        Assert.That(characterStatusEffectType, Is.EqualTo(StatusEffectType.Burn));
        Assert.AreEqual(testStatusEffect, _character1.StatSystem.StatusEffects[0]);

        await testStatusEffect1.Apply(_character1, _character2, cancellationTokenSource);
        characterStatusEffectType = _character1.StatSystem.ActiveStatusEffectTypes;
        Assert.IsTrue(characterStatusEffectType.HasFlag(StatusEffectType.Burn) &&
              characterStatusEffectType.HasFlag(StatusEffectType.Bleed));
        Assert.AreEqual(testStatusEffect, _character1.StatSystem.StatusEffects[0]);
        Assert.AreEqual(testStatusEffect1, _character1.StatSystem.StatusEffects[1]);
    }

    [Test]
    public async void Given_StatusEffectsWithStatusTypeBleedAndBurn_WhenAddedToCharacterWithActiveStatusEffectTypeBurnAndNoResistances_ThenCharacterHasActiveStatusTypeOfBleedAndBurn()
    {
        CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

        // Given StatusEffect
        IStatusEffect testStatusEffect = ServiceLocatorObject.Instance.StatusEffectFactory.CreateStatusEffectFromBlueprint(_testStatusEffectBlueprint, null);
        IStatusEffect testStatusEffect1 = ServiceLocatorObject.Instance.StatusEffectFactory.CreateStatusEffectFromBlueprint(_testStatusEffectBlueprint1, null);

        Assert.That(testStatusEffect, Is.Not.Null);
        StatusEffectType characterStatusEffectType = _character1.StatSystem.ActiveStatusEffectTypes;
        Assert.That(characterStatusEffectType, Is.Not.Null);
        Assert.That(characterStatusEffectType, Is.EqualTo(StatusEffectType.None));

        // When StatusEffectIsAddedToCharacterWithNoStatusEffects
        Assert.That(_character1.StatSystem.StatusEffects.Count, Is.EqualTo(0));
        await testStatusEffect.Apply(_character1, _character2, cancellationTokenSource);
        characterStatusEffectType = _character1.StatSystem.ActiveStatusEffectTypes;
        Assert.That(characterStatusEffectType, Is.EqualTo(StatusEffectType.Burn));
        Assert.AreEqual(testStatusEffect, _character1.StatSystem.StatusEffects[0]);

        await testStatusEffect1.Apply(_character1, _character2, cancellationTokenSource);
        characterStatusEffectType = _character1.StatSystem.ActiveStatusEffectTypes;
        Assert.IsTrue(characterStatusEffectType.HasFlag(StatusEffectType.Burn) &&
              characterStatusEffectType.HasFlag(StatusEffectType.Bleed));
        Assert.AreEqual(testStatusEffect, _character1.StatSystem.StatusEffects[0]);
        Assert.AreEqual(testStatusEffect1, _character1.StatSystem.StatusEffects[1]);
    }

    [Test]
    public async void Given_StatusEffectWithDurationOf2IsAdded_WhenAdvanceCharacterTurnIsCalledTwice_ThenCharacterHasHadStatusEffectRemoved()
    {
        CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

        ServiceLocatorObject.Instance.CharacterModel.AddEnemyCharacters(new List<Character> { _character1 });

        // Given StatusEffectWithDurationOf2IsAdded
        IStatusEffect testStatusEffect = ServiceLocatorObject.Instance.StatusEffectFactory.CreateStatusEffectFromBlueprint(_testStatusEffectBlueprint, null);
        Assert.That(testStatusEffect, Is.Not.Null);
        Assert.That(_character1.StatSystem.StatusEffects.Count, Is.EqualTo(0));
        await testStatusEffect.Apply(_character1, _character2, cancellationTokenSource);
        Assert.That(_character1.StatSystem.StatusEffects.Count, Is.EqualTo(1));
        
        // When AdvanceCharacterTurnIsCalledTwice
        int iterations = 0;
        while (iterations < 2)
        {
            _character1.AdvanceCharacterTurn();
            iterations++;
        }

        //Then CharacterHasHadStatusEffectRemoved
        Assert.That(_character1.StatSystem.StatusEffects.Count, Is.EqualTo(0));
    }

    [Test]
    public async void Given_StatusEffectWithDurationOf2IsAdded_WhenAdvanceCharacterTurnIsCalledTwice_ThenCharacterHasHadStatusEffectTypeFlagSetBackToNone()
    {
        CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

        ServiceLocatorObject.Instance.CharacterModel.AddEnemyCharacters(new List<Character> { _character1 });

        StatusEffectType characterStatusEffectType = _character1.StatSystem.ActiveStatusEffectTypes;
        Assert.That(characterStatusEffectType, Is.Not.Null);
        Assert.That(characterStatusEffectType, Is.EqualTo(StatusEffectType.None));

        // Given StatusEffectWithDurationOf2IsAdded
        IStatusEffect testStatusEffect = ServiceLocatorObject.Instance.StatusEffectFactory.CreateStatusEffectFromBlueprint(_testStatusEffectBlueprint, null);
        Assert.That(testStatusEffect, Is.Not.Null);
        Assert.That(_character1.StatSystem.StatusEffects.Count, Is.EqualTo(0));
        await testStatusEffect.Apply(_character1, _character2, cancellationTokenSource);
        Assert.That(_character1.StatSystem.StatusEffects.Count, Is.EqualTo(1));

        characterStatusEffectType = _character1.StatSystem.ActiveStatusEffectTypes;
        Assert.That(characterStatusEffectType, Is.Not.Null);
        Assert.That(characterStatusEffectType, Is.EqualTo(testStatusEffect.StatusEffectTypeWrapper.StatusEffectTypeFlag));

        // When AdvanceCharacterTurnIsCalledTwice
        int iterations = 0;
        while (iterations < 2)
        {
            _character1.AdvanceCharacterTurn();
            iterations++;
        }

        characterStatusEffectType = _character1.StatSystem.ActiveStatusEffectTypes;
        Assert.That(characterStatusEffectType, Is.Not.Null);
        Assert.That(characterStatusEffectType, Is.EqualTo(StatusEffectType.None));

        //Then CharacterHasHadStatusEffectRemoved
        Assert.That(_character1.StatSystem.StatusEffects.Count, Is.EqualTo(0));
    }

    [Test]
    public async void Given_StatusEffectWithDexMinusIsAdded_WhenAdvanceCharacterTurnIsCalledTwice_ThenCharacterHasHadDexSetBackToNormal()
    {
        CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

        ServiceLocatorObject.Instance.CharacterModel.AddEnemyCharacters(new List<Character> { _character1 });

        float startingCharacterDexterityValue = _character1.StatSystem.GetStat(Constants.Dexterity).Value;

        // Given StatusEffectWithDurationOf2IsAdded
        IStatusEffect testStatusEffect = ServiceLocatorObject.Instance.StatusEffectFactory.CreateStatusEffectFromBlueprint(_testStatusEffectBlueprint, null);
        Assert.That(testStatusEffect, Is.Not.Null);
        Assert.That(_character1.StatSystem.StatusEffects.Count, Is.EqualTo(0));
        await testStatusEffect.Apply(_character1, _character2, cancellationTokenSource);
        Assert.That(_character1.StatSystem.StatusEffects.Count, Is.EqualTo(1));
        float newCharacterDexterityValue = _character1.StatSystem.GetStat(Constants.Dexterity).Value;
        Assert.That(startingCharacterDexterityValue, Is.Not.EqualTo(newCharacterDexterityValue));

        // When AdvanceCharacterTurnIsCalledTwice
        int iterations = 0;
        while (iterations < 2)
        {
            _character1.AdvanceCharacterTurn();
            iterations++;
        }

        //Then CharacterHasHadStatusEffectRemoved and dex is back to original
        Assert.That(_character1.StatSystem.StatusEffects.Count, Is.EqualTo(0));
        newCharacterDexterityValue = _character1.StatSystem.GetStat(Constants.Dexterity).Value;
        Assert.That(newCharacterDexterityValue, Is.EqualTo(startingCharacterDexterityValue));
    }

    [Test]
    public async void Given_StatusEffectWithDexMinusIsAddedAndStatusEffectWithSpeedMinusIsAdded_WhenAdvanceCharacterTurnIsCalledTwice_ThenCharacterHasHadDexSetBackToNormalButSpeedIsStillReduced()
    {
        CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

        ServiceLocatorObject.Instance.CharacterModel.AddEnemyCharacters(new List<Character> { _character1 });

        float startingCharacterDexterityValue = _character1.StatSystem.GetStat(Constants.Dexterity).Value;
        float startingCharacterSpeedValue = _character1.StatSystem.GetStat(Constants.Speed).Value;

        // Given StatusEffectWithDurationOf2IsAdded
        IStatusEffect testStatusEffect = ServiceLocatorObject.Instance.StatusEffectFactory.CreateStatusEffectFromBlueprint(_testStatusEffectBlueprint, null);
        IStatusEffect testStatusEffect1 = ServiceLocatorObject.Instance.StatusEffectFactory.CreateStatusEffectFromBlueprint(_testStatusEffectBlueprint1, null);
        Assert.That(testStatusEffect, Is.Not.Null);
        Assert.That(testStatusEffect1, Is.Not.Null);
        Assert.That(_character1.StatSystem.StatusEffects.Count, Is.EqualTo(0));
        await testStatusEffect.Apply(_character1, _character2, cancellationTokenSource);
        await testStatusEffect1.Apply(_character1, _character2, cancellationTokenSource);
        Assert.That(_character1.StatSystem.StatusEffects.Count, Is.EqualTo(2));
        float newCharacterDexterityValue = _character1.StatSystem.GetStat(Constants.Dexterity).Value;
        float newCharacterSpeedValue = _character1.StatSystem.GetStat(Constants.Speed).Value;
        Assert.That(startingCharacterDexterityValue, Is.Not.EqualTo(newCharacterDexterityValue));
        Assert.That(startingCharacterSpeedValue, Is.Not.EqualTo(newCharacterSpeedValue));

        // When AdvanceCharacterTurnIsCalledTwice
        int iterations = 0;
        while (iterations < 2)
        {
            _character1.AdvanceCharacterTurn();
            iterations++;
        }

        //Then CharacterHasHadStatusEffectRemoved and dex is back to original
        Assert.That(_character1.StatSystem.StatusEffects.Count, Is.EqualTo(1));
        newCharacterDexterityValue = _character1.StatSystem.GetStat(Constants.Dexterity).Value;
        newCharacterSpeedValue = _character1.StatSystem.GetStat(Constants.Speed).Value;
        Assert.That(newCharacterDexterityValue, Is.EqualTo(startingCharacterDexterityValue));
        Assert.That(newCharacterSpeedValue, Is.Not.EqualTo(startingCharacterSpeedValue));
    }

    #endregion

    #region Protected Functions
    #endregion

    #region Private Functions
    #endregion

    #region Inner Classes
    #endregion
}
