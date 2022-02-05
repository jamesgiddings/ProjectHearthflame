using GramophoneUtils.Items.Hotbars;
using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New Skill", menuName = "Character Classes/Skills")]
public class Skill : Resource, IHotbarItem
{
	[SerializeField] private CharacterClass[] classRestrictions;
	[SerializeField] private Skill[] prerequisites;
	[SerializeField] private TargetFlags targetFlag;

	private int targetFlagSum;

	public Action OnSkillUsed;
	public CharacterClass[] ClassRestrictions => classRestrictions;
	public Skill[] Prerequisites => prerequisites;

	protected override void OnValidate()
	{
		base.OnValidate();
		Debug.Log((int)targetFlag);
		SetTargetFlagSum();
		Debug.Log(targetFlagSum);
	}

	private void SetTargetFlagSum()
	{
		targetFlagSum = (int)targetFlag;
	}

	public void Use()
	{
		throw new System.NotImplementedException();
	}
}
