using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GramophoneUtils.Stats
{
	[Serializable]
	public class Character
	{
		private readonly string name;
		
		private readonly StatSystem statSystem;
		private readonly HealthSystem healthSystem;
		private readonly LevelSystem levelSystem;
		private readonly CharacterClass characterClass;

		public string Name => name; //getter
		public StatSystem StatSystem => statSystem; //getter
		public HealthSystem HealthSystem => healthSystem; //getter
		public LevelSystem LevelSystem => levelSystem; //getter
		public CharacterClass CharacterClass => characterClass; //getter
		public Character() { } //constructor 1
		public Character(CharacterTemplate template) //constructor 2
		{
			name = template.name;
			statSystem = new StatSystem(template);
			healthSystem = new HealthSystem(template); 
			// subscribe to OnDeathEvent here? also, inject a reference to Ch aracter if needed
			characterClass = template.CharacterClass;
			levelSystem = new LevelSystem(characterClass, this);
			levelSystem.OnLevelChanged += characterClass.LevelUp;
		}
	}
}

