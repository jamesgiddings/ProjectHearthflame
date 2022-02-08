using GramophoneUtils.Items.Containers;
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
		private readonly SkillSystem skillSystem;
		private readonly EquipmentInventory equipmentInventory;
		
		private bool isPlayer;
		private bool isCurrentActor = false;

		private readonly Party party;
		private readonly Inventory partyInventory;

		private readonly CharacterTemplate characterTemplate;
		private readonly PartyCharacterTemplate partyCharacterTemplate;
		
		public readonly Dictionary<string, IStatType> StatTypeStringRefDictionary;
		public string Name => name; //getter
		public StatSystem StatSystem => statSystem; //getter
		public HealthSystem HealthSystem => healthSystem; //getter
		public LevelSystem LevelSystem => levelSystem; //getter
		public CharacterClass CharacterClass => characterClass; //getter
		public SkillSystem SkillSystem => skillSystem; //getter
		public EquipmentInventory EquipmentInventory => equipmentInventory; //getter
		public bool IsPlayer { get { return isPlayer; } set { isPlayer = value; } }
		public bool IsCurrentActor { get { return isCurrentActor; } }
		public Party Party => party; //getter
		public Inventory PartyInventory => partyInventory; //getter
		public CharacterTemplate CharacterTemplate => characterTemplate; //getter
		public PartyCharacterTemplate PartyCharacterTemplate => partyCharacterTemplate; //getter
		public Character() { } //constructor 1
		public Character(PartyCharacterTemplate partyCharacterTemplate, Party party) //constructor 2
		{	
			this.partyCharacterTemplate = partyCharacterTemplate;
			this.characterTemplate = partyCharacterTemplate.Template;
			name = partyCharacterTemplate.Template.Name;
			StatTypeStringRefDictionary = partyCharacterTemplate.Template.StatTypeStringRefDictionary;
			statSystem = new StatSystem(partyCharacterTemplate.Template);
			healthSystem = new HealthSystem(partyCharacterTemplate.Template); 
			// subscribe to OnDeathEvent here? also, inject a reference to Ch aracter if needed
			characterClass = partyCharacterTemplate.Template.CharacterClass;
			this.skillSystem = new SkillSystem(partyCharacterTemplate, this);
			this.levelSystem = new LevelSystem(characterClass, this);
			levelSystem.OnLevelChanged += characterClass.LevelUp;
			equipmentInventory = new EquipmentInventory(this, party);
			equipmentInventory.onInventoryItemsUpdated = party.onInventoryItemsUpdated;
			isPlayer = partyCharacterTemplate.Template.IsPlayer;
			this.party = party;
			partyInventory = party.PartyInventory;

			skillSystem.Initialise();
		}

		public void SetIsCurrentActor(bool value)
		{
			isCurrentActor = value; 
		}
	}
}

