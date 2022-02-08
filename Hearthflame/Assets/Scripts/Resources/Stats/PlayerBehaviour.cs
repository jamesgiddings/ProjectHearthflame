using System;
using UnityEngine;
using GramophoneUtils.SavingLoading;
using UnityEngine.Events;
using GramophoneUtils.Items.Containers;

namespace GramophoneUtils.Stats
{
	public class PlayerBehaviour : MonoBehaviour, ISaveable
	{
		[SerializeField] private PartyTemplate partyTemplate;
		private Party party;

		public Party Party
		{
			get
			{
				if (party != null) { return party; }
				party = partyTemplate.CreateBlueprintInstance<Party>();
				return party;
			}
		}

		private void Start()
		{
			party.PartyInventory.onInventoryItemsUpdated = party.onInventoryItemsUpdated;

			RegisterCharactersOnStatsChangedEvent();
		}

		private void RegisterCharactersOnStatsChangedEvent()
		{
			for (int i = 0; i < party.PartyCharacters.Length; i++)
			{
				RegisterCharacterOnStatsChangedEvent(party.PartyCharacters[i].Character);
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
			party.onStatsChanged.Invoke();
		}

		private void OnDestroy() // just deregister from the unity event
		{
			foreach (PartyCharacter partyCharacter in Party.PartyCharacters)
			{
				UnregisterCharacterOnStatsChangedEvent(partyCharacter.Character);
			}
		}

		#region SavingLoading
		public object CaptureState()
		{
			Debug.Log("Player CaptureState()");
			PartyCharacterSaveData[] partyCharactersSaveDatasCache = new PartyCharacterSaveData[Party.PartyCharacters.Length];
			for (int i = 0; i < Party.PartyCharacters.Length; i++)
			{

				partyCharactersSaveDatasCache[i] = new PartyCharacterSaveData
				{
					// IsUnlocked

					IsUnlocked = Party.PartyCharacters[i].IsUnlocked,

					// IsRear

					IsRear = Party.PartyCharacters[i].IsRear,

					// LevelSystem

					Level = Party.PartyCharacters[i].Character.LevelSystem.GetLevel(),
					Experience = Party.PartyCharacters[i].Character.LevelSystem.GetExperience(),

					// StatSystem

					Dexterity = Party.PartyCharacters[i].Character.StatSystem.GetBaseStatValue(Party.PartyCharacters[i].Character.CharacterTemplate.StatTypeStringRefDictionary["Dexterity"]),
					Magic = Party.PartyCharacters[i].Character.StatSystem.GetBaseStatValue(Party.PartyCharacters[i].Character.StatTypeStringRefDictionary["Magic"]),
					Resilience = Party.PartyCharacters[i].Character.StatSystem.GetBaseStatValue(Party.PartyCharacters[i].Character.StatTypeStringRefDictionary["Resilience"]),
					Speed = Party.PartyCharacters[i].Character.StatSystem.GetBaseStatValue(Party.PartyCharacters[i].Character.StatTypeStringRefDictionary["Speed"]),
					Strength = Party.PartyCharacters[i].Character.StatSystem.GetBaseStatValue(Party.PartyCharacters[i].Character.StatTypeStringRefDictionary["Strength"]),
					Wits = Party.PartyCharacters[i].Character.StatSystem.GetBaseStatValue(Party.PartyCharacters[i].Character.StatTypeStringRefDictionary["Wits"]),

					// HealthSystem

					CurrentHealth = Party.PartyCharacters[i].Character.HealthSystem.CurrentHealth,
					MaxHealth = Party.PartyCharacters[i].Character.HealthSystem.MaxHealth,

					// EquipmentInventory

					EquipmentInventorySaveData = Party.PartyCharacters[i].Character.EquipmentInventory.CaptureState()
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

				PartyInventorySaveData = Party.PartyInventory.CaptureState()
			};
		}
		
		public void RestoreState(object state)
		{
			var saveData = (SaveData)state;

			// Position
			Debug.Log("We are resetting the player's position.");
			gameObject.transform.position = new Vector3(saveData.positionX, saveData.positionY);

			// PartyCharacters

			for (int i = 0; i < saveData.partyCharactersSaveData.Length; i++)
			{
				// IsUnlocked
				
				Party.PartyCharacters[i].IsUnlocked = saveData.partyCharactersSaveData[i].IsUnlocked;

				// IsRear

				Party.PartyCharacters[i].IsRear = saveData.partyCharactersSaveData[i].IsRear;

				Character character = Party.PartyCharacters[i].Character;

				// LevelSystem

				character.LevelSystem.SetLevel(saveData.partyCharactersSaveData[i].Level);
				character.LevelSystem.SetExperience(saveData.partyCharactersSaveData[i].Experience);

				character.LevelSystem.LevelSystemAnimated.UpdateLevelSystemAnimated();
				  
				// StatSystem

				character.StatSystem.GetStat(character.CharacterTemplate.StatTypeStringRefDictionary["Dexterity"]).UpdateBaseValue(saveData.partyCharactersSaveData[i].Dexterity);
				character.StatSystem.GetStat(character.CharacterTemplate.StatTypeStringRefDictionary["Magic"]).UpdateBaseValue(saveData.partyCharactersSaveData[i].Magic);
				character.StatSystem.GetStat(character.CharacterTemplate.StatTypeStringRefDictionary["Resilience"]).UpdateBaseValue(saveData.partyCharactersSaveData[i].Resilience);
				character.StatSystem.GetStat(character.CharacterTemplate.StatTypeStringRefDictionary["Speed"]).UpdateBaseValue(saveData.partyCharactersSaveData[i].Speed);
				character.StatSystem.GetStat(character.CharacterTemplate.StatTypeStringRefDictionary["Strength"]).UpdateBaseValue(saveData.partyCharactersSaveData[i].Strength);
				character.StatSystem.GetStat(character.CharacterTemplate.StatTypeStringRefDictionary["Wits"]).UpdateBaseValue(saveData.partyCharactersSaveData[i].Wits);

				// HealthSystem

				character.HealthSystem.SetCurrentHealth(saveData.partyCharactersSaveData[i].CurrentHealth);
				character.HealthSystem.SetMaxHealth(saveData.partyCharactersSaveData[i].MaxHealth);

				// EquipmentInventory

				character.EquipmentInventory.RestoreState(saveData.partyCharactersSaveData[i].EquipmentInventorySaveData);
			}

			// PartyInventory

			Party.PartyInventory.RestoreState(saveData.PartyInventorySaveData);
		}

		[Serializable]
		public struct PartyCharacterSaveData
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

			public PartyCharacterSaveData[] partyCharactersSaveData;

			public object PartyInventorySaveData;
		}
		#endregion
	}
}

