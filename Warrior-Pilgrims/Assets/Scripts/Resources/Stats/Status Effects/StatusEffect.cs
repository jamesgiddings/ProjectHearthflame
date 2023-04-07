using GramophoneUtils.Stats;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// This class groups together StatModifiers (flat effects which last the duration
/// of the StatusEffect), DoTs and HoTs (whose effects are applied based on where they are 
/// added to the TurnDamageEffects and TurnHealingEffects [][]s, and 
/// </summary>
public class StatusEffect : IStatusEffect
{
    #region Attributes/Fields/Properties


    private readonly string _uid;
    public string UID => _uid;


    private bool _isSubscribedToOnICharacterTurnAdvance = false;

    private ICharacter _originator;

    private CancellationTokenSource _cancellationTokenSource;


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


    private readonly string _name;
    public string Name => _name;


    public Sprite Sprite => Icon;

    #endregion

    #region Constructors

    public StatusEffect(
        StatusEffectType statusEffectTypeFlag,
        List<IStatModifier> statModifiers,
        Damage[][] turnDamageEffects,
        Healing[][] turnHealingEffects,
        Move[][] turnMoveEffects,        
        string tooltipText,
        string name,
        Sprite icon,
        bool damageMustLandForOtherEffectsToLand = false,
        int duration = -1,
        object source = null
        )
    {
        _uid = new Guid().ToString();

        _statusEffectTypeFlag = statusEffectTypeFlag;
        _statModifiers = CloneStatModifiers(statModifiers);

        _turnDamageEffects = CloneAndSanitizeDamageArray(turnDamageEffects, duration);
        _turnHealingEffects = CloneAndSanitizeHealingArray(turnHealingEffects, duration);
        _turnMoveEffects = CloneAndSanitizeMoveArray(turnMoveEffects, duration);

        _name = name;
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

    public string GetInfoDisplayText()
    {
        StringBuilder builder = new StringBuilder();
        builder.Append(ServiceLocatorObject.Instance.UIConstants.TOOLTIP_TITLE_SIZE_OPEN_TAG)
            .Append(Name)
            .Append(ServiceLocatorObject.Instance.UIConstants.TOOLTIP_SIZE_CLOSE_TAG)
            .AppendLine();
        
        if(_statusEffectTypeFlag != StatusEffectType.None)
        {
            builder.Append(GetStatusEffectTypeString())
            .AppendLine();
        }

        foreach (StatModifier statModifier in _statModifiers)
        {
            builder.Append(statModifier.GetInfoDisplayText())
                .AppendLine();
        }

        foreach (Damage[] damages in _turnDamageEffects)
        {
            foreach (Damage damage in damages)
            {
                builder.Append(damage.GetInfoDisplayText())
                    .Append(" per turn.")
                    .AppendLine();
            }
            break; // Only add the first iteration
        }

        foreach (Healing[] healings in _turnHealingEffects)
        {
            foreach (Healing healing in healings)
            {
                builder.Append(healing.GetInfoDisplayText())
                    .Append(" per turn.")
                    .AppendLine();
            }
            break; // Only add the first iteration
        }

        foreach (Move[] moves in _turnMoveEffects)
        {
            foreach (Move move in moves)
            {
                builder.Append(move.GetInfoDisplayText())
                    .Append(" per turn.")
                    .AppendLine();
            }
            break; // Only add the first iteration
        }

        builder.Append("Remaining Duration: " + _duration + " turns.")
            .AppendLine();

        builder.Append(ServiceLocatorObject.Instance.UIConstants.TOOLTIP_FLAVOUR_TEXT_OPEN_TAG)
            .Append(_tooltipText)
            .Append(ServiceLocatorObject.Instance.UIConstants.TOOLTIP_FLAVOUR_TEXT_CLOSE_TAG);
           

        return builder.ToString();
    }

    public void ElapseDuration()
    {
        OnDurationElapsed?.Invoke(this);
    }

    public async Task Apply(ICharacter target, ICharacter originator, CancellationTokenSource tokenSource)
    {
        _originator = originator;
        _cancellationTokenSource = tokenSource;

        if (tokenSource.Token.IsCancellationRequested) return;
        await SendStatusEffectType(target, originator, tokenSource);

        if (tokenSource.Token.IsCancellationRequested) return;
        await SendStatModifiers(target, originator, tokenSource);

        SubscribeToICharacterOnTurnElapsed(target);

        target.StatSystem.AddStatusEffectObject(this);
    }

    public Task Remove(ICharacter target, ICharacter originator, CancellationTokenSource tokenSource)
    {
        UnsubscribeFromICharacterOnTurnElapsed(target);

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
               _isSubscribedToOnICharacterTurnAdvance == effect._isSubscribedToOnICharacterTurnAdvance &&
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
        hash.Add(_isSubscribedToOnICharacterTurnAdvance);
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

    private string GetStatusEffectTypeString()
    {
        switch (_statusEffectTypeFlag)
        {
            case StatusEffectType.None:
                return null;
            case StatusEffectType.Burn:
                return ServiceLocatorObject.Instance.UIConstants.TOOLTIP_BURN_COLOUR_OPEN_TAG + "Burning" + ServiceLocatorObject.Instance.UIConstants.TOOLTIP_COLOUR_CLOSE_TAG;
            case StatusEffectType.Bleed:
                return ServiceLocatorObject.Instance.UIConstants.TOOLTIP_BLEED_COLOUR_OPEN_TAG + "Bleeding" + ServiceLocatorObject.Instance.UIConstants.TOOLTIP_COLOUR_CLOSE_TAG;
            case StatusEffectType.Stun:
                return ServiceLocatorObject.Instance.UIConstants.TOOLTIP_STUN_COLOUR_OPEN_TAG + "Stunned" + ServiceLocatorObject.Instance.UIConstants.TOOLTIP_COLOUR_CLOSE_TAG;
            default:
                return null;
        }
    }

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

    private Task SendStatModifiers(ICharacter target, ICharacter originator, CancellationTokenSource tokenSource)
    {
        if (tokenSource.Token.IsCancellationRequested) return Task.CompletedTask;
        List<IStatModifier> modifiedStatModifiers = ModifyStatModifiers(originator);
        target.StatSystem.ReceiveModifiedStatModifiers(modifiedStatModifiers, originator, tokenSource);
        return Task.CompletedTask;
    }

    private Task SendStatusEffectType(ICharacter target, ICharacter originator, CancellationTokenSource tokenSource)
    {
        if (tokenSource.Token.IsCancellationRequested) return Task.CompletedTask;
        target.StatSystem.ReceiveStatusEffectType(new StatusEffectTypeWrapper(_statusEffectTypeFlag, this), originator, tokenSource);
        return Task.CompletedTask;

    }

    private Task SendDamageStructs(ICharacter target, ICharacter originator, CancellationTokenSource tokenSource)
    {
        if (tokenSource.Token.IsCancellationRequested) return Task.CompletedTask;
        List<Damage> modifiedDamages = ModifyDamageStructs(originator);

        target.StatSystem.ReceiveModifiedDamageStructs(modifiedDamages, originator, tokenSource);
        return Task.CompletedTask;
    }

    private Task SendHealingStructs(ICharacter target, ICharacter originator, CancellationTokenSource tokenSource)
    {
        if (tokenSource.Token.IsCancellationRequested) return Task.CompletedTask;
        List<Healing> modifiedHealings = ModifyHealingStructs(originator);
        target.StatSystem.ReceiveModifiedHealingStructs(modifiedHealings, originator, tokenSource);
        return Task.CompletedTask;
    }

    private Task SendMoveStructs(ICharacter target, ICharacter originator, CancellationTokenSource tokenSource)
    {
        if (tokenSource.Token.IsCancellationRequested) return Task.CompletedTask;
        List<Move> modifiedStructs = ModifyMoveStructs(originator);
        target.StatSystem.ReceiveModifiedMoveStructs(modifiedStructs, originator, tokenSource);
        return Task.CompletedTask;
    }

    private List<IStatModifier> ModifyStatModifiers(ICharacter originator)
    {
        List<IStatModifier> modifiedStatModifiers = new List<IStatModifier>();

        foreach (IStatModifier statModifier in _statModifiers)
        {
            modifiedStatModifiers.Add(originator.StatSystem.ModifyOutgoinStatModifiers(statModifier, this));
        }

        return modifiedStatModifiers;
    }

    private List<Damage> ModifyDamageStructs(ICharacter originator)
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

    private List<Healing> ModifyHealingStructs(ICharacter originator)
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

    private List<Move> ModifyMoveStructs(ICharacter originator) // TODO we should probably add moves as well
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

    private void DecrementDuration(ICharacter target)
    {
        IncrementDuration();
    }

    private void SubscribeToICharacterOnTurnElapsed(ICharacter target)
    {
        if (!_isSubscribedToOnICharacterTurnAdvance)
        {
            target.OnCharacterTurnElapsed += SendDamageHealingAndMove;
            target.OnCharacterTurnElapsed += DecrementDuration;

            OnDurationElapsed += target.StatSystem.RemoveElapsible;
        }
        _isSubscribedToOnICharacterTurnAdvance = true;
    }

    private void UnsubscribeFromICharacterOnTurnElapsed(ICharacter target)
    {
        if (_isSubscribedToOnICharacterTurnAdvance)
        {
            target.OnCharacterTurnElapsed -= SendDamageHealingAndMove;
            target.OnCharacterTurnElapsed -= DecrementDuration;

            OnDurationElapsed -= target.StatSystem.RemoveElapsible;
        }
        _isSubscribedToOnICharacterTurnAdvance = false;
    }

    private async void SendDamageHealingAndMove(ICharacter target)
    {
        if (_cancellationTokenSource.Token.IsCancellationRequested) return;
        await SendDamageStructs(target, _originator, _cancellationTokenSource);

        if (_cancellationTokenSource.Token.IsCancellationRequested) return;
        await SendHealingStructs(target, _originator, _cancellationTokenSource);

        if (_cancellationTokenSource.Token.IsCancellationRequested) return;
        await SendMoveStructs(target, _originator, _cancellationTokenSource);
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
