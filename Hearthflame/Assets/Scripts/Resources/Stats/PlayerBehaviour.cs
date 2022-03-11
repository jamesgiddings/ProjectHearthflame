using System;
using UnityEngine;
using GramophoneUtils.SavingLoading;
using UnityEngine.Events;
using GramophoneUtils.Items.Containers;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

namespace GramophoneUtils.Stats
{
	public class PlayerBehaviour : MonoBehaviour, ISaveable
	{
		[SerializeField] public UnityEvent onStatsChanged;
		[SerializeField] public UnityEvent onInventoryItemsUpdated;

		private Inventory partyInventory;
		private int partyInventorySize;
		private int startingScrip;

		private string transitionTargetNameCache = "";

		[SerializeField] private CharacterTemplate[] playerCharacterTemplates = new CharacterTemplate[8];

		private List<Character> playerCharacters;

		public CharacterTemplate[] PlayerCharacterTemplates => playerCharacterTemplates; // getter

		public Inventory PartyInventory 
		{
			get
			{
				if (partyInventory != null) { return partyInventory;  };
				partyInventory = new Inventory(24, 10000, onInventoryItemsUpdated);
				return partyInventory;
			}
		}

		public List<Character> PlayerCharacters
		{
			get
			{
				if (playerCharacters != null) { return playerCharacters; }
				playerCharacters = InstanceCharacters();
				return playerCharacters;
			}
		}

		private void Start()
		{
			RegisterCharactersOnStatsChangedEvent();
		}

		private void RegisterCharactersOnStatsChangedEvent()
		{
			for (int i = 0; i < PlayerCharacters.Count; i++)
			{
				RegisterCharacterOnStatsChangedEvent(PlayerCharacters[i]);
			}
		}

		private void RegisterCharacterOnStatsChangedEvent(Character character)
		{
			foreach (var stat in character.StatSystem.Stats)
			{
				stat.Value.OnStatChanged += OnStatsChanged;
			}
		}

		private void UnregisterCharacterOnStatsChangedEvent(Character character)
		{
			foreach (var stat in character.StatSystem.Stats)
			{
				stat.Value.OnStatChanged -= OnStatsChanged;
			}
		}

		private void OnStatsChanged()
		{
			onStatsChanged.Invoke();
		}

		private void OnDestroy() // just deregister from the unity event
		{
			foreach (Character character in PlayerCharacters)
			{
				UnregisterCharacterOnStatsChangedEvent(character);
			}
		}

		public List<Character> InstanceCharacters()
		{
			playerCharacters = new List<Character>();
			for (int i = 0; i < playerCharacterTemplates.Length; i++)
			{
				if (playerCharacterTemplates[i] != null)
				{
					playerCharacters.Add(new Character(playerCharacterTemplates[i], PartyInventory));
					playerCharacters[i].IsPlayer = true;
					playerCharacters[i].IsRear = (i > 3) ? true : false;
				}
			}
			return playerCharacters;
		}

		#region SavingLoading
		public object CaptureState()
		{
			CharacterSaveData[] charactersSaveDatasCache = new CharacterSaveData[PlayerCharacters.Count];
			for (int i = 0; i < PlayerCharacters.Count; i++)
			{

				charactersSaveDatasCache[i] = new CharacterSaveData
				{
					// IsUnlocked

					IsUnlocked = PlayerCharacters[i].IsUnlocked,

					// IsRear

					IsRear = PlayerCharacters[i].IsRear,

					// LevelSystem

					Level = PlayerCharacters[i].LevelSystem.GetLevel(),
					Experience = PlayerCharacters[i].LevelSystem.GetExperience(),

					// StatSystem

					Dexterity = PlayerCharacters[i].StatSystem.GetBaseStatValue(PlayerCharacters[i].StatSystem.StatTypeStringRefDictionary["Dexterity"]),
					Magic = PlayerCharacters[i].StatSystem.GetBaseStatValue(PlayerCharacters[i].StatTypeStringRefDictionary["Magic"]),
					Resilience = PlayerCharacters[i].StatSystem.GetBaseStatValue(PlayerCharacters[i].StatTypeStringRefDictionary["Resilience"]),
					Speed = PlayerCharacters[i].StatSystem.GetBaseStatValue(PlayerCharacters[i].StatTypeStringRefDictionary["Speed"]),
					Strength = PlayerCharacters[i].StatSystem.GetBaseStatValue(PlayerCharacters[i].StatTypeStringRefDictionary["Strength"]),
					Wits = PlayerCharacters[i].StatSystem.GetBaseStatValue(PlayerCharacters[i].StatTypeStringRefDictionary["Wits"]),

					// HealthSystem

					CurrentHealth = PlayerCharacters[i].HealthSystem.CurrentHealth,
					MaxHealth = PlayerCharacters[i].HealthSystem.MaxHealth,

					// EquipmentInventory

					EquipmentInventorySaveData = PlayerCharacters[i].EquipmentInventory.CaptureState()
				};
			}
			return new SaveData
			{
				// Position

				positionX = gameObject.transform.position.x,
				positionY = gameObject.transform.position.y,

				// PartyCharacters

				charactersSaveData = charactersSaveDatasCache,

				// PartyInventory

				PartyInventorySaveData = PartyInventory.CaptureState()
			};
		}

		public void RestoreState(object state)
		{
			var saveData = (SaveData)state;

			// Position
			Debug.Log("We are resetting the player's position.");
			gameObject.transform.position = new Vector3(saveData.positionX, saveData.positionY);

			// PartyCharacters

			for (int i = 0; i < saveData.charactersSaveData.Length; i++)
			{
				// IsUnlocked

				PlayerCharacters[i].IsUnlocked = saveData.charactersSaveData[i].IsUnlocked;

				// IsRear

				PlayerCharacters[i].IsRear = saveData.charactersSaveData[i].IsRear;

				Character character = PlayerCharacters[i];

				// LevelSystem

				character.LevelSystem.SetLevel(saveData.charactersSaveData[i].Level);
				character.LevelSystem.SetExperience(saveData.charactersSaveData[i].Experience);

				character.LevelSystem.LevelSystemAnimated.UpdateLevelSystemAnimated();

				// StatSystem

				character.StatSystem.GetStat(character.StatSystem.StatTypeStringRefDictionary["Dexterity"]).UpdateBaseValue(saveData.charactersSaveData[i].Dexterity);
				character.StatSystem.GetStat(character.StatSystem.StatTypeStringRefDictionary["Magic"]).UpdateBaseValue(saveData.charactersSaveData[i].Magic);
				character.StatSystem.GetStat(character.StatSystem.StatTypeStringRefDictionary["Resilience"]).UpdateBaseValue(saveData.charactersSaveData[i].Resilience);
				character.StatSystem.GetStat(character.StatSystem.StatTypeStringRefDictionary["Speed"]).UpdateBaseValue(saveData.charactersSaveData[i].Speed);
				character.StatSystem.GetStat(character.StatSystem.StatTypeStringRefDictionary["Strength"]).UpdateBaseValue(saveData.charactersSaveData[i].Strength);
				character.StatSystem.GetStat(character.StatSystem.StatTypeStringRefDictionary["Wits"]).UpdateBaseValue(saveData.charactersSaveData[i].Wits);

				// HealthSystem

				character.HealthSystem.SetCurrentHealth(saveData.charactersSaveData[i].CurrentHealth);
				character.HealthSystem.SetMaxHealth(saveData.charactersSaveData[i].MaxHealth);

				// EquipmentInventory

				character.EquipmentInventory.RestoreState(saveData.charactersSaveData[i].EquipmentInventorySaveData);
			}

			// PartyInventory

			PartyInventory.RestoreState(saveData.PartyInventorySaveData);
		}

		[Serializable]
		public struct CharacterSaveData
		{
			// IsUnlocked
			public bool IsUnlocked;

			// IsRear

			public bool IsRear;

			// LevelSystem
			public int Level;
			public int Experience;

			// StatSystem
			public float Dexterity;
			public float Magic;
			public float Resilience;
			public float Speed;
			public float Strength;
			public float Wits;

			// HealthSystem
			public int CurrentHealth;
			public int MaxHealth;

			public object EquipmentInventorySaveData;
		}

		[Serializable]
		public struct SaveData
		{
			// Position

			public float positionX;
			public float positionY;

			public CharacterSaveData[] charactersSaveData;

			public object PartyInventorySaveData;
		}
		#endregion
	}
}

