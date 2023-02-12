using System;

namespace GramophoneUtils.Stats
{
    [Flags]
    public enum StatusEffectType
    {
        None = 0,
        Burn = 1,
        Bleed = 2,
        Stun = 4
    }

    /// <summary>
    /// This is wrapper class for the enum so that it can have a 'source' which allows 
    /// 'remove all from source' behvaiour and can be removed upon elapsing by subscribing to an event
    /// </summary>
    [Serializable]
    public class StatusEffectTypeWrapper
    {
        #region Attributes/Fields/Properties

        private StatusEffectType _statusEffectTypeFlag;
        public StatusEffectType StatusEffectTypeFlag => _statusEffectTypeFlag;

        private object _source;
        public object Source => _source;

        private Guid _id;

        #endregion

        #region Constructors

        public StatusEffectTypeWrapper(object source = null)
        {
            _statusEffectTypeFlag = StatusEffectType.None;
            _source = source;
            _id = new Guid();
        }

        public StatusEffectTypeWrapper(StatusEffectType statusEffectTypeFlag, object source = null)
        {
            _statusEffectTypeFlag = statusEffectTypeFlag;
            _source = source;
            _id = new Guid();
        }

        public StatusEffectTypeWrapper(StatusEffectTypeWrapper statusEffectType, object source = null)
        {
            _statusEffectTypeFlag = statusEffectType.StatusEffectTypeFlag;
            _source = source;
            _id = new Guid();
        }

        #endregion

        #region Callbacks
        #endregion

        #region Public Functions

        public void ApplyStatusEffectType(StatusEffectType statusEffectTypeFlag)
        {
            _statusEffectTypeFlag |= statusEffectTypeFlag;
        }

        public void RemoveStatusEffectType(StatusEffectType statusEffectTypeFlag)
        {
            _statusEffectTypeFlag &= ~statusEffectTypeFlag;
        }

        public bool HasStatusEffectType(StatusEffectType statusEffectTypeFlag)
        {
            return (_statusEffectTypeFlag & statusEffectTypeFlag) == statusEffectTypeFlag;
        }

        public override bool Equals(object obj)
        {
            return obj is StatusEffectTypeWrapper wrapper &&
                   _id.Equals(wrapper._id);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(_id);
        }

        #endregion

        #region Protected Functions
        #endregion

        #region Private Functions
        #endregion

        #region Inner Classes
        #endregion
    }
}

