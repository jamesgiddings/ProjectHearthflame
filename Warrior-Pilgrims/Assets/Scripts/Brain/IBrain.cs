using System.Collections.Generic;

public interface IBrain
{
    #region Attributes/Fields/Properties
    #endregion

    #region Constructors
    #endregion

    #region Callbacks
    #endregion

    #region Public Functions

    ISkill ChooseSkill(ICharacter character);

    List<ICharacter> ChooseTargets(ICharacter originator, ISkill skill);

    #endregion

    #region Protected Functions
    #endregion

    #region Private Functions
    #endregion

    #region Inner Classes
    #endregion
}
