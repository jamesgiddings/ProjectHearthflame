using GramophoneUtils.Items.Hotbars;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Skill", menuName = "Character Classes/Skills")]
public class Skill : Resource, IHotbarItem
{
	[SerializeField] private CharacterClass[] classRestrictions;
	[SerializeField] private Skill[] prerequisites;
	public CharacterClass[] ClassRestrictions => classRestrictions;
	public Skill[] Prerequisites => prerequisites;

	public void Use()
	{
		throw new System.NotImplementedException();
	}
}
