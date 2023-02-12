using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Update Character Game Object Positions Task", menuName = "Tasks/Battle/Update Character Game Object Positions Task")]
public class UpdateCharacterGameObjectPositionsTask : BattleStateTask
{
    #region Attributes/Fields/Properties

    [SerializeField] int _delayInMilliseconds;

    #endregion

    #region Constructors
    #endregion

    #region Callbacks
    #endregion

    #region Public Functions

    public override async Task Execute()
    {
        await Task.Delay(_delayInMilliseconds);

        ServiceLocator.Instance.CharacterGameObjectManager.UpdatePlayerBattlers(); // TODO, this should be an event
        ServiceLocator.Instance.CharacterGameObjectManager.UpdateEnemyBattlers(); // TODO, this should be an event
        ServiceLocator.Instance.CharacterGameObjectManager.MovePlayerBattlersForward(); // TODO, this should be an event
        ServiceLocator.Instance.CharacterGameObjectManager.MoveEnemyBattlersForward(); // TODO, this should be an event
    }

    #endregion

    #region Protected Functions
    #endregion

    #region Private Functions
    #endregion

    #region Inner Classes
    #endregion

}
