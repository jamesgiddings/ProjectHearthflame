using System;
using UnityEngine;
using GramophoneUtils.SavingLoading;
using System.Collections.Generic;
using UnityEngine.Rendering;
using GramophoneUtils.Characters;
using System.Linq;
using Sirenix.OdinInspector;
using DG.Tweening;
using System.Collections;

namespace GramophoneUtils.Stats
{
	public class CharacterGameObjectManager : MonoBehaviour, ISaveable
	{
        private BattleDataModel _battleDataModel;

        private Transform _battlerToFollow;

        private Transform _playerStartTransform = null;

        private Vector3 _savedPosition = Vector3.zero;

        private CharacterMovement _frontBattlerPosition = null;
        public CharacterMovement FrontBattlerPosition => _frontBattlerPosition;

        private Dictionary<Character, Battler> _characterBattlerDictionary = new Dictionary<Character, Battler>();
        public Dictionary<Character, Battler> CharacterBattlerDictionary => _characterBattlerDictionary;

        [SerializeField] private Transform _battlerParent;

        [SerializeField, Required] private FloatReference _battlerGapReference;
        private float _battlerGap => _battlerGapReference.Value;

        [SerializeField] private GameObject _frontBattlerPositionPrefab;
		
        #region Callbacks

        private void Awake()
        {
            _battleDataModel = ServiceLocator.Instance.BattleDataModel;
        }

        public void Update()
		{
			if (_battlerToFollow != null && ServiceLocator.Instance.GameStateManager.State == ServiceLocator.Instance.ExplorationState) // TODO hacky way to stop the camera following the first player in a battle
				// instead, the camera should follow something else during a battle
			{
                this.transform.position = _battlerToFollow.transform.position;
            }
        }

        #endregion

        #region API

        public void InstantiatePlayerCharacters()
        {
            CharacterOrder playerCharacterOrder = ServiceLocator.Instance.CharacterModel.PlayerCharacterOrder;

            this.GetComponent<SortingGroup>().sortingOrder = CharacterOrder.NumberOfSlots + 1;
            Vector3 instantiationPosition = _playerStartTransform == null ? _savedPosition == Vector3.zero ? this.transform.position : _savedPosition : _playerStartTransform.position;

            bool leaderSet = false;
            Character leader = null;
            for (int i = 0; i < CharacterOrder.NumberOfSlots; i++)
            {
                Character character = playerCharacterOrder.GetCharacterBySlotIndex(i);
                if (character != null && character.CharacterPrefab != null && !_characterBattlerDictionary.ContainsKey(character))
                {
                    Battler battler = Instantiate(character.CharacterPrefab, instantiationPosition + new Vector3(-_battlerGap * i, 0, 0), Quaternion.identity, _battlerParent).GetComponent<Battler>();
                    battler.Initialise(character);
                    
                    if (!leaderSet) // this is the first character, which will directly follow the playerBehaviour
                    {
                        battler.SetCharacterMovementIsLeader(this.GetComponent<SortingGroup>().sortingOrder = CharacterOrder.NumberOfSlots);
                        _battlerToFollow = battler.transform;
                        leader = character;
                        leaderSet = true;
                    }
                    else
                    {
                        battler.ConnectFollowerToLeader(_characterBattlerDictionary[leader].GetComponent<CharacterMovement>(), _battlerGap, i, CharacterOrder.NumberOfSlots - i); // the rest follow in a chain
                    }
                    Debug.Log(character.Name);
                    _characterBattlerDictionary.Add(character, battler);
                }
            }

            _playerStartTransform = null; // set this to null, as the override transform should only be used once (i.e. on loading into a scene or from a save)
        }

        public void InstantiateEnemyCharacters()
        {
            // Todo, this is being called outside of battle and I don't know why and loads of times in battle

            CharacterOrder enemyCharacterOrder = ServiceLocator.Instance.CharacterModel.EnemyCharacterOrder;

            if (enemyCharacterOrder == null) { return; }

            if (_frontBattlerPosition == null)
            {
                _frontBattlerPosition = Instantiate(_frontBattlerPositionPrefab, this.transform.position + new Vector3(_battlerGap * 1, 0), Quaternion.identity).GetComponent<CharacterMovement>();
            }

            for (int i = 0; i < CharacterOrder.NumberOfSlots; i++)
            {
                Character character = enemyCharacterOrder.GetCharacterBySlotIndex(i);
                if (character != null && character.CharacterPrefab != null && !_characterBattlerDictionary.ContainsKey(character))
                {
                    Battler battler = Instantiate(character.CharacterPrefab, _frontBattlerPosition.transform.position + new Vector3(_battlerGap * 2, 0) + new Vector3((_battlerGap * i), 0, 0), Quaternion.identity).GetComponent<Battler>();
                    battler.Initialise(character);

                    battler.ConnectFollowerToLeader(_frontBattlerPosition, _battlerGap, i, CharacterOrder.NumberOfSlots - i); // the rest follow in a chain
                    
                    if (character.CharacterClass.Size == 2)
                    {
                        i++;
                    }

                    _characterBattlerDictionary.Add(character, battler);
                }
            }
        }

        public void UpdateEnemyBattlers()
        {
            if (_characterBattlerDictionary.Count == 0) { return; }

            for (int i = _characterBattlerDictionary.Keys.Count - 1; i >= 0; i--)
            {
                Character character = _characterBattlerDictionary.ElementAt(i).Key;
                if (!ServiceLocator.Instance.CharacterModel.AllCharacters.Contains(character))
                {
                    _characterBattlerDictionary[character].Uninitialise();
                    Destroy(_characterBattlerDictionary[character].gameObject);
                    _characterBattlerDictionary.Remove(character);
                }
            }
            InstantiateEnemyCharacters();
        }

        public void ConnectPlayerCharactersToLeadCharacter()
        {
            CharacterOrder playerCharacterOrder = ServiceLocator.Instance.CharacterModel.PlayerCharacterOrder;

            bool leaderSet = false;
            Character leader = null;
            int j = 0;
            for (int i = 0; i < CharacterOrder.NumberOfSlots; i++)
            {
                Character character = playerCharacterOrder.GetCharacterBySlotIndex(i);
                if (character != null && character.CharacterPrefab != null)
                {
                    Battler battler = _characterBattlerDictionary[character];

                    if (!leaderSet) // this is the first character, which will directly follow the playerBehaviour
                    {
                        battler.SetCharacterMovementIsLeader(this.GetComponent<SortingGroup>().sortingOrder = CharacterOrder.NumberOfSlots);
                        _battlerToFollow = battler.transform;
                        leader = character;
                        leaderSet = true;
                        j++;
                    }
                    else
                    {
                        battler.ConnectFollowerToLeader(_characterBattlerDictionary[leader].GetComponent<CharacterMovement>(), _battlerGap, j, CharacterOrder.NumberOfSlots - j); // the rest follow in a chain
                        j++;
                    }
                }
            }
        }


        [Button]
        public void MoveEnemyBattlersForward()
        {

            // Todo, this is being called outside of battle and I don't know why

            CharacterOrder enemyCharacterOrder = ServiceLocator.Instance.CharacterModel.EnemyCharacterOrder;

            if(enemyCharacterOrder == null) { return; }

            int numberOfCharacters = enemyCharacterOrder.GetCharacters().Count;
            List<Character> characterList = new List<Character>();
            
            for (int i = 0; i < CharacterOrder.NumberOfSlots; i++)
            {
                Character character = enemyCharacterOrder.GetCharacterBySlotIndex(i);
                if (character != null)
                {
                    if (!characterList.Contains(character))
                    {
                        characterList.Add(enemyCharacterOrder.GetCharacterBySlotIndex(i));
                    }
                }
            }
            for (int i = 0; i < characterList.Count; i++)
            {
                Battler battler = _characterBattlerDictionary[characterList[i]];

                battler.ConnectFollowerToLeader(_frontBattlerPosition, _battlerGap, enemyCharacterOrder.GetSlotIndexByCharacter(characterList[i]), CharacterOrder.NumberOfSlots - i);
            }
            foreach (Character character in characterList)
            {
                if (_characterBattlerDictionary.ContainsKey(character))
                {
                    StartCoroutine(EnableBattlerMovementForSeconds(_characterBattlerDictionary[character], 2f));
                }
            }
        }

        [Button]
        public void MovePlayerBattlersForward()
        {

            // Todo, this is being called outside of battle and I don't know why

            CharacterOrder playerCharacterOrder = ServiceLocator.Instance.CharacterModel.PlayerCharacterOrder;

            if (playerCharacterOrder == null) { return; }

            int numberOfCharacters = playerCharacterOrder.GetCharacters().Count;
            List<Character> characterList = new List<Character>();

            for (int i = 0; i < CharacterOrder.NumberOfSlots; i++)
            {
                Character character = playerCharacterOrder.GetCharacterBySlotIndex(i);
                if (character != null)
                {
                    if (!characterList.Contains(character))
                    {
                        characterList.Add(playerCharacterOrder.GetCharacterBySlotIndex(i));
                    }
                }
            }

            for (int i = 0; i < characterList.Count; i++)
            {
                Battler battler = _characterBattlerDictionary[characterList[i]];

                battler.ConnectFollowerToLeader(_frontBattlerPosition, _battlerGap, playerCharacterOrder.GetSlotIndexByCharacter(characterList[i]), CharacterOrder.NumberOfSlots - i);
            }

            foreach (Character character in characterList)
            {
                if (_characterBattlerDictionary.ContainsKey(character))
                {
                    StartCoroutine(EnableBattlerMovementForSeconds(_characterBattlerDictionary[character], 2f));
                }
            }

        }

        public void UpdatePlayerBattlers()
		{
            if (_characterBattlerDictionary.Count == 0) { return; } 

            for (int i = _characterBattlerDictionary.Keys.Count - 1; i >= 0; i--)
            {
                Character character = _characterBattlerDictionary.ElementAt(i).Key;
                if (!ServiceLocator.Instance.CharacterModel.AllCharacters.Contains(character))
                {
                    _characterBattlerDictionary[character].Uninitialise();
                    Destroy(_characterBattlerDictionary[character].gameObject);
                    _characterBattlerDictionary.Remove(character);
                }
            }
            InstantiatePlayerCharacters();
        }

        public void UninstantiatePlayerBattlers()
        {
            if (_characterBattlerDictionary.Count == 0) { return; }

            for (int i = _characterBattlerDictionary.Keys.Count - 1; i >= 0; i--)
            {
                Character character = _characterBattlerDictionary.ElementAt(i).Key;
                _characterBattlerDictionary[character].Uninitialise();
                Destroy(_characterBattlerDictionary[character].gameObject);
                _characterBattlerDictionary.Remove(character);
            }
        }

        public void SetPlayerStartTransform(Transform transform)
        {
            _playerStartTransform = transform;
        }

        public void OnExitBattle()
        {

            for (int i = _characterBattlerDictionary.Count - 1; i >= 0; i--)
            {
                if (!_characterBattlerDictionary.Keys.ElementAt(i).IsPlayer)
                {
                    Character enemyCharacter = _characterBattlerDictionary.Keys.ElementAt(i);
                    GameObject battlerGameObject = _characterBattlerDictionary[enemyCharacter].gameObject;
                    _characterBattlerDictionary.Remove(enemyCharacter);
                    Destroy(battlerGameObject);
                }
            }
            if(_frontBattlerPosition != null)
            {
                Destroy(_frontBattlerPosition.gameObject);
            }
            _frontBattlerPosition = null;
        }

        #endregion

        #region Utilities

        private IEnumerator EnableBattlerMovementForSeconds(Battler battler, float seconds)
        {
            battler.GetComponent<CharacterMovement>().enabled = true;
            yield return new WaitForSeconds(seconds);
            if (battler != null)
            {
                battler.GetComponent<CharacterMovement>().enabled = false;
            }
        }

        private void DestroyAllBattlers()
		{
			foreach (Battler battler in _characterBattlerDictionary.Values)
			{
				Destroy(battler.gameObject);
            }
			_characterBattlerDictionary.Clear();

        }

        #endregion

        #region SavingLoading
        public object CaptureState()
		{
            return new SaveData
            {
                // Position

                x = transform.position.x,
                y = transform.position.y,
                z = transform.position.z,
                
			};
		}

		public void RestoreState(object state)
		{
			var saveData = (SaveData)state;

            _savedPosition = new Vector3(saveData.x, saveData.y, saveData.z);
        }

		[Serializable]
		public struct SaveData
		{			
			// Position

			public float x;
			public float y;
			public float z;
		}
		#endregion
	}
}

