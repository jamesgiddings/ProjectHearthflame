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
			Debug.Log("about to check if Null");
			if (itemSlot.item as EquipmentItem != null)
			{
				IEquippable equippable = (EquipmentItem)itemSlot.item;
				equippable.Unequip(statSystemBehaviour.StatSystem);
				Debug.Log("not Null");
			}
		}
	}
}