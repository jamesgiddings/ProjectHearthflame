using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Calculate Next Actor Task", menuName = "Tasks/Battle/Calculate Next Actor Task")]
public class CalculateNextActorTask : BattleStateTask
{
    #region Attributes/Fields/Properties
    #endregion

    #region Constructors
    #endregion

    #region Callbacks
    #endregion

    #region Public Functions

    public override async Task Execute()
    {
        Task[] tasks = new Task[2];

        tasks[0] = BattleDataModel.UpdateCurrentActor();

        await Task.Delay(100);

        tasks[1] = BattleDataModel.UpdateState();

        await Task.WhenAll(tasks);
    }

    #endregion

    #region Protected Functions
    #endregion

    #region Private Functions
    #endregion

    #region Inner Classes
    #endregion

}
