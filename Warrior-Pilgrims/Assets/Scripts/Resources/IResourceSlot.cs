using GramophoneUtils.Events.CustomEvents;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class IResourceSlot : MonoBehaviour
{
    #region Attributes/Fields/Properties

    [SerializeField] protected Image resourceIconImage = null;
    [SerializeField] protected ResourceEvent onMouseRightClickResource = null;

    public int SlotIndex { get; protected set; }

    public virtual IResource SlotResource { get; set; }

    private void OnEnable() => UpdateSlotUI();

    #endregion

    #region Constructors
    #endregion

    #region Callbacks
    protected virtual void Start()
    {
        SlotIndex = transform.GetSiblingIndex();
        UpdateSlotUI();
    }

    #endregion

    #region Public Functions
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

    #endregion

    #region Protected Functions
    #endregion

    #region Private Functions
    #endregion

    #region Inner Classes
    #endregion








}
