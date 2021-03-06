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

		private bool isRear;
		private bool isPlayer;
		private bool isUnlocked;
		private bool isCurrentActor = false;

		private readonly Inventory partyInventory;

		private readonly CharacterTemplate characterTemplate;
		//private readonly PartyCharacterTemplate partyCharacterTemplate;

		private readonly Brain brain;

		private Queue<int> notificationQueue = new Queue<int>();
		
		public readonly Dictionary<string, IStatType> StatTypeStringRefDictionary;
		public string Name => name; //getter
		public StatSystem StatSystem => statSystem; //getter
		public HealthSystem HealthSystem => healthSystem; //getter
		public LevelSystem LevelSystem => levelSystem; //getter
		public CharacterClass CharacterClass => characterClass; //getter
		public SkillSystem SkillSystem => skillSystem; //getter
		public EquipmentInventory EquipmentInventory => equipmentInventory; //getter
		public bool IsPlayer { get { return isPlayer; } set { isPlayer = value; } }
		public bool IsRear { get { return isRear; } set { isRear = value; } }
		public bool IsUnlocked { get { return isUnlocked; } set { isUnlocked = value; } }
		public bool IsCurrentActor { get { return isCurrentActor; } }
		public Inventory PartyInventory => partyInventory; //getter
		public CharacterTemplate CharacterTemplate => characterTemplate; //getter
		//public PartyCharacterTemplate PartyCharacterTemplate => partyCharacterTemplate; //getter
		public Brain Brain => brain; //getter
		public Character() { } //constructor 1

		public Character(CharacterTemplate characterTemplate, Inventory partyInventory) //constructor 2
		{
			//this.partyCharacterTemplate = partyCharacterTemplate;
			this.characterTemplate = characterTemplate;
			name = characterTemplate.Name;
			statSystem = new StatSystem(characterTemplate, this);
			StatTypeStringRefDictionary = statSystem.StatTypeStringRefDictionary;
			characterClass = characterTemplate.CharacterClass;
			healthSystem = new HealthSystem(characterClass);

			healthSystem.OnHealthChanged += EnqueueBattlerNotification;

			// subscribe to OnDeathEvent here? also, inject a reference to Character if needed
			
			this.skillSystem = new SkillSystem(this);
			this.levelSystem = new LevelSystem(characterClass, this);
			levelSystem.OnLevelChanged += characterClass.LevelUp;
			equipmentInventory = new EquipmentInventory(this);
			equipmentInventory.onInventoryItemsUpdated = partyInventory.onInventoryItemsUpdated;
			this.partyInventory = partyInventory;

			this.brain = CharacterTemplate.Brain;
			this.IsUnlocked = CharacterTemplate.StartsUnlocked;
			skillSystem.Initialise();
			if (brain != null)
			{
				brain.Initialise(this);
			}
		}

		//public Character(PartyCharacterTemplate partyCharacterTemplate, Party party) //constructor 2
		//{
		//	this.partyCharacterTemplate = partyCharacterTemplate;
		//	this.characterTemplate = partyCharacterTemplate.Template;
		//	name = partyCharacterTemplate.Template.Name;
		//	statSystem = new StatSystem(partyCharacterTemplate.Template, this);
		//	StatTypeStringRefDictionary = statSystem.StatTypeStringRefDictionary;
		//	healthSystem = new HealthSystem(partyCharacterTemplate.Template);

		//	healthSystem.OnHealthChanged += EnqueueBattlerNotification;

		//	// subscribe to OnDeathEvent here? also, inject a reference to Character if needed
		//	characterClass = partyCharacterTemplate.Template.CharacterClass;
		//	this.skillSystem = new SkillSystem(partyCharacterTemplate, this);
		//	this.levelSystem = new LevelSystem(characterClass, this);
		//	levelSystem.OnLevelChanged += characterClass.LevelUp;
		//	equipmentInventory = new EquipmentInventory(this, party);
		//	equipmentInventory.onInventoryItemsUpdated = party.onInventoryItemsUpdated;
		//	isPlayer = partyCharacterTemplate.Template.IsPlayer;
		//	isRear = partyCharacterTemplate.IsRear;
		//	this.party = party;
		//	partyInventory = party.PartyInventory;

		//	this.brain = partyCharacterTemplate.Brain;

		//	skillSystem.Initialise();
		//	brain.Initialise(this);
		//}

		private void EnqueueBattlerNotification(int value)
		{
			notificationQueue.Enqueue(value);
		}

		public int DequeueBattlerNoticiation()
		{
			return notificationQueue.Dequeue();
		}

		public bool GetIsAnyNotificationInQueue()
		{
			if (notificationQueue.Count > 0)
			{
				return true;
			}
			return false;
		}

		public void SetIsCurrentActor(bool value)
		{
			isCurrentActor = value; 
		}

		public TargetAreaFlag GetTargetAreaFlag(bool IsOriginatorPlayer)
		{
			TargetAreaFlag characaterTargetAreaFlag = 0;
			switch (IsOriginatorPlayer)
			{
				case true:
					if (isPlayer && !isRear)
					{
						characaterTargetAreaFlag += (int)TargetAreaFlag.AllyFront;
					}
					else if (isPlayer && isRear)
					{
						characaterTargetAreaFlag += (int)TargetAreaFlag.AllyRear;
					}
					else if (!isPlayer && !isRear)
					{
						characaterTargetAreaFlag += (int)TargetAreaFlag.OpponentFront;
					}
					else if (!isPlayer && isRear)
					{
						characaterTargetAreaFlag += (int)TargetAreaFlag.OpponentRear;
					}
					break;
				case false:
					if (isPlayer && !isRear)
					{
						characaterTargetAreaFlag += (int)TargetAreaFlag.OpponentFront;
					}
					else if (isPlayer && isRear)
					{
						characaterTargetAreaFlag += (int)TargetAreaFlag.OpponentRear;
					}
					else if (!isPlayer && !isRear)
					{
						characaterTargetAreaFlag += (int)TargetAreaFlag.AllyFront;
					}
					else if (!isPlayer && isRear)
					{
						characaterTargetAreaFlag += (int)TargetAreaFlag.AllyRear;
					}
					break;
			}				
			return characaterTargetAreaFlag;
		}

		public TargetTypeFlag GetTargetTypeFlag()
		{
			if (healthSystem.IsDead)
			{
				return TargetTypeFlag.Dead;
			}
			else
			{
				return TargetTypeFlag.Alive;
			}
		}
	}
}

