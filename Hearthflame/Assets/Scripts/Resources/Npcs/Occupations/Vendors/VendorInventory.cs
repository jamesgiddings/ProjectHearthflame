using GramophoneUtils.Items.Containers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Vendor Inventory", menuName = "NPCs/Vendor/Vendor Inventory")]
public class VendorInventory : Data
{
	[SerializeField] private Inventory inventory;

	public Inventory Inventory => inventory;
}
