using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Character Death Coroutine Command", menuName = "Commands/Battle/Character Death Coroutine Command")]
public class CharacterDeathCoroutineCommand : BattleStateCommand
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
        ServiceLocator.Instance.CharacterModel.StartCharacterDeathCoroutine();
    }

    #endregion

    #region Protected Functions
    #endregion

    #region Private Functions
    #endregion

    #region Inner Classes
    #endregion

}
