using System;
using UnityEngine;
using GramophoneUtils.SavingLoading;
using UnityEngine.Events;
using GramophoneUtils.Items.Containers;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Character = GramophoneUtils.Characters.Character;
using System.Linq;
using GramophoneUtils.Events.CustomEvents;
using System.Collections;

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

#if !UNITY_EDITOR
        [ShowInInspector]
#endif
        public List<Character> PlayerCharacters
        {
            get
            {
                if (_playerCharacterOrder != null) { return _playerCharacterOrder.GetCharacters().ToList(); }
                _playerCharacters = InstanceCharacters();
                foreach (Character character in _playerCharacters)
                {

                    if (!character.IsUnlocked) // TODO, there might be a better place to handle Unlocked Status
                    {
                        continue;
                    }

                    ConnectInventories(character);
                }

                _playerCharacterOrder = new CharacterOrder(_playerCharacters.ToArray());
                OnCharacterModelPlayerCharacterOrderUpdated?.Raise();
                return _playerCharacters;
            }
        }

#if !UNITY_EDITOR
        [ShowInInspector]
#endif
        public List<Character> EnemyCharacters
        {
            get
            {
                if (_enemyCharacterOrder == null)
                {
                    _enemyCharacterOrder = new CharacterOrder(_enemyCharacters.ToArray());
                    OnCharacterModelEnemyCharacterOrderUpdated?.Raise();
                }
                return _enemyCharacterOrder.GetCharacters().ToList();
            }
        }

#if !UNITY_EDITOR
        [ShowInInspector]
#endif
        public List<Character> ReserveEnemyCharacters
        {
            get
            {
                return _reserveEnemyCharacters;
            }
        }

#if !UNITY_EDITOR
        [ShowInInspector]
#endif 
        public Character Slot1PlayerCharacter
        {
            get
            {
                if (_playerCharacterOrder == null)
                {
                    _playerCharacterOrder = new CharacterOrder(_playerCharacters == null ? PlayerCharacters.ToArray() : _playerCharacters.ToArray());
                    OnCharacterModelPlayerCharacterOrderUpdated?.Raise();
                }
                return _playerCharacterOrder.Slot1Character;
            }
        }

#if !UNITY_EDITOR
        [ShowInInspector]
#endif
        public Character Slot2PlayerCharacter
        {
            get
            {
                if (_playerCharacterOrder == null)
                {
                    _playerCharacterOrder = new CharacterOrder(_playerCharacters == null ? PlayerCharacters.ToArray() : _playerCharacters.ToArray());
                    OnCharacterModelPlayerCharacterOrderUpdated?.Raise();
                }
                return _playerCharacterOrder.Slot2Character;
            }
        }

#if !UNITY_EDITOR
        [ShowInInspector]
#endif
        public Character Slot3PlayerCharacter
        {
            get
            {
                if (_playerCharacterOrder == null)
                {
                    _playerCharacterOrder = new CharacterOrder(_playerCharacters == null ? PlayerCharacters.ToArray() : _playerCharacters.ToArray());
                    OnCharacterModelPlayerCharacterOrderUpdated?.Raise();
                }
                return _playerCharacterOrder.Slot3Character;
            }
        }

#if !UNITY_EDITOR
        [ShowInInspector]
#endif
        public Character Slot4PlayerCharacter
        {
            get
            {
                if (_playerCharacterOrder == null)
                {
                    _playerCharacterOrder = new CharacterOrder(_playerCharacters == null ? PlayerCharacters.ToArray() : _playerCharacters.ToArray());
                    OnCharacterModelPlayerCharacterOrderUpdated?.Raise();
                }
                return _playerCharacterOrder.Slot4Character;
            }
        }


#if !UNITY_EDITOR
        [ShowInInspector]
#endif 
        public Character Slot1EnemyCharacter
        {
            get
            {
                if (_enemyCharacterOrder == null)
                {
                    _enemyCharacterOrder = new CharacterOrder(_enemyCharacters.ToArray());
                    OnCharacterModelEnemyCharacterOrderUpdated?.Raise();
                }
                return _enemyCharacterOrder.Slot1Character;
            }
        }

#if !UNITY_EDITOR
        [ShowInInspector]
#endif
        public Character Slot2EnemyCharacter
        {
            get
            {
                if (_enemyCharacterOrder == null)
                {
                    _enemyCharacterOrder = new CharacterOrder(_enemyCharacters.ToArray());
                    OnCharacterModelEnemyCharacterOrderUpdated?.Raise();
                }
                return _enemyCharacterOrder.Slot2Character;
            }
        }

#if !UNITY_EDITOR
        [ShowInInspector]
#endif
        public Character Slot3EnemyCharacter
        {
            get
            {
                if (_enemyCharacterOrder == null)
                {
                    _enemyCharacterOrder = new CharacterOrder(_enemyCharacters.ToArray());
                    OnCharacterModelEnemyCharacterOrderUpdated?.Raise();
                }
                return _enemyCharacterOrder.Slot3Character;
            }
        }

#if !UNITY_EDITOR
        [ShowInInspector]
#endif
        public Character Slot4EnemyCharacter
        {
            get
            {
                if (_enemyCharacterOrder == null)
                {
                    _enemyCharacterOrder = new CharacterOrder(_enemyCharacters.ToArray());
                    OnCharacterModelEnemyCharacterOrderUpdated?.Raise();
                }
                return _enemyCharacterOrder.Slot4Character;
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
        public List<Character> AllCharacters
        {
            get
            {
                List<Character> allCharacters = new List<Character>();
                allCharacters.AddRange(PlayerCharacters);
                allCharacters.AddRange(EnemyCharacters);
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

        public List<Character> InstanceCharacters()
        {
            _playerCharacters = new List<Character>();

            for (int i = 0; i < _playerCharacterBlueprints.Length; i++)
            {
                if (_playerCharacterBlueprints[i] != null)
                {
                    _playerCharacters.Add(_playerCharacterBlueprints[i].Instance());
                    _playerCharacters.Last<Character>().IsPlayer = true;
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
            StartCoroutine(CharacterDeathCoroutine(character));
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

        public void AddPlayerToDeadPlayerCharactersList(Character character)
        {
            _deadPlayerCharacters.Add(character);
        }

        public void AddEnemyCharacters(List<Character> charactersToAdd)
        {
            if (_enemyCharacterOrder == null)
            {
                _enemyCharacterOrder = new CharacterOrder(_enemyCharacters.ToArray()); // make an empty enemyCharacterOrder
            }
            _reserveEnemyCharacters = _enemyCharacterOrder.AddCharactersAndReturnRemainder(charactersToAdd);
            OnCharacterModelEnemyCharacterOrderUpdated?.Raise();
        }

        public void ReplaceEnemyCharacters(List<Character> charactersToReplaceWith)
        {
            _enemyCharacterOrder = new CharacterOrder(charactersToReplaceWith.ToArray());
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
        private IEnumerator CharacterDeathCoroutine(Character character)
        {
            if (character.IsPlayer)
            {
                AddPlayerToDeadPlayerCharactersList(character);
                // todo dead player characters should be swapped to the rear? maybe the ui just gives the option for that
            }
            else
            {
                AddEnemyToDeadEnemyCharactersList(character);
                RemoveEnemyCharacter(character);
                yield return new WaitForSeconds(0.5f);
                _enemyCharacterOrder.MoveCharactersForwardIntoSpaces();
                ServiceLocator.Instance.CharacterGameObjectManager.MoveEnemyBattlersForward();
                yield return new WaitForSeconds(0.5f);
                _reserveEnemyCharacters = _enemyCharacterOrder.AddCharactersAndReturnRemainder(_reserveEnemyCharacters);
                OnCharacterModelEnemyCharacterOrderUpdated?.Raise();
            }
        }

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

