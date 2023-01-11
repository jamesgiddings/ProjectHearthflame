using UnityEngine;

[CreateAssetMenu(fileName = "Next Round Command", menuName = "Commands/Battle/Next Round Command")]
public class NextRoundCommand : BattleStateCommand
{
    #region Attributes/Fields/Properties
    #endregion

    #region Constructors
    #endregion

    #region Callbacks
    #endregion

    #region Public Functions

    public override void Execute()
    {
        BattleDataModel.NextRound();
    }

    #endregion

    #region Protected Functions
    #endregion

    #region Private Functions
    #endregion

    #region Inner Classes
    #endregion

}
