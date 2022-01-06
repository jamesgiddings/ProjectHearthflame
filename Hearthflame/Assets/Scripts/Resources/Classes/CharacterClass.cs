using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Character Class", menuName = "Character Classes/Character Class")]
public class CharacterClass : Resource
{
	[SerializeField] private new string name;
	[SerializeField] private int maxLevel;

	public string Name => name; //getter
}
