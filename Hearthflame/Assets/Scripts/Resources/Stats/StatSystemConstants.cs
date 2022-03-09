using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Stat System Constants", menuName = "Stat System/Stat System Constants")]
public class StatSystemConstants : ScriptableObject
{
	[SerializeField] public float BaseMeleeEvasion;
	[SerializeField] public float BaseMeleeAccuracy;	
	[SerializeField] public float BaseRangedEvasion;
	[SerializeField] public float BaseRangedAccuracy;
	[SerializeField] public float BaseMagicAccuracy;
	[SerializeField] public float BaseMagicEvasion;

	// Strength
	[SerializeField] public float StrengthMultiplier;

	// Magic
	[SerializeField] public float MagicMultiplier;

	// Dexterity
	[SerializeField] public float DexterityMultiplier;	
	
	// Speed
	[SerializeField] public float SpeedMultiplier;

	// Armour
	[SerializeField] public float BasePhysicalArmour;
	[SerializeField] public float BaseFireArmour;
	[SerializeField] public float BaseElectricityArmour;
}
