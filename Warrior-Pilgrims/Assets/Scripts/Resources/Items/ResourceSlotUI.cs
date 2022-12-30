using GramophoneUtils.Events.CustomEvents;
using GramophoneUtils.Items.Hotbars;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace GramophoneUtils.Items.Containers
{
    public abstract class ResourceSlotUI : MonoBehaviour, IDropHandler
    {
        [SerializeField] protected Image resourceIconImage = null;
        [SerializeField] protected ResourceEvent onMouseRightClickResource = null;

        public int SlotIndex { get; protected set; }

        public virtual Resource SlotResource { get; set; }

        private void OnEnable() => UpdateSlotUI();

        protected virtual void Start()
        {
            SlotIndex = transform.GetSiblingIndex();
            UpdateSlotUI();
        }

        public abstract void OnDrop(PointerEventData eventData);

        public abstract void UpdateSlotUI();

        protected virtual void EnableSlotUI(bool enable) => resourceIconImage.enabled = enable;

		#region SavingLoading

		[Serializable]
        public struct ResourceSlotSaveData
        {
            public string resourceUID;
            public int resourceQuantity;

            public ResourceSlotSaveData(string resourceUID, int resourceQuantity)
            {
                this.resourceUID = resourceUID;
                this.resourceQuantity = resourceQuantity;
            }
        }

        #endregion
    }
}
