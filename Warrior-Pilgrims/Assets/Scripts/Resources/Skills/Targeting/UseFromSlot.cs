using GramophoneUtils.Characters;
using UnityEngine;

[CreateAssetMenu(fileName = "UseFromSlot", menuName = "Skills/Targeting/UseFromSlot")]
public class UseFromSlot : ScriptableObject, IUseFromSlot
{
    #region Attributes/Fields/Properties

    [SerializeField] private bool _slot1 = false;
    public bool Slot1 => _slot1;

    [SerializeField] private bool _slot2 = false;
    public bool Slot2 => _slot2;

    [SerializeField] private bool _slot3 = false;
    public bool Slot3 => _slot3;

    [SerializeField] private bool _slot4 = false;
    public bool Slot4 => _slot4;

    #endregion

    #region Public Functions

    public bool CanUseFromSlot(ICharacter character)
    {
        int slotIndex = ServiceLocator.Instance.ServiceLocatorObject.CharacterModel.PlayerCharacterOrder.GetSlotIndexByCharacter(character);
        switch (slotIndex)
	    {
            case 0:
                return _slot1 ? true : false;
            case 1:
                return _slot2 ? true : false;
            case 2:
                return _slot3 ? true : false;
            case 3:
                return _slot4 ? true : false;
		    default:
                return false;
	    }
    }

    #endregion
}
