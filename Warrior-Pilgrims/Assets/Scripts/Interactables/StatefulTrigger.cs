using GramophoneUtils.SavingLoading;
using UnityEngine;

[RequireComponent(typeof(SaveableEntity))]
public abstract class StatefulTrigger : Trigger, ISaveable
{
    public abstract object CaptureState();
    public abstract void RestoreState(object state);
}
