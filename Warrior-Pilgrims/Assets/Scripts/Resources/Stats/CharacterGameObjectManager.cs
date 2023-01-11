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

        private Dictionary<Character, Battler> _characterBattlerDictionary = new Dictionary<Character, Battler>();
        public Dictionary<Character, Battler> CharacterBattlerDictionary => _characterBattlerDictionary;

        [SerializeField] private Transform _battlerParent;
		[SerializeField] private float battlerGap = 2f;
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

            for (int i = 0; i < CharacterOrder.NumberOfSlots; i++)
            {
                Character character = playerCharacterOrder.GetCharacterBySlotIndex(i);
                if (character != null && character.CharacterPrefab != null && !_characterBattlerDictionary.ContainsKey(character))
                {
                    Battler battler = Instantiate(character.CharacterPrefab, instantiationPosition + new Vector3(-2 * i, 0, 0), Quaternion.identity, _battlerParent).GetComponent<Battler>();
                    battler.Initialise(character);
                    
                    if (i == 0) // this is the first character, which will directly follow the playerBehaviour
                    {
                        battler.SetCharacterMovementIsLeader(this.GetComponent<SortingGroup>().sortingOrder = CharacterOrder.NumberOfSlots);
                        _battlerToFollow = battler.transform;
                    }
                    else
                    {
                        battler.ConnectFollowerToLeader(_characterBattlerDictionary[playerCharacterOrder.GetCharacterBySlotIndex(0)].GetComponent<CharacterMovement>(), battlerGap * i, CharacterOrder.NumberOfSlots - i); // the rest follow in a chain
                    }

                    _characterBattlerDictionary.Add(character, battler);
                }
            }

            _playerStartTransform = null; // set this to null, as the override transform should only be used once (i.e. on loading into a scene or from a save)
        }

        public void InstantiateEnemyCharacters()
        {
            if (_frontBattlerPosition == null)
            {
                _frontBattlerPosition = Instantiate(_frontBattlerPositionPrefab, this.transform.position + new Vector3(2, 0, 0), Quaternion.identity).GetComponent<CharacterMovement>();
            }
            CharacterOrder enemyCharacterOrder = ServiceLocator.Instance.CharacterModel.EnemyCharacterOrder;
            
            for (int i = 0; i < CharacterOrder.NumberOfSlots; i++)
            {
                Character character = enemyCharacterOrder.GetCharacterBySlotIndex(i);
                if (character != null && character.CharacterPrefab != null && !_characterBattlerDictionary.ContainsKey(character))
                {
                    Battler battler = Instantiate(character.CharacterPrefab, _frontBattlerPosition.transform.position + new Vector3((2 * i), 0, 0), Quaternion.identity).GetComponent<Battler>();
                    battler.Initialise(character);

                    battler.ConnectFollowerToLeader(_frontBattlerPosition, battlerGap * i, CharacterOrder.NumberOfSlots - i); // the rest follow in a chain
                    
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

        [Button]
        public void MoveEnemyBattlersForward()
        {
            CharacterOrder enemyCharacterOrder = ServiceLocator.Instance.CharacterModel.EnemyCharacterOrder;
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

                battler.ConnectFollowerToLeader(_frontBattlerPosition, battlerGap * enemyCharacterOrder.GetSlotIndexByCharacter(characterList[i]), CharacterOrder.NumberOfSlots - i);
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
            Destroy(_frontBattlerPosition.gameObject);
            _frontBattlerPosition = null;
        }

        #endregion

        #region Utilities

        private IEnumerator EnableBattlerMovementForSeconds(Battler battler, float seconds)
        {
            battler.GetComponent<CharacterMovement>().enabled = true;
            yield return new WaitForSeconds(seconds);
            battler.GetComponent<CharacterMovement>().enabled = false;
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

