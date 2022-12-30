using System;
using UnityEngine;
using GramophoneUtils.SavingLoading;
using System.Collections.Generic;
using UnityEngine.Rendering;
using GramophoneUtils.Characters;
using System.Linq;
using Sirenix.OdinInspector;
using DG.Tweening;

namespace GramophoneUtils.Stats
{
	public class CharacterGameObjectManager : MonoBehaviour, ISaveable
	{
        private BattleDataModel _battleDataModel;

        private Transform _battlerToFollow;

        private Transform _playerStartTransform = null;

        private Vector3 _savedPosition = Vector3.zero;
        
        private Dictionary<Character, Battler> _characterBattlerDictionary = new Dictionary<Character, Battler>();
        public Dictionary<Character, Battler> CharacterBattlerDictionary => _characterBattlerDictionary;

        [SerializeField] private Transform _battlerParent;
		[SerializeField] private float battlerGap = 2f;
		
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
            Character[] characters = ServiceLocator.Instance.CharacterModel.FrontPlayerCharacters;
            this.GetComponent<SortingGroup>().sortingOrder = characters.Length + 1;
            Vector3 instantiationPosition = _playerStartTransform == null ? _savedPosition == Vector3.zero ? this.transform.position : _savedPosition : _playerStartTransform.position;

            for (int i = 0; i < characters.Length; i++)
            {
                if (characters[i] != null && characters[i].CharacterPrefab != null && !_characterBattlerDictionary.ContainsKey(characters[i]))
                {
                    Battler battler = Instantiate(characters[i].CharacterPrefab, instantiationPosition + new Vector3(-2 * i, 0, 0), Quaternion.identity, _battlerParent).GetComponent<Battler>();
                    battler.Initialise(ServiceLocator.Instance.BattleManager, characters[i]);
                    
                    if (i == 0) // this is the first character, which will directly follow the playerBehaviour
                    {
                        battler.SetCharacterMovementIsLeader(this.GetComponent<SortingGroup>().sortingOrder = characters.Length);
                        _battlerToFollow = battler.transform;
                    }
                    else
                    {
                        battler.ConnectFollowerToLeader(_characterBattlerDictionary[characters[0]].GetComponent<CharacterMovement>(), battlerGap * i, characters.Length - i); // the rest follow in a chain
                    }

                    _characterBattlerDictionary.Add(characters[i], battler);
                }
            }

            _playerStartTransform = null; // set this to null, as the override transform should only be used once (i.e. on loading into a scene or from a save)
        }

        public void InstantiateEnemyCharacters()
        {
            Character[] characters = ServiceLocator.Instance.CharacterModel.FrontEnemyCharacters;

            for (int i = 0; i < characters.Length; i++)
            {
                if (characters[i] != null && characters[i].CharacterPrefab != null && !_characterBattlerDictionary.ContainsKey(characters[i]))
                {
                    Battler battler = Instantiate(characters[i].CharacterPrefab, this.transform.position + new Vector3(2 + (2 * i), 0, 0), Quaternion.identity).GetComponent<Battler>();
                    battler.Initialise(ServiceLocator.Instance.BattleManager, characters[i]);

                    if (i == 0) // this is the first character, which will directly follow the playerBehaviour
                    {
                        battler.SetCharacterMovementIsLeader(this.GetComponent<SortingGroup>().sortingOrder = characters.Length);
                        _battlerToFollow = battler.transform;
                    }
                    else
                    {
                        battler.ConnectFollowerToLeader(_characterBattlerDictionary[characters[0]].GetComponent<CharacterMovement>(), battlerGap * i, characters.Length - i); // the rest follow in a chain
                    }

                    _characterBattlerDictionary.Add(characters[i], battler);
                }
            }
        }

        public void UpdateEnemyBattlers()
        {
            if (_characterBattlerDictionary.Count == 0) { return; }

            for (int i = _characterBattlerDictionary.Keys.Count - 1; i >= 0; i--)
            {
                Character character = _characterBattlerDictionary.ElementAt(i).Key;
                if (!ServiceLocator.Instance.CharacterModel.AllFrontCharactersList.Contains(character))
                {
                    _characterBattlerDictionary[character].Uninitialise();
                    Destroy(_characterBattlerDictionary[character].gameObject);
                    _characterBattlerDictionary.Remove(character);
                }
            }
            InstantiateEnemyCharacters();
        }

        public void UpdatePlayerBattlers()
		{
            if (_characterBattlerDictionary.Count == 0) { return; } 

            for (int i = _characterBattlerDictionary.Keys.Count - 1; i >= 0; i--)
            {
                Character character = _characterBattlerDictionary.ElementAt(i).Key;
                if (!ServiceLocator.Instance.CharacterModel.AllFrontCharactersList.Contains(character))
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
        }

        #endregion

        #region Utilities

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

