using GramophoneUtils.Items.Hotbars;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Skill", menuName = "Character Classes/Skills")]
public class Skill : Resource, IHotbarItem
{
	[SerializeField] private new string name;
	[SerializeField] private CharacterClass[] classRestrictions;
	public string Name => name; //getter
	public CharacterClass[] ClassRestrictions => classRestrictions;

	public Sprite Icon => throw new System.NotImplementedException();

	public void Use()
	{
		throw new System.NotImplementedException();
	}
}
