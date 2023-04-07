using System.Threading;
using System.Threading.Tasks;

public interface IApplyable
{
    #region Attributes/Fields/Properties
    #endregion

    #region Constructors
    #endregion

    #region Callbacks
    #endregion

    #region Public Functions

    Task Apply(ICharacter target, ICharacter originator, CancellationTokenSource tokenSource);

    Task Remove(ICharacter target, ICharacter originator, CancellationTokenSource tokenSource);

    #endregion

    #region Protected Functions
    #endregion

    #region Private Functions
    #endregion

    #region Inner Classes
    #endregion
}
