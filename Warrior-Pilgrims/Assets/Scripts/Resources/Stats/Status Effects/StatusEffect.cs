using GramophoneUtils.Stats;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using Character = GramophoneUtils.Characters.Character;

/// <summary>
/// This class groups together StatModifiers (flat effects which last the duration
/// of the StatusEffect), DoTs and HoTs (whose effects are applied based on where they are 
/// added to the TurnDamageEffects and TurnHealingEffects [][]s, and 
/// </summary>
public class StatusEffect : IStatusEffect
{
    #region Attributes/Fields/Properties

    private bool _isSubscribedToOnCharacterTurnAdvance = false;


    private readonly StatusEffectType _statusEffectTypeFlag;

    private StatusEffectTypeWrapper _statusEffectTypeWrapper = null;
    public StatusEffectTypeWrapper StatusEffectTypeWrapper
    {
        get 
        {
            if (_statusEffectTypeWrapper == null)
            {
                _statusEffectTypeWrapper = new StatusEffectTypeWrapper(_statusEffectTypeFlag, _source);
            }
            return _statusEffectTypeWrapper; 
        }
    }

    private readonly List<IStatModifier> _statModifiers;
    public List<IStatModifier> StatModifiers => _statModifiers;


    private readonly Damage[][] _turnDamageEffects;
    public Damage[][] TurnDamageEffects => _turnDamageEffects;


    private readonly Healing[][] _turnHealingEffects;
    public Healing[][] TurnHealingEffects => _turnHealingEffects;


    private readonly Move[][] _turnMoveEffects;
    public Move[][] TurnMoveEffects => _turnMoveEffects;


    private readonly object _source = null;
    public object Source => _source;


    private int _duration;
    public int Duration => _duration;


    private readonly bool _damageMustLandForOtherEffectsToLand;
    public bool DamageMustLandForOtherEffectsToLand => _damageMustLandForOtherEffectsToLand;


    private readonly string _tooltipText;
    public string TooltipText => _tooltipText;


    private readonly Sprite _icon;
    public Sprite Icon => _icon;


    public Action<IElapsible> OnDurationElapsed { get; set; }

    #endregion

    #region Constructors

    public StatusEffect(
        StatusEffectType statusEffectTypeFlag,
        List<IStatModifier> statModifiers,
        Damage[][] turnDamageEffects,
        Healing[][] turnHealingEffects,
        Move[][] turnMoveEffects,
        string tooltipText,
        Sprite icon,
        bool damageMustLandForOtherEffectsToLand = false,
        int duration = -1,
        object source = null
        )
    {
        _statusEffectTypeFlag = statusEffectTypeFlag;
        _statModifiers = CloneStatModifiers(statModifiers);

        _turnDamageEffects = CloneAndSanitizeDamageArray(turnDamageEffects, duration);
        _turnHealingEffects = CloneAndSanitizeHealingArray(turnHealingEffects, duration);
        _turnMoveEffects = CloneAndSanitizeMoveArray(turnMoveEffects, duration);

        _tooltipText = tooltipText;
        _icon = icon;

        _source = source;
        _duration = duration;
        _damageMustLandForOtherEffectsToLand = damageMustLandForOtherEffectsToLand;
    }

    #endregion

    #region Callbacks
    #endregion

    #region Public Functions

    /// <summary>
    /// A negative value reduces the amount of time left until the IElapsible elapses.
    /// The default value, if no argument is passed, is -1.
    /// </summary>
    /// <param name="value"></param>
    public void IncrementDuration(int value = -1)
    {
        _duration += value;

        if (_duration <= 0)
        {
            ElapseDuration();
        }
    }

    public void ElapseDuration()
    {
        OnDurationElapsed?.Invoke(this);
    }

    public async Task Apply(Character target, Character originator, CancellationTokenSource tokenSource)
    {
        if (tokenSource.Token.IsCancellationRequested) return;
        await SendDamageStructs(target, originator, tokenSource);

        if (tokenSource.Token.IsCancellationRequested) return;
        await SendStatusEffectType(target, originator, tokenSource);

        if (tokenSource.Token.IsCancellationRequested) return;
        await SendStatModifiers(target, originator, tokenSource);

        if (tokenSource.Token.IsCancellationRequested) return;
        await SendHealingStructs(target, originator, tokenSource);
        
        if (tokenSource.Token.IsCancellationRequested) return;
        await SendMoveStructs(target, originator, tokenSource);
        
        SubscribeToCharacterOnTurnElapsed(target);

        target.StatSystem.AddStatusEffectObject(this);
    }

    public Task Remove(Character target, Character originator, CancellationTokenSource tokenSource)
    {
        UnsubscribeFromCharacterOnTurnElapsed(target);

        target.StatSystem.RemoveStatusEffectTypeWrapper(StatusEffectTypeWrapper);

        foreach (KeyValuePair<IStatType, IStat> entry in target.StatSystem.Stats)
        {
            entry.Value.RemoveAllModifiersFromSources(new object[] { this });
        }

        target.StatSystem.RemoveStatusEffectObject(this);
        return Task.CompletedTask;
    }

    public override bool Equals(object obj)
    {// TODO the problem is possibly with both of comparing the private variables but it is also definitely comparing the StatusEffectTypeWrapper in the equality way.
        return obj is StatusEffect effect &&
               _isSubscribedToOnCharacterTurnAdvance == effect._isSubscribedToOnCharacterTurnAdvance &&
               _statusEffectTypeFlag == effect._statusEffectTypeFlag &&
               EqualityComparer<StatusEffectTypeWrapper>.Default.Equals(StatusEffectTypeWrapper, effect.StatusEffectTypeWrapper) &&
               EqualityComparer<List<IStatModifier>>.Default.Equals(_statModifiers, effect._statModifiers) &&
               EqualityComparer<List<IStatModifier>>.Default.Equals(StatModifiers, effect.StatModifiers) &&
               EqualityComparer<Damage[][]>.Default.Equals(_turnDamageEffects, effect._turnDamageEffects) &&
               EqualityComparer<Damage[][]>.Default.Equals(TurnDamageEffects, effect.TurnDamageEffects) &&
               EqualityComparer<Healing[][]>.Default.Equals(_turnHealingEffects, effect._turnHealingEffects) &&
               EqualityComparer<Healing[][]>.Default.Equals(TurnHealingEffects, effect.TurnHealingEffects) &&
               EqualityComparer<Move[][]>.Default.Equals(_turnMoveEffects, effect._turnMoveEffects) &&
               EqualityComparer<Move[][]>.Default.Equals(TurnMoveEffects, effect.TurnMoveEffects) &&
               EqualityComparer<object>.Default.Equals(_source, effect._source) &&
               EqualityComparer<object>.Default.Equals(Source, effect.Source) &&
               _duration == effect._duration &&
               Duration == effect.Duration &&
               _damageMustLandForOtherEffectsToLand == effect._damageMustLandForOtherEffectsToLand &&
               DamageMustLandForOtherEffectsToLand == effect.DamageMustLandForOtherEffectsToLand;
    }

    public override int GetHashCode()
    {
        HashCode hash = new HashCode();
        hash.Add(_isSubscribedToOnCharacterTurnAdvance);
        hash.Add(_statusEffectTypeFlag);
        hash.Add(StatusEffectTypeWrapper);
        hash.Add(_statModifiers);
        hash.Add(StatModifiers);
        hash.Add(_turnDamageEffects);
        hash.Add(TurnDamageEffects);
        hash.Add(_turnHealingEffects);
        hash.Add(TurnHealingEffects);
        hash.Add(_turnMoveEffects);
        hash.Add(TurnMoveEffects);
        hash.Add(_source);
        hash.Add(Source);
        hash.Add(_duration);
        hash.Add(Duration);
        hash.Add(_damageMustLandForOtherEffectsToLand);
        hash.Add(DamageMustLandForOtherEffectsToLand);
        hash.Add(OnDurationElapsed);
        return hash.ToHashCode();
    }

    #endregion

    #region Protected Functions
    #endregion

    #region Private Functions

    private List<IStatModifier> CloneStatModifiers(List<IStatModifier> statModifiers)
    {
        List<IStatModifier> clonedStatModifiers = new List<IStatModifier>();
        foreach (IStatModifier statModifier in statModifiers)
        {
            List<object> sourceObjects = new List<object>();
            sourceObjects.AddRange(statModifier.Sources);
            sourceObjects.Add(this);
            object[] objectArray = sourceObjects.ToArray();
             
            clonedStatModifiers.Add(ServiceLocatorObject.Instance.StatModifierFactory.CreateStatModifierFromInstance(statModifier, objectArray));
        }
        return clonedStatModifiers;
    }

    private Task SendStatModifiers(Character target, Character originator, CancellationTokenSource tokenSource)
    {
        if (tokenSource.Token.IsCancellationRequested) return Task.CompletedTask;
        List<IStatModifier> modifiedStatModifiers = ModifyStatModifiers(originator);
        target.StatSystem.ReceiveModifiedStatModifiers(modifiedStatModifiers, originator, tokenSource);
        return Task.CompletedTask;
    }

    private Task SendStatusEffectType(Character target, Character originator, CancellationTokenSource tokenSource)
    {
        if (tokenSource.Token.IsCancellationRequested) return Task.CompletedTask;
        target.StatSystem.ReceiveStatusEffectType(new StatusEffectTypeWrapper(_statusEffectTypeFlag, this), originator, tokenSource);
        return Task.CompletedTask;

    }

    private Task SendDamageStructs(Character target, Character originator, CancellationTokenSource tokenSource)
    {
        if (tokenSource.Token.IsCancellationRequested) return Task.CompletedTask;
        List<Damage> modifiedDamages = ModifyDamageStructs(originator);

        target.StatSystem.ReceiveModifiedDamageStructs(modifiedDamages, originator, tokenSource);
        return Task.CompletedTask;
    }

    private Task SendHealingStructs(Character target, Character originator, CancellationTokenSource tokenSource)
    {
        if (tokenSource.Token.IsCancellationRequested) return Task.CompletedTask;
        List<Healing> modifiedHealings = ModifyHealingStructs(originator);
        target.StatSystem.ReceiveModifiedHealingStructs(modifiedHealings, originator, tokenSource);
        return Task.CompletedTask;
    }

    private Task SendMoveStructs(Character target, Character originator, CancellationTokenSource tokenSource)
    {
        if (tokenSource.Token.IsCancellationRequested) return Task.CompletedTask;
        List<Move> modifiedStructs = ModifyMoveStructs(originator);
        target.StatSystem.ReceiveModifiedMoveStructs(modifiedStructs, originator, tokenSource);
        return Task.CompletedTask;
    }

    private List<IStatModifier> ModifyStatModifiers(Character originator)
    {
        List<IStatModifier> modifiedStatModifiers = new List<IStatModifier>();

        foreach (IStatModifier statModifier in _statModifiers)
        {
            modifiedStatModifiers.Add(originator.StatSystem.ModifyOutgoinStatModifiers(statModifier, this));
        }

        return modifiedStatModifiers;
    }

    private List<Damage> ModifyDamageStructs(Character originator)
    {
        List<Damage> modifiedDamages = new List<Damage>();

        for (int i = 0; i < _turnDamageEffects[_duration - 1].Length; i++)
        {
            if (i > _turnDamageEffects.Length - 1) { break; }
            if (_turnDamageEffects[i] != null)
            {
                modifiedDamages.Add(originator.StatSystem.ModifyOutgoingDamage(_turnDamageEffects[_duration - 1][i]));
            }
        }

        return modifiedDamages;
    }

    private List<Healing> ModifyHealingStructs(Character originator)
    {
        List<Healing> modifiedHealings = new List<Healing>();

        for (int i = 0; i < _turnHealingEffects[_duration - 1].Length; i++)
        {
            if (i > _turnHealingEffects.Length - 1) { break; }
            if (_turnHealingEffects[i] != null)
            {
                modifiedHealings.Add(originator.StatSystem.ModifyOutgoingHealing(_turnHealingEffects[_duration - 1][i]));
            }

        }
        return modifiedHealings;
    }

    private List<Move> ModifyMoveStructs(Character originator) // TODO we should probably add moves as well
    {
        List<Move> modifiedMoves = new List<Move>();

        for (int i = 0; i < _turnMoveEffects[_duration - 1].Length; i++)
        {
            if (i > _turnMoveEffects.Length - 1) { break; }
            if (_turnMoveEffects[i] != null)
            {
                modifiedMoves.Add(originator.StatSystem.ModifyOutgoingMove(_turnMoveEffects[_duration - 1][i]));
            }

        }
        return modifiedMoves;
    }

    private void DecrementDuration()
    {
        IncrementDuration();
    }

    private void SubscribeToCharacterOnTurnElapsed(Character target)
    {
        if (!_isSubscribedToOnCharacterTurnAdvance)
        {
            target.OnCharacterTurnElapsed += DecrementDuration;
            OnDurationElapsed += target.StatSystem.RemoveElapsible;
        }
        _isSubscribedToOnCharacterTurnAdvance = true;
    }

    private void UnsubscribeFromCharacterOnTurnElapsed(Character character)
    {
        if (_isSubscribedToOnCharacterTurnAdvance)
        {
            character.OnCharacterTurnElapsed -= DecrementDuration;
        }
        _isSubscribedToOnCharacterTurnAdvance = false;
    }

    private Damage[][] CloneAndSanitizeDamageArray(Damage[][] turnDamageEffects, int duration)
    {
        int endOfArray = turnDamageEffects.Length; ;

        Damage[][] tempTurnDamageEffects = new Damage[duration][];

        for (int i = 0; i < duration; i++)
        {
            if (i >= endOfArray)
            {
                tempTurnDamageEffects[i] = new Damage[0];
                continue;
            }

            int numberOfEffects = turnDamageEffects[i] == null ? 0 : turnDamageEffects[i].Length;

            tempTurnDamageEffects[i] = new Damage[numberOfEffects];

            for (int j = 0; j < numberOfEffects; j++)
            {
                if (turnDamageEffects[i] == null)
                {
                    tempTurnDamageEffects[i] = new Damage[0];
                }
                else if (turnDamageEffects[i][j] == null)
                {
                    tempTurnDamageEffects[i][j] = null;
                }
                else
                {
                    tempTurnDamageEffects[i][j] = turnDamageEffects[i][j];
                }
            }
        }
        return tempTurnDamageEffects;
    }

    private Healing[][] CloneAndSanitizeHealingArray(Healing[][] turnHealingEffects, int duration)
    {
        int endOfArray = turnHealingEffects.Length;

        Healing[][] tempTurnHealingEffects = new Healing[duration][];

        for (int i = 0; i < duration; i++)
        {
            if (i >= endOfArray)
            {
                tempTurnHealingEffects[i] = new Healing[0];
                continue;
            }

            int numberOfEffects = turnHealingEffects[i] == null ? 0 : turnHealingEffects[i].Length;

            tempTurnHealingEffects[i] = new Healing[numberOfEffects];

            for (int j = 0; j < numberOfEffects; j++)
            {
                if (turnHealingEffects[i] == null)
                {
                    tempTurnHealingEffects[i] = new Healing[0];
                }
                else if (turnHealingEffects[i][j] == null)
                {
                    tempTurnHealingEffects[i][j] = null;
                }
                else
                {
                    tempTurnHealingEffects[i][j] = turnHealingEffects[i][j];
                }
            }
        }

        return tempTurnHealingEffects;
    }

    private Move[][] CloneAndSanitizeMoveArray(Move[][] turnMoveEffects, int duration)
    {
        Move[][] tempTurnMoveEffects = new Move[duration][];

        int endOfArray = turnMoveEffects.Length;
        for (int i = 0; i < duration; i++)
        {
            if (i >= endOfArray)
            {
                tempTurnMoveEffects[i] = new Move[0];
                continue;
            }

            int numberOfEffects = turnMoveEffects[i] == null ? 0 : turnMoveEffects[i].Length;

            tempTurnMoveEffects[i] = new Move[numberOfEffects];

            for (int j = 0; j < numberOfEffects; j++)
            {
                if (turnMoveEffects[i] == null)
                {
                    tempTurnMoveEffects[i] = new Move[0];
                }
                else if (turnMoveEffects[i][j] == null)
                {
                    tempTurnMoveEffects[i][j] = null;
                }
                else
                {
                    tempTurnMoveEffects[i][j] = turnMoveEffects[i][j];
                }
            }
        }
        return tempTurnMoveEffects;
    }

    #endregion

    #region Inner Classes
    #endregion
}
