using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GramophoneUtils.Stats;

public class CharacterTurnSlotUI : CharacterSlotUI
{

	[SerializeField] private bool isCurrentCharacter = false;
	[SerializeField] protected Image currentCharacterImage;

    private CharacterInventory characterInventory;
    private Character character = null;

    public CharacterInventory CharacterInventory { get { return characterInventory; } set { characterInventory = value; } }
    public Action OnCurrentActorChanged;

    public override Resource SlotResource
    {
        get { return slotResource; }
        set { slotResource = value; UpdateSlotUI(); }
    }

    public Character Character
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
        get { return isCurrentCharacter; }
        set { isCurrentCharacter = value; currentCharacterImage.enabled = value; UpdateSlotUI(); }
    }

    public bool GetIsCurrentCharacter()
	{
        if (character == null)
		{
            return false;
		}
        else
        return character.IsCurrentActor;
      //  //if (characterInventory != null)
      //  //    Debug.Log("SlotIndex: " + SlotIndex + " is checking if its Character is the current Actor. It thinks that it's actor is " + characterInventory.GetSlotByIndex(SlotIndex).Character.Name + " and thinks it IsCurrentActor is " + characterInventory.GetSlotByIndex(SlotIndex).Character.IsCurrentActor);
      //  //    return characterInventory.GetSlotByIndex(SlotIndex).Character.IsCurrentActor;        
      //  if (characterInventory != null)
      //      Debug.Log("SlotIndex: " + SlotIndex + " is checking if its Character is the current Actor. It thinks that it's actor is " + character.Name + " and thinks it IsCurrentActor is " + character.IsCurrentActor);
      //      if(character.IsCurrentActor != null)
		    //{
      //          return character.IsCurrentActor;
      //      }
      //  return false;
            
    }


	public override void UpdateSlotUI()
	{
        if (SlotResource == null)
        {
            EnableSlotUI(false);
            return;
        }
        if (SlotResource.Icon != null)
        {
            resourceIconImage.sprite = SlotResource.Icon;
        }

        EnableSlotUI(true);
        onCharacterSlotChanged?.Invoke();

        currentCharacterImage.enabled = GetIsCurrentCharacter();
    }
}
