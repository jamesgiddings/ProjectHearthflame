using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GramophoneUtils.Stats;
using System;

namespace GramophoneUtils.Items.Containers
{
    public class EquipmentInventory : Inventory
    {
        [SerializeField] private StatSystemBehaviour statSystemBehaviour;
		public StatSystemBehaviour StatSystemBehaviour { get => statSystemBehaviour; }

		private void Start()
		{
			onInventoryItemsUpdated.AddListener(RefreshEquipmentInSlots);
		}		

		private void OnDestroy()
		{
			onInventoryItemsUpdated.RemoveListener(RefreshEquipmentInSlots);
		}

		private void EquipItemInSlot(ItemSlot itemSlot)
		{
			if (itemSlot.item as EquipmentItem != null)
			{
				IEquippable equippable = (EquipmentItem)itemSlot.item;
				equippable.Equip(statSystemBehaviour.StatSystem);
			}
		}

		private void UnequipItemFromSlot(ItemSlot itemSlot)
		{
			if (itemSlot.item as EquipmentItem != null)
			{
				IEquippable equippable = (EquipmentItem)itemSlot.item;
				equippable.Unequip(statSystemBehaviour.StatSystem);
			}
		}

		private void RefreshEquipmentInSlots()
		{
			Debug.Log("Refresh is called");
			foreach(ItemSlot itemSlot in ItemSlots)
			{
				IEquippable equippable = (EquipmentItem)itemSlot.item;
				if (equippable != null)
				{
					equippable.Unequip(statSystemBehaviour.StatSystem);
					equippable.Equip(statSystemBehaviour.StatSystem);
				}
			}
		}
	}
}