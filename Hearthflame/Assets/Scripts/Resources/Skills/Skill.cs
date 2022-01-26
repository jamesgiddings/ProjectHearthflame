using GramophoneUtils.Items.Hotbars;
using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New Skill", menuName = "Character Classes/Skills")]
public class Skill : Resource, IHotbarItem
{
	[SerializeField] private CharacterClass[] classRestrictions;
	[SerializeField] private Skill[] prerequisites;

	public Action OnSkillUsed;
	public CharacterClass[] ClassRestrictions => classRestrictions;
	public Skill[] Prerequisites => prerequisites;

	public void Use()
	{
		throw new System.NotImplementedException();
	}
}
