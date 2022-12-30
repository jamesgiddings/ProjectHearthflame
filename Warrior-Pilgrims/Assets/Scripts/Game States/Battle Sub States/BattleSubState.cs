using UnityEngine;

public abstract class BattleSubState : State
{
	protected BattleManager BattleManager;

	public override void EnterState()
	{
		base.EnterState();
		BattleManager = ServiceLocator.Instance.BattleManager;
	}

	public override void ExitState()
	{
		base.ExitState();
		if (BattleManager != null && BattleManager.TargetManager != null)
		{
            BattleManager.TargetManager.ClearTargets(); // Todo, Hack - BattleInitialisationState exits before the BattleManager and TargetManager even exist. 
        }
	}
}
