using GramophoneUtils.Events.CustomEvents;
using GramophoneUtils.Items;
using GramophoneUtils.Items.Containers;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(CanvasGroup))]
public class SkillDragHandler : ResourceDragHandler
{
    //[SerializeField] protected SkillSlotUI skillSlot = null;
    //[SerializeField] protected SkillEvent onMouseStartHoverSkill = null;
    //[SerializeField] protected VoidEvent onMouseEndHoverSkill = null;


    //public SkillSlotUI SkillSlot => skillSlot;

    //private void Start() => canvasGroup = GetComponent<CanvasGroup>();

    //private void OnDisable()
    //{
    //    if (isHovering)
    //    {
    //        onMouseEndHoverSkill.Raise();
    //        isHovering = false;
    //    }
    //}

    //public override void OnPointerDown(PointerEventData eventData)
    //{
    //    if (eventData.button == PointerEventData.InputButton.Left)
    //    {
    //        onMouseEndHoverSkill.Raise();

    //        originalParent = transform.parent;

    //        transform.SetParent(transform.parent.parent);

    //        canvasGroup.blocksRaycasts = false;
    //    }
    //}

    //public override void OnDrag(PointerEventData eventData)
    //{
    //    if (eventData.button == PointerEventData.InputButton.Left)
    //    {
    //        transform.position = Input.mousePosition;
    //    }
    //}

    //public override void OnPointerUp(PointerEventData eventData)
    //{
    //    if (eventData.button == PointerEventData.InputButton.Left)
    //    {
    //        transform.SetParent(originalParent);
    //        transform.localPosition = Vector3.zero;
    //        canvasGroup.blocksRaycasts = true;
    //    }
    //}

    //public override void OnPointerEnter(PointerEventData eventData)
    //{
    //    onMouseStartHoverSkill.Raise(SkillSlot.SlotResource as Skill);
    //    isHovering = true;
    //}

    //public override void OnPointerExit(PointerEventData eventData)
    //{
    //    onMouseEndHoverSkill.Raise();
    //    isHovering = false;
    //}
}
