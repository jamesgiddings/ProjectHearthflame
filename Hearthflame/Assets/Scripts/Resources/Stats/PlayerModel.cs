using System;
using UnityEngine;
using GramophoneUtils.SavingLoading;
using UnityEngine.Events;
using GramophoneUtils.Items.Containers;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

namespace GramophoneUtils.Stats
{
    //
    // Summary:
    // This is the MonoBehvaiour which provides a reference to the data model for the player
    //

    public class PlayerModel : MonoBehaviour, ISaveable
    {
        [SerializeField] public UnityEvent onStatsChanged;
        [SerializeField] public UnityEvent onInventoryItemsUpdated;
        [SerializeField] private int _partyInventorySize = 20;
        [SerializeField] private int _startingScrip = 10000;

        private Inventory partyInventory;
        
        [SerializeField] private CharacterTemplate[] _characterTemplates = new CharacterTemplate[6];
        [SerializeField] private CharacterTemplate[] _rearCharacterTemplates = new CharacterTemplate[6]; // TODO hack while getting rear to work

        private List<Character> _playerCharacters;
        private List<Character> _rearCharacters;
       
        public CharacterTemplate[] CharacterTemplates => _characterTemplates; // getter
        public CharacterTemplate[] RearCharacterTemplates => _rearCharacterTemplates; // getter
        

        public Inventory PartyInventory
        {
            get
            {
                if (partyInventory != null) { return partyInventory; };
                partyInventory = new Inventory(_partyInventorySize, _startingScrip, onInventoryItemsUpdated);
                return partyInventory;
            }
        }

        public List<Character> PlayerCharacters
        {
            get
            {
                if (_playerCharacters != null) { return _playerCharacters; }
                _playerCharacters = InstanceCharacters();
                return _playerCharacters;
            }
        }

        public List<Character> RearCharacters
        {
            get
            {
                if (_rearCharacters != null) { return _rearCharacters; }
                _rearCharacters = InstanceRearCharacters();
                return _rearCharacters;
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
            _playerCharacters = new List<Character>();
            for (int i = 0; i < _characterTemplates.Length; i++)
            {
                if (_characterTemplates[i] != null)
                {
                    _playerCharacters.Add(new Character(_characterTemplates[i], PartyInventory));
                    _playerCharacters[i].IsPlayer = true;
                    /*playerCharacters[i].IsRear = (i > 3) ? true : false;*/
                }
            }
            return _playerCharacters;
        }

        public List<Character> InstanceRearCharacters()
        {
            _rearCharacters = new List<Character>();
            for (int i = 0; i < _rearCharacterTemplates.Length; i++)
            {
                if (_rearCharacterTemplates[i] != null)
                {
                    _rearCharacters.Add(new Character(_rearCharacterTemplates[i], PartyInventory));
                    _rearCharacters[i].IsPlayer = true;
                    _rearCharacters[i].IsRear = true;
                }
            }
            return _rearCharacters;
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
                // PartyCharacters

                charactersSaveData = charactersSaveDatasCache,

                // PartyInventory

                PartyInventorySaveData = PartyInventory.CaptureState()
            };
        }

        public void RestoreState(object state)
        {
            var saveData = (SaveData)state;

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

            public CharacterSaveData[] charactersSaveData;

            public object PartyInventorySaveData;
        }
        #endregion
    }
}

