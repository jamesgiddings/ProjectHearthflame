using GramophoneUtils.Events.CustomEvents;
using GramophoneUtils.Items.Containers;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GramophoneUtils.Items
{
    [RequireComponent(typeof(CanvasGroup))]
    public class ResourceDragHandler : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] protected ResourceSlotUI resourceSlotUI = null;
        [SerializeField] protected ResourceEvent onMouseStartHoverResource = null;
        [SerializeField] protected VoidEvent onMouseEndHoverResource = null;

        protected CanvasGroup canvasGroup = null;
        protected Transform originalParent = null;
        protected bool isHovering = false;

        public ResourceSlotUI ResourceSlotUI => resourceSlotUI;

        private void Start() => canvasGroup = GetComponent<CanvasGroup>();

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
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                onMouseEndHoverResource.Raise();

                originalParent = transform.parent;

                transform.SetParent(transform.parent.parent);

                canvasGroup.blocksRaycasts = false;
            }
        }

        public virtual void OnDrag(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                transform.position = Input.mousePosition;
            }
        }

        public virtual void OnPointerUp(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                transform.SetParent(originalParent);
                transform.localPosition = Vector3.zero;
                canvasGroup.blocksRaycasts = true;
            }
        }

        public virtual void OnPointerEnter(PointerEventData eventData)
        {
            onMouseStartHoverResource.Raise(ResourceSlotUI.SlotResource);
            isHovering = true;
        }

        public virtual void OnPointerExit(PointerEventData eventData)
        {
            onMouseEndHoverResource.Raise();
            isHovering = false;
        }
    }
}
