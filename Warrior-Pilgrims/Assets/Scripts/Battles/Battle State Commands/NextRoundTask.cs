using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Next Round Task", menuName = "Tasks/Battle/Next Round Task")]
public class NextRoundTask : BattleStateTask
{
    #region Attributes/Fields/Properties
    #endregion

    #region Constructors
    #endregion

    #region Callbacks
    #endregion

    #region Public Functions

    public override Task Execute()
    {
        BattleDataModel.NextRound();
        return Task.CompletedTask;
    }

    #endregion

    #region Protected Functions
    #endregion

    #region Private Functions
    #endregion

    #region Inner Classes
    #endregion

}
