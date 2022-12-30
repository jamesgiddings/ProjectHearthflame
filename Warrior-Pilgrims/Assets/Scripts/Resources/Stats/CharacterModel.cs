using System;
using UnityEngine;
using GramophoneUtils.SavingLoading;
using UnityEngine.Events;
using GramophoneUtils.Items.Containers;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using GramophoneUtils.Characters;
using Sirenix.OdinInspector;
using UnityEngine.TextCore.Text;
using Character = GramophoneUtils.Characters.Character;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using GramophoneUtils.Events.CustomEvents;

namespace GramophoneUtils.Stats
{

    /// <summary>
    /// This class provides references to the data model for characters (player and enemy)/
    /// </summary>
    public class CharacterModel : MonoBehaviour, ISaveable
    {
        #region Attributes/Fields

        [SerializeField] public UnityEvent onStatsChanged;
        [SerializeField] public UnityEvent onInventoryItemsUpdated;
        [SerializeField] public VoidEvent OnCharacterModelPlayerCharacterOrderUpdated;
        [SerializeField] public VoidEvent OnCharacterModelEnemyCharacterOrderUpdated;
        [SerializeField] public CharacterEvent OnCharacterDeath;

        [SerializeField] private int _partyInventorySize = 20;
        [SerializeField] private int _startingScrip = 10000;

        private Inventory _partyInventory;
        private Inventory _enemyInventory;

        [SerializeField] private Character[] _playerCharacterBlueprints = new Character[6];

        private List<Character> _playerCharacters = null;
        private List<Character> _enemyCharacters = new List<Character>();
        private List<Character> _reserveEnemyCharacters = new List<Character>();
        private List<Character> _deadEnemyCharacters = new List<Character>();
        private List<Character> _deadPlayerCharacters = new List<Character>();

        private CharacterOrder _playerCharacterOrder = null;
        private CharacterOrder _enemyCharacterOrder = null;

        [ShowInInspector] public Inventory PartyInventory => _partyInventory;
        [ShowInInspector] public Inventory EnemyInventory => _enemyInventory;

        [ShowInInspector] public CharacterOrder PlayerCharacterOrder => _playerCharacterOrder;
        [ShowInInspector] public CharacterOrder EnemyCharacterOrder => _enemyCharacterOrder;

        public List<Character> PlayerCharacters
        {
            get
            {
                if (_playerCharacters != null) { return _playerCharacters; }
                _playerCharacters = InstanceCharacters();
                List<Character> _frontCharacters = new List<Character>();
                List<Character> _rearCharacters = new List<Character>();
                foreach (Character character in _playerCharacters)
                {

                    if (!character.IsUnlocked) // TODO, there might be a better place to handle Unlocked Status
                    {
                        continue;
                    }

                    ConnectInventories(character);
                    if (character.IsRear)
                    {
                        _rearCharacters.Add(character);
                    }
                    else
                    {
                        _frontCharacters.Add(character);
                    }
                }

                _playerCharacterOrder = new CharacterOrder(_frontCharacters, _rearCharacters);
                OnCharacterModelPlayerCharacterOrderUpdated?.Raise();
                return _playerCharacters;
            }
        }

        public List<Character> EnemyCharacters
        {
            get
            {
                return _enemyCharacters;
            }
        }

        [ShowInInspector] public Character[] FrontPlayerCharacters
        {
            get
            {
                if (_playerCharacterOrder == null)
                {
                    _playerCharacterOrder = new CharacterOrder(_playerCharacters == null ? PlayerCharacters.ToArray() : _playerCharacters.ToArray());
                    OnCharacterModelPlayerCharacterOrderUpdated?.Raise();
                }
                return _playerCharacterOrder.FrontCharacters;
            }
        }

        [ShowInInspector] public Character[] RearPlayerCharacters
        {
            get
            {
                if (_playerCharacterOrder == null)
                {
                    _playerCharacterOrder = new CharacterOrder(_playerCharacters == null ? PlayerCharacters.ToArray() : _playerCharacters.ToArray());
                    OnCharacterModelPlayerCharacterOrderUpdated?.Raise();
                }
                return _playerCharacterOrder.RearCharacters;
            }
        }

        [ShowInInspector] public Character[] FrontEnemyCharacters
        {
            get
            {
                if (_enemyCharacterOrder == null)
                {
                    _enemyCharacterOrder = new CharacterOrder(_enemyCharacters, new List<Character>());
                    OnCharacterModelEnemyCharacterOrderUpdated?.Raise();
                }
                return _enemyCharacterOrder.FrontCharacters;
            }
        }



#if !UNITY_EDITOR
        [ShowInInspector]
#endif
        public List<Character> FrontPlayerCharactersList
        {
            get
            {
                IEnumerable<Character> allNonNull = from nonNull in FrontPlayerCharacters where nonNull != null && nonNull.IsUnlocked select nonNull;
                return allNonNull.ToList();
            }
        }

#if !UNITY_EDITOR
        [ShowInInspector]
#endif
        public List<Character> RearPlayerCharactersList
        {
            get
            {
                IEnumerable<Character> allNonNull = from nonNull in RearPlayerCharacters where nonNull != null && nonNull.IsUnlocked select nonNull;
                return allNonNull.ToList();
            }
        }

#if !UNITY_EDITOR
        [ShowInInspector]
#endif
        public List<Character> FrontEnemyCharactersList
        {
            get
            {
                IEnumerable<Character> allNonNull = from nonNull in FrontEnemyCharacters where nonNull != null select nonNull;
                return allNonNull.ToList();
            }
        }

#if !UNITY_EDITOR
        [ShowInInspector]
#endif
        public List<Character> DeadEnemyCharactersList
        {
            get
            {
                return _deadEnemyCharacters;
            }
        }

#if !UNITY_EDITOR
        [ShowInInspector]
#endif
        public List<Character> AllFrontCharactersList
        {
            get
            {
                List<Character> allFrontCharacters = new List<Character>();
                allFrontCharacters.AddRange(FrontPlayerCharactersList);
                allFrontCharacters.AddRange(FrontEnemyCharactersList);
                return allFrontCharacters;
            }
        }
#if !UNITY_EDITOR
        [ShowInInspector]
#endif
        public List<Character> AllCharactersList
        {
            get
            {
                List<Character> allCharacters = new List<Character>();
                allCharacters.AddRange(AllFrontCharactersList);
                allCharacters.AddRange(RearPlayerCharactersList);
                IEnumerable<Character> allNonNull = from nonNull in FrontEnemyCharacters where nonNull != null select nonNull;
                return allCharacters;
            }
        }


#endregion

        #region Callbacks

        private void Awake()
        {
            Debug.Log(PlayerCharacters);
        }

        private void Start()
        {
            RegisterCharactersOnStatsChangedEvent();
        }


        private void OnDestroy() // just deregister from the unity event
        {
            foreach (Character character in PlayerCharacters)
            {
                UnregisterCharacterOnStatsChangedEvent(character);
            }
        }

        #endregion

        #region API

        public Character GetFrontPlayerCharacterByPosition(int position)
        {
            if (position < 0 || position > FrontPlayerCharacters.Length)
            {
                Debug.LogWarning("Position out of range");
                return null;
            }
            return FrontPlayerCharacters[position];
        }

        public Character GetRearPlayerCharacterByPosition(int position)
        {
            if (position < 0 || position > RearPlayerCharacters.Length)
            {
                Debug.LogWarning("Position out of range");
                return null;
            }
            return RearPlayerCharacters[position];
        }

        public Character GetFrontEnemyCharacterByPosition(int position)
        {
            if (position < 0 || position > FrontEnemyCharacters.Length)
            {
                Debug.LogWarning("Position out of range");
                return null;
            }
            return FrontEnemyCharacters[position];
        }

        public List<Character> InstanceCharacters()
        {
            _playerCharacters = new List<Character>();

            for (int i = 0; i < _playerCharacterBlueprints.Length; i++)
            {
                if (_playerCharacterBlueprints[i] != null)
                {
                    _playerCharacters.Add(_playerCharacterBlueprints[i].Instance());
                    _playerCharacters.Last<Character>().IsPlayer = true;
                    _playerCharacters.Last<Character>().IsRear = (i > 2) ? true : false;
                    _playerCharacters.Last<Character>().PartyInventory = GetPartyInventory();
                }
            }
            return _playerCharacters;
        }

        public Inventory GetPartyInventory()
        {
            if (_partyInventory == null)
            {
                _partyInventory = new Inventory(_partyInventorySize, _startingScrip, onInventoryItemsUpdated);
            }
            return _partyInventory;
        }

        public void RemoveEnemyCharacter(Character characterToRemove)
        {
            if (_enemyCharacterOrder == null)
            {
                return;
            }
            if (_enemyCharacterOrder.RemoveCharacter(characterToRemove))
            {
                OnCharacterModelEnemyCharacterOrderUpdated?.Raise();
            }
        }

        public void RemovePlayerCharacter(Character characterToRemove)
        {
            if (_playerCharacterOrder == null)
            {
                return;
            }
            if (_playerCharacterOrder.RemoveCharacter(characterToRemove))
            {
                OnCharacterModelPlayerCharacterOrderUpdated?.Raise();
            }
        }

        /// <summary>
        /// Adds the character to the appropriate dead characters list, and, if and enemy, 
        /// removes the enemy from the enemyCharacterOrder
        /// </summary>
        /// <param name="character"></param>
        public void RegisterCharacterDeath(Character character)
        {
            if(character.IsPlayer)
            {
                AddEnemyToDeadEnemyCharactersList(character);
                // todo dead player characters should be swapped to the rear? maybe the ui just gives the option for that
            }
            else
            {
                AddEnemyToDeadEnemyCharactersList(character);
                RemoveEnemyCharacter(character);
            }
        }

        /// <summary>
        /// Removes all dead enemies from the list.
        /// To be called at the end of the battle.
        /// </summary>
        public void ClearDeadEnemyCharactersList()
        {
            _deadEnemyCharacters.Clear();
        }

        /// <summary>
        /// Resets the enemyCharacterOrder to null.
        /// To be called at the end of the battle.
        /// </summary>
        public void ResetEnemyCharacterOrder()
        {
            _enemyCharacterOrder = null;
        }

        public void AddEnemyToDeadEnemyCharactersList(Character character)
        {
            _deadEnemyCharacters.Add(character);
        }

        public void AddEnemyToDeadPlayerCharactersList(Character character)
        {
            _deadPlayerCharacters.Add(character);
        }

        public void AddEnemyCharacters(List<Character> charactersToAdd)
        {
            if (_enemyCharacterOrder == null)
            {
                _enemyCharacterOrder = new CharacterOrder(_enemyCharacters, new List<Character>()); // make an empty enemyCharacterOrder
            }
            _reserveEnemyCharacters = _enemyCharacterOrder.AddCharactersToFrontAndReturnRemainder(charactersToAdd);
            OnCharacterModelEnemyCharacterOrderUpdated?.Raise();
        }

        public void ReplaceEnemyCharacters(List<Character> charactersToReplaceWith)
        {
            _enemyCharacterOrder = new CharacterOrder(charactersToReplaceWith, new List<Character>());
            OnCharacterModelEnemyCharacterOrderUpdated?.Raise();
        }

        public void UpdatePlayerCharacterOrder(CharacterOrder characterOrder)
        {
            if (_playerCharacterOrder.Equals(characterOrder)) { return; }
            _playerCharacterOrder = characterOrder;
            OnCharacterModelPlayerCharacterOrderUpdated?.Raise();
        }

        public void UpdateEnemyCharacterOrder(CharacterOrder characterOrder)
        {
            if (_enemyCharacterOrder.Equals(characterOrder)) { return; }
            _enemyCharacterOrder = characterOrder;
            OnCharacterModelEnemyCharacterOrderUpdated?.Raise();
        }

        #endregion 

        #region Utilities
        private void ConnectInventories(Character character)
        {
            character.PartyInventory = this._partyInventory;
            character.EquipmentInventory.ConnectToCharacter(character);
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

        #endregion

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

