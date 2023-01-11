using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UseFromSlot", menuName = "Character Classes/Skills/Targeting/UseFromSlot")]
public class UseFromSlot : ScriptableObject
{
    [SerializeField] private bool _slot1 = false;
    public bool Slot1 => _slot1;

    [SerializeField] private bool _slot2 = false;
    public bool Slot2 => _slot2;

    [SerializeField] private bool _slot3 = false;
    public bool Slot3 => _slot3;

    [SerializeField] private bool _slot4 = false;
    public bool Slot4 => _slot4;
}
