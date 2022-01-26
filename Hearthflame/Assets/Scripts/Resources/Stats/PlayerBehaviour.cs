using System;
using UnityEngine;
using GramophoneUtils.SavingLoading;
using UnityEngine.Events;
using GramophoneUtils.Items.Containers;

namespace GramophoneUtils.Stats
{
	public class PlayerBehaviour : MonoBehaviour, ISaveable
	{
		[SerializeField] private CharacterTemplate playerTemplate;
		private Inventory partyInventory = new Inventory(20, 10000);

		[SerializeField] private PartyCharactersTemplateObject partyCharactersTemplateObject;

		public UnityEvent onStatsChanged;
		public UnityEvent onInventoryItemsUpdated;
		public Inventory PartyInventory => partyInventory; //getter

		private PartyCharacter[] partyCharacters;

		public PartyCharacter[] PartyCharacters
		{
			get
			{
				if (partyCharacters != null) { return partyCharacters; }
				partyCharacters = new PartyCharacter[partyCharactersTemplateObject.PartyCharacterTemplates.Length];
				for (int i = 0; i < partyCharactersTemplateObject.PartyCharacterTemplates.Length; i++)
				{
					PartyCharacterTemplate partyCharacterTemplate = partyCharactersTemplateObject.PartyCharacterTemplates[i];
					partyCharacters[i] = new PartyCharacter(partyCharacterTemplate, this);

					RegisterCharacterOnStatsChangedEvent(partyCharacters[i].Character);
				}
				return partyCharacters;
			}
		}

		private void Start()
		{
			partyInventory.onInventoryItemsUpdated = onInventoryItemsUpdated;
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
			foreach (PartyCharacter partyCharacter in partyCharacters)
			{
				UnregisterCharacterOnStatsChangedEvent(partyCharacter.Character);
			}
		}

		#region SavingLoading
		public object CaptureState()
		{
			PartyCharacterSaveData[] partyCharactersSaveDatasCache = new PartyCharacterSaveData[this.partyCharacters.Length];
			for (int i = 0; i < partyCharacters.Length; i++)
			{
				partyCharactersSaveDatasCache[i] = new PartyCharacterSaveData
				{
					// IsUnlocked

					IsUnlocked = partyCharacters[i].IsUnlocked,

					// LevelSystem

					Level = partyCharacters[i].Character.LevelSystem.GetLevel(),
					Experience = partyCharacters[i].Character.LevelSystem.GetExperience(),

					// StatSystem

					Dexterity = partyCharacters[i].Character.StatSystem.GetBaseStatValue(playerTemplate.StatTypeStringRefDictionary["Dexterity"]),
					Magic = partyCharacters[i].Character.StatSystem.GetBaseStatValue(playerTemplate.StatTypeStringRefDictionary["Magic"]),
					Resilience = partyCharacters[i].Character.StatSystem.GetBaseStatValue(playerTemplate.StatTypeStringRefDictionary["Resilience"]),
					Speed = partyCharacters[i].Character.StatSystem.GetBaseStatValue(playerTemplate.StatTypeStringRefDictionary["Speed"]),
					Strength = partyCharacters[i].Character.StatSystem.GetBaseStatValue(playerTemplate.StatTypeStringRefDictionary["Strength"]),
					Wits = partyCharacters[i].Character.StatSystem.GetBaseStatValue(playerTemplate.StatTypeStringRefDictionary["Wits"]),

					// HealthSystem

					CurrentHealth = partyCharacters[i].Character.HealthSystem.CurrentHealth,
					MaxHealth = partyCharacters[i].Character.HealthSystem.MaxHealth,

					// EquipmentInventory

					EquipmentInventorySaveData = partyCharacters[i].Character.EquipmentInventory.CaptureState()
				};
			}
			return new SaveData
			{
				// Position

				positionX = gameObject.transform.position.x,
				positionY = gameObject.transform.position.y,

				// PartyCharacters

				partyCharactersSaveData = partyCharactersSaveDatasCache,

				// PartyInventory

				PartyInventorySaveData = partyInventory.CaptureState()
			};
		}
		
		public void RestoreState(object state)
		{
			var saveData = (SaveData)state;

			// Position

			gameObject.transform.position = new Vector3(saveData.positionX, saveData.positionY);

			// PartyCharacters

			for (int i = 0; i < saveData.partyCharactersSaveData.Length; i++)
			{
				partyCharacters[i].IsUnlocked = saveData.partyCharactersSaveData[i].IsUnlocked;
				Character character = partyCharacters[i].Character;

				// LevelSystem

				character.LevelSystem.SetLevel(saveData.partyCharactersSaveData[i].Level);
				character.LevelSystem.SetExperience(saveData.partyCharactersSaveData[i].Experience);

				character.LevelSystem.LevelSystemAnimated.UpdateLevelSystemAnimated();
				  
				// StatSystem

				character.StatSystem.GetStat(playerTemplate.StatTypeStringRefDictionary["Dexterity"]).UpdateBaseValue(saveData.partyCharactersSaveData[i].Dexterity);
				character.StatSystem.GetStat(playerTemplate.StatTypeStringRefDictionary["Magic"]).UpdateBaseValue(saveData.partyCharactersSaveData[i].Magic);
				character.StatSystem.GetStat(playerTemplate.StatTypeStringRefDictionary["Resilience"]).UpdateBaseValue(saveData.partyCharactersSaveData[i].Resilience);
				character.StatSystem.GetStat(playerTemplate.StatTypeStringRefDictionary["Speed"]).UpdateBaseValue(saveData.partyCharactersSaveData[i].Speed);
				character.StatSystem.GetStat(playerTemplate.StatTypeStringRefDictionary["Strength"]).UpdateBaseValue(saveData.partyCharactersSaveData[i].Strength);
				character.StatSystem.GetStat(playerTemplate.StatTypeStringRefDictionary["Wits"]).UpdateBaseValue(saveData.partyCharactersSaveData[i].Wits);

				// HealthSystem

				character.HealthSystem.SetCurrentHealth(saveData.partyCharactersSaveData[i].CurrentHealth);
				character.HealthSystem.SetMaxHealth(saveData.partyCharactersSaveData[i].MaxHealth);

				// EquipmentInventory

				character.EquipmentInventory.RestoreState(saveData.partyCharactersSaveData[i].EquipmentInventorySaveData);
			}

			// PartyInventory

			partyInventory.RestoreState(saveData.PartyInventorySaveData);
		}
		[Serializable]
		public struct PartyCharacterSaveData
		{
			// IsUnlocked
			public bool IsUnlocked;
			
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

			public PartyCharacterSaveData[] partyCharactersSaveData;

			public object PartyInventorySaveData;
		}
		#endregion
	}
}

