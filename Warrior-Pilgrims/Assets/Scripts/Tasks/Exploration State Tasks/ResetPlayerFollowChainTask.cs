using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Reset Player Follow Chain Task", menuName = "Tasks/Reset Player Follow Chain Task")]
public class ResetPlayerFollowChainTask : ScriptableObject, ITask
{
    #region Attributes/Fields/Properties

    [SerializeField] int _delayInMilliseconds;

    #endregion

    #region Constructors
    #endregion

    #region Callbacks
    #endregion

    #region Public Functions
    public async Task Execute()
    {
        await Task.Delay(_delayInMilliseconds);
        ServiceLocator.Instance.CharacterGameObjectManager.ConnectPlayerCharactersToLeadCharacter();
    }

    #endregion

    #region Protected Functions
    #endregion

    #region Private Functions
    #endregion

    #region Inner Classes
    #endregion

}
