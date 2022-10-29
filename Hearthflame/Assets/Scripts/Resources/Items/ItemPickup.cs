using System;
using UnityEngine;

namespace GramophoneUtils.Items
{
    public class ItemPickup : StatefulTrigger
    {
        [SerializeField] private ItemSlot[] itemSlots;

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(transform.position, 1);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.tag == "Player")
            {
                TriggerAction();
            }            
        }

        protected override void TriggerAction()
        {
            var itemContainer = ServiceLocator.Instance.PlayerModel.PartyInventory;

            foreach (ItemSlot itemSlot in itemSlots)
            {
                if (itemContainer == null) { return; }

                if (itemContainer.AddItem(itemSlot).quantity == 0)
                {
                    this.gameObject.SetActive(false);
                }

                if (deactivateOnTrigger)
                {
                    this.gameObject.SetActive(false);
                }
            }
        }

        #region SavingLoading
        public override object CaptureState()
        {
            return new SaveData
            {
                IsActive = gameObject.activeInHierarchy
            };
        }

        public override void RestoreState(object state)
        {
            var saveData = (SaveData)state;
            gameObject.SetActive(saveData.IsActive);
        }

        [Serializable]
        public struct SaveData
        {
            public bool IsActive;
        }
        #endregion


    }

    	
}