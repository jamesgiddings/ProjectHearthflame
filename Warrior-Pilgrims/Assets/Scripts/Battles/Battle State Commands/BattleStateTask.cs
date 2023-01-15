using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public abstract class BattleStateTask : ScriptableObject, ITask
{
    #region Attributes/Fields/Properties

    [SerializeField] protected BattleDataModel BattleDataModel;

    #endregion

    #region Constructors
    #endregion

    #region Callbacks
    #endregion

    #region Public Functions

    public abstract Task Execute();

    #endregion

    #region Protected Functions
    #endregion

    #region Private Functions
    #endregion

    #region Inner Classes
    #endregion

}
