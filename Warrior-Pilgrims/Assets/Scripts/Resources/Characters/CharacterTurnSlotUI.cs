using System;
using UnityEngine;
using UnityEngine.UI;

public class CharacterTurnSlotUI : CharacterSlotUI
{

	[SerializeField] private bool isCurrentCharacter = false;
	[SerializeField] protected Image currentCharacterImage;

    private CharacterInventory characterInventory;
    private ICharacter character = null;

    public CharacterInventory CharacterInventory { get { return characterInventory; } set { characterInventory = value; } }
    public Action OnCurrentActorChanged;

    public override IResource SlotResource
    {
        get { return slotResource; }
        set { slotResource = value; UpdateSlotUI(); }
    }

    public ICharacter Character
	{
        get { return character; }
        set { character = value; UpdateSlotUI(); }
    }

    protected override void Start()
    {
        SlotIndex = transform.GetSiblingIndex();
    }

    private void OnDisable()
	{
        UnsubscribeFromOnCurrentActorChanged();
    }

	public void SubscribeToOnCurrentActorChanged(Action action)
	{
        OnCurrentActorChanged += UpdateSlotUI;
    }

    private void UnsubscribeFromOnCurrentActorChanged()
	{
        OnCurrentActorChanged -= UpdateSlotUI;
    }

	public bool IsCurrentCharacter
    {
        get 
        { 
            return isCurrentCharacter; 
        }
        set 
        { 
            isCurrentCharacter = value; 
            currentCharacterImage.enabled = value; 
            UpdateSlotUI(); 
        }
    }

    public bool GetIsCurrentCharacter()
	{
        if (character == null && slotResource == null)
		{
            return false;
		}
        else if ((ICharacter) slotResource != null)
        {
            ICharacter characterFromSlotResource = (ICharacter) slotResource;
            return characterFromSlotResource.IsCurrentActor;
        }
		{
            return character.IsCurrentActor;
        }
    }

    public bool GetIsCharacterDead()
    {
        if (character == null)
        {
            return false;
        }
        else
            return character.HealthSystem.IsDead;
    }

    public override void UpdateSlotUI()
	{
        if (SlotResource == null)
        {
            EnableSlotUI(false);
            return;
        }
        if (SlotResource.Sprite != null)
        {
            resourceIconImage.sprite = SlotResource.Sprite;
        }

        EnableSlotUI(true);
        onCharacterSlotChanged?.Invoke();

        currentCharacterImage.enabled = GetIsCurrentCharacter();

        if (GetIsCharacterDead())
        {
            if (SlotResource.Sprite != null)
            {
                resourceIconImage.GetComponent<Image>().color = new Color32(255, 255, 225, 100); // greys out the characters image
            }
        }
    }
}
