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
using GramophoneUtils.Utilities;
using System.Threading.Tasks;

namespace GramophoneUtils.Stats
{

    /// <summary>
    /// This class provides references to the data model for characters (player and enemy)/
    /// </summary>
    [CreateAssetMenu(fileName = "Character Model", menuName = "Characters/Character Model")]
    public class CharacterModel : ScriptableObjectThatCanRunCoroutines, ICharacterModel, ISaveable
    {
        #region Attributes/Fields/Properties

        [SerializeField] private readonly IAnimationService _animationService;
        public IAnimationService AnimationService => _animationService;

        [SerializeField] public UnityEvent onStatsChanged;
        [SerializeField] public UnityEvent onInventoryItemsUpdated;
        [SerializeField] public VoidEvent OnCharacterModelPlayerCharacterOrderUpdated;
        [SerializeField] public VoidEvent OnCharacterModelEnemyCharacterOrderUpdated;

        private bool _unhandledCharacterDeath = false;
        [SerializeField] public CharacterEvent OnCharacterDeath;

        [SerializeField] private Inventory _partyInventory;
        [ShowInInspector] public Inventory PartyInventory => _partyInventory;

        [SerializeField] private Inventory _enemyInventory;
        [ShowInInspector] public Inventory EnemyInventory => _enemyInventory;

        [SerializeField] private Character[] _playerCharacterBlueprints = new Character[6];
        public Character[] PlayerCharacterBlueprints
        {
            get
            {
                return _playerCharacterBlueprints;
            }
        }

        private List<Character> _playerCharacters = null;
        private List<Character> _enemyCharacters = new List<Character>();
        private List<Character> _reserveEnemyCharacters = new List<Character>();
        private List<Character> _deadEnemyCharacters = new List<Character>();
        public List<Character> DeadEnemyCharacters => _deadEnemyCharacters;

        private List<Character> _deadPlayerCharacters = new List<Character>();
        public List<Character> DeadPlayerCharacters => _deadPlayerCharacters;

        private CharacterOrder _playerCharacterOrder = null;
        private CharacterOrder _enemyCharacterOrder = null;


        public CharacterOrder PlayerCharacterOrder
        {
            get
            {
                _playerCharacterOrder = new CharacterOrder(PlayerCharacters.ToArray());
                OnCharacterModelPlayerCharacterOrderUpdated?.Raise();
                return _playerCharacterOrder;
            }
            set
            {
                _playerCharacterOrder = value;
            }
        }

        public CharacterOrder EnemyCharacterOrder
        {
            get
            {
                return _enemyCharacterOrder;
            }
            set
            {
                _enemyCharacterOrder = value;
            }
        }

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
                return _playerCharacters;
            }
        }

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

        public List<Character> ReserveEnemyCharacters
        {
            get
            {
                return _reserveEnemyCharacters;
            }
        }

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

        public List<Character> DeadEnemyCharactersList
        {
            get
            {
                return _deadEnemyCharacters;
            }
        }

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

        private void OnEnable()
        {
            _playerCharacters = null;
            _enemyCharacters = new List<Character>();
            _reserveEnemyCharacters = new List<Character>();
            _deadEnemyCharacters = new List<Character>();
            _deadPlayerCharacters = new List<Character>();

            _playerCharacterOrder = null;
            _enemyCharacterOrder = null;
        }

        private void OnDestroy() // just deregister from the unity event
        {
            foreach (Character character in PlayerCharacters)
            {
                UnregisterCharacterOnStatsChangedEvent(character);
            }
        }

        #endregion

        #region Public Functions

        public async Task PerformAction()
        {
            // Do some initial processing

            // Wait for the animation to complete
            await _animationService.PlayAnimation(IAnimationService.AnimationType.Idle);
            throw new NotImplementedException();
            // Continue with the action after the animation is complete
            // Do some more processing
        }

        public List<Character> InstanceCharacters()
        {
            _playerCharacters = new List<Character>();

            if (PlayerCharacterBlueprints == null)
            {
                Debug.LogWarning("PlayerCharacterBlueprints was null.");
                return _playerCharacters;
            }

            for (int i = 0; i < PlayerCharacterBlueprints.Length; i++)
            {
                if (PlayerCharacterBlueprints[i] != null)
                {
                    _playerCharacters.Add(PlayerCharacterBlueprints[i].Instance());
                    _playerCharacters.Last().IsPlayer = true;
                    _playerCharacters.Last().PartyInventory = PartyInventory;
                }
            }

            RegisterCharactersOnStatsChangedEvent();

            return _playerCharacters;
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
            _unhandledCharacterDeath = true;
            if (character.IsPlayer)
            {
                AddPlayerToDeadPlayerCharactersList(character);
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
        /// Removes all dead players from the list.
        /// </summary>
        public void ClearDeadPlayerCharactersList()
        {
            _deadPlayerCharacters.Clear();
        }

        /// <summary>
        /// Resets the enemyCharacterOrder to null.
        /// To be called at the end of the battle.
        /// </summary>
        public void ResetEnemyCharacterOrder()
        {
            _enemyCharacterOrder = null;
        }

        /// <summary>
        /// Resets the playerCharacterOrder to null.
        /// To be called at the end of the battle.
        /// </summary>
        public void ResetPlayerCharacterOrder()
        {
            _playerCharacterOrder = null;
        }

        /// <summary>
        /// Gets the character order by the character parameter
        /// </summary>
        /// <param name="character"></param>
        /// <returns>
        /// Returns the CharacterOrder object if the character was found in one of the CharacterModel's
        /// CharacterOrder objects.
        /// </returns>
        public CharacterOrder GetCharacterOrderByCharacter(Character character)
        {
            // TODO test

            if (PlayerCharacterOrder.GetCharacters().Contains(character))
            {
                return PlayerCharacterOrder;
            }
            else if (EnemyCharacterOrder.GetCharacters().Contains(character))
            {
                return EnemyCharacterOrder;
            }
            return null;
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

        public async Task AwaitCharacterDeathSequence()
        {
            await CharacterDeathSequence();
        }

        #endregion

        #region Private Functions

        private async Task CharacterDeathSequence()
        {
            // Todo this is going through the full sequence even when things are not dead.

            EnemyCharacterOrder.MoveCharactersForwardIntoSpaces();

            float end = Time.time + 0.2f;
            while (Time.time < end)
            {
                await Task.Yield();
            }

            ServiceLocator.Instance.CharacterGameObjectManager.MoveEnemyBattlersForward();
            end = Time.time + 0.3f;
            while (Time.time < end)
            {
                await Task.Yield();
            }
            _reserveEnemyCharacters = _enemyCharacterOrder.AddCharactersAndReturnRemainder(_reserveEnemyCharacters);
            OnCharacterModelEnemyCharacterOrderUpdated?.Raise();
            _unhandledCharacterDeath = false;
        }

        private void ConnectInventories(Character character)
        {
            character.PartyInventory = this.PartyInventory;
            character.EquipmentInventory.ConnectToCharacter(character, PartyInventory);
        }

        private void RegisterCharactersOnStatsChangedEvent()
        {
            for (int i = 0; i < _playerCharacters.Count; i++)
            {
                RegisterCharacterOnStatsChangedEvent(_playerCharacters[i]);
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

