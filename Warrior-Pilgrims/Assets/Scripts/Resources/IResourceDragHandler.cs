using GramophoneUtils.Items.Containers;
using GramophoneUtils.Items;
using UnityEngine;
using UnityEngine.EventSystems;
using GramophoneUtils.Events.CustomEvents;

/// <summary>
/// //TODO, consolidate this with the ihover and inventory slot system
/// For now, this just allows the display of the tooltip
/// </summary>
public class IResourceDragHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    //[SerializeField] protected ResourceSlotUI resourceSlotUI = null;
    //[SerializeField] protected ResourceEvent onMouseStartHoverResource = null;
    [SerializeField] protected IResourceEvent onMouseStartHoverIResource = null;
    [SerializeField] protected VoidEvent onMouseEndHoverResource = null;
    //[SerializeField] protected ResourceEvent onMouseRightClickResource = null;
    [SerializeField] protected IResourceEvent onMouseRightClickIResource = null;

/*    protected CanvasGroup canvasGroup = null;
    protected GameObject dragDropCanvas;
    protected Transform originalParent = null;*/
    protected bool isHovering = false;



    private IResource _resource;

    public void Initialise(IResource resource)
    {
        _resource = resource;
    }

    private void OnDrawGizmos()
    {
        BoxCollider2D collider = GetComponent<BoxCollider2D>();
        Gizmos.DrawWireCube(collider.bounds.center, collider.bounds.size);
    }

    //public ResourceSlotUI ResourceSlotUI => resourceSlotUI;

    /*    private void Start()
        {
            dragDropCanvas = GameObject.FindGameObjectWithTag("DragDropCanvas");
            canvasGroup = GetComponent<CanvasGroup>();
        }*/

    private void OnDisable()
    {
        if (isHovering)
        {
            onMouseEndHoverResource.Raise();
            isHovering = false;
        }
    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {
        /*if (eventData.button == PointerEventData.InputButton.Left)
        {
            onMouseEndHoverResource.Raise();

            originalParent = transform.parent;

            if (dragDropCanvas == null)
            {
                transform.SetParent(transform.parent.parent.parent.parent);
            }
            else
            {
                transform.SetParent(dragDropCanvas.transform);
            }

            canvasGroup.blocksRaycasts = false;
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (resourceSlotUI.SlotResource as Item != null && resourceSlotUI as InventorySlotUI != null && resourceSlotUI as EquipmentSlotUI == null)
            {
                Item item = resourceSlotUI.SlotResource as Item;
                InventorySlotUI inventorySlotUI = resourceSlotUI as InventorySlotUI;
                item.Use(inventorySlotUI.Character, inventorySlotUI);
            }
        }*/
    }

    public virtual void OnDrag(PointerEventData eventData)
    {
        /*if (eventData.button == PointerEventData.InputButton.Left)
        {
            transform.position = Input.mousePosition;
        }*/
    }

    public virtual void OnPointerUp(PointerEventData eventData)
    {
        /*if (eventData.button == PointerEventData.InputButton.Left)
        {
            transform.SetParent(originalParent);
            transform.localPosition = Vector3.zero;
            canvasGroup.blocksRaycasts = true;
        }*/
    }

    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        onMouseStartHoverIResource.Raise(_resource);
        isHovering = true;
    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
        onMouseEndHoverResource.Raise();
        isHovering = false;
    }
}
