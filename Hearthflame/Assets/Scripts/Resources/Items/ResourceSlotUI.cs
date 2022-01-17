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

        public int SlotIndex { get; private set; }

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
        public struct ItemSlotSaveData
        {
            public string itemUID;
            public int itemQuantity;

            public ItemSlotSaveData(string itemUID, int itemQuantity)
            {
                this.itemUID = itemUID;
                this.itemQuantity = itemQuantity;
            }
        }

        #endregion
    }
}
