using GramophoneUtils.Items;
using GramophoneUtils.Items.Containers;
using GramophoneUtils.Stats;
using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class CharacterSlotUI : ResourceSlotUI, IDropHandler
{

    [SerializeField] private bool isRear;

    protected Resource slotResource = null;

    public Action onCharacterSlotChanged;


    public override Resource SlotResource
    {
        get { return slotResource; }
        set {slotResource = value; UpdateSlotUI();}
    }

    public override void OnDrop(PointerEventData eventData)
    {
        ResourceDragHandler resourceDragHandler = eventData.pointerDrag.GetComponent<ResourceDragHandler>();

        if (resourceDragHandler == null) { return; }
        if (!(resourceDragHandler == null))
        {
            CharacterSlotUI characterSlotUI = resourceDragHandler.ResourceSlotUI as CharacterSlotUI;
            if (characterSlotUI != null)
            {
                Resource oldResource = SlotResource;
                SlotResource = characterSlotUI.SlotResource;
                characterSlotUI.SlotResource = oldResource;
                UpdateSlotUI();
                return;
            }
        }
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
            if (isRear)
			{
                CharacterTemplate character = SlotResource as CharacterTemplate;
			}
        }

        EnableSlotUI(true);
        onCharacterSlotChanged?.Invoke();
    }

    protected override void EnableSlotUI(bool enable)
    {
        base.EnableSlotUI(enable);
        //itemQuantityText.enabled = enable;
    }
}
