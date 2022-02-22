using GramophoneUtils.Stats;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Database", menuName = "DatabaseForTest/testDatabase")]
public class TestDatabase : ScriptableObject
{
	[SerializeField] private List<CharacterTemplate> characterTemplates;// = new List<CharacterTemplate>();

	public List<CharacterTemplate> CharacterTemplates 
	{
		get 
		{ 
			if (characterTemplates != null)
			return characterTemplates;
			else
			{
				characterTemplates = new List<CharacterTemplate>();
				return characterTemplates;
			}
		}
	} 
}
