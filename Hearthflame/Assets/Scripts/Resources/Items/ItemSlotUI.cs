using GramophoneUtils.Items.Hotbars;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace GramophoneUtils.Items.Containers
{
    public abstract class ItemSlotUI : MonoBehaviour, IDropHandler
    {
        [SerializeField] protected Image itemIconImage = null;
        public int SlotIndex { get; private set; }

        public abstract Item SlotItem { get; set; }

        private void OnEnable() => UpdateSlotUI();

        protected virtual void Start()
        {
            SlotIndex = transform.GetSiblingIndex();
            UpdateSlotUI();
        }

        public abstract void OnDrop(PointerEventData eventData);

        public abstract void UpdateSlotUI();

        protected virtual void EnableSlotUI(bool enable) => itemIconImage.enabled = enable;

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
