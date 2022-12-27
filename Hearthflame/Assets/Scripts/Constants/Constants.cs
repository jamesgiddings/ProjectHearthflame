using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Constants Object", menuName = "Constants/Global Constants")]
public class Constants : ScriptableObject
{
    [BoxGroup("Characters")]
    [SerializeField] public int NumberOfFrontCharacters = 3;
    [BoxGroup("Characters")]
    [SerializeField] public int NumberOfRearCharacters = 3;

    [BoxGroup("Battle")]
    [BoxGroup("Battle/Floating Text")]
    [SerializeField] public string MagicEvasionText = "Resist!";
    [BoxGroup("Battle/Floating Text")]
    [SerializeField] public string PhysicalEvasionText = "Miss!";
}
