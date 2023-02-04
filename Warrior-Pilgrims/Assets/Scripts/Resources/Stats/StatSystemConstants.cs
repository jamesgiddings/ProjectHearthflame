using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Stat System Constants", menuName = "Constants/Stat System Constants")]
public class StatSystemConstants : ScriptableObject
{
	[SerializeField] public float BaseMeleeEvasion = 1f;
	[SerializeField] public float BaseMeleeAccuracy = 1f;	
	[SerializeField] public float BaseRangedEvasion = 1f;
	[SerializeField] public float BaseRangedAccuracy = 1f;
	[SerializeField] public float BaseMagicAccuracy = 1f;
	[SerializeField] public float BaseMagicEvasion = 1f;

	// Strength
	[SerializeField] public float StrengthMultiplier = 1f;

	// Magic
	[SerializeField] public float MagicMultiplier = 1f;

	// Dexterity
	[SerializeField] public float DexterityMultiplier = 1f;	
	
	// Speed
	[SerializeField] public float SpeedMultiplier = 1f;	
	
	// Resilience
	[SerializeField] public float ResilienceMultiplier = 1f;

	// Wits
	[SerializeField] public float WitsMultiplier = 1f;

	// Armour
	[SerializeField] public float BasePhysicalArmour = 1f;
	[SerializeField] public float BaseFireArmour = 1f;
	[SerializeField] public float BaseElectricityArmour = 1f;
}
