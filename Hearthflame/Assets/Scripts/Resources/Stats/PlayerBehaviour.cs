using System;
using UnityEngine;
using GramophoneUtils.SavingLoading;
using UnityEngine.Events;
using GramophoneUtils.Items.Containers;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using GramophoneUtils.Maps;
using UnityEngine.Rendering;

namespace GramophoneUtils.Stats
{
	//
	// Summary:
	// This is the MonoBehvaiour which attaches to the player object in the scene
	//

	public class PlayerBehaviour : MonoBehaviour, ISaveable
	{
		[SerializeField] private Transform battlerParent;
		[SerializeField] private float battlerGap = 2f;
		private Transform battlerToFollow;

		private string transitionTargetNameCache = "";

        private Dictionary<Character, Battler> playerBattlers = new Dictionary<Character, Battler>();
        public Dictionary<Character, Battler> PlayerBattlers => playerBattlers; // getter

        #region Callbacks

        private void Start()
		{
			battlerParent.position = transform.position;

			List<Character> characters = ServiceLocator.Instance.PlayerModel.PlayerCharacters;

			this.GetComponent<SortingGroup>().sortingOrder = characters.Count + 1;

            for (int i = 0; i < characters.Count; i++)
			{
				if (characters[i] != null && characters[i].CharacterTemplate.CharacterObject != null)
                {
                    Battler battler = Instantiate(characters[i].CharacterTemplate.CharacterObject, battlerParent.position + new Vector3(-2 * i, 0, 0), Quaternion.identity, battlerParent).GetComponent<Battler>();
						
					if (i == 0) // this is the first character, which will directly follow the playerBehaviour
					{
                        battler.SetCharacterMovementIsLeader(this.GetComponent<SortingGroup>().sortingOrder = characters.Count);
						battlerToFollow = battler.transform;
                    } else
					{
						battler.ConnectFollowerToLeader(playerBattlers[characters[i - 1]].GetComponent<CharacterMovement>(), battlerGap, characters.Count - i); // the rest follow in a chain
                    }

                    playerBattlers.Add(characters[i], battler);
                }
			}
		}

		public void Update()
		{
			if (battlerToFollow != null && ServiceLocator.Instance.GameStateManager.State == ServiceLocator.Instance.ExplorationState) // TODO hacky way to stop the camera following the first player in a battle
				// instead, the camera should follow something else during a battle
			{
                this.transform.position = battlerToFollow.transform.position;
            }
        }

        #endregion


        #region SavingLoading
        public object CaptureState()
		{
			string sceneName = ServiceLocator.Instance.ServiceLocatorObject.SceneController.GetActiveSceneName();
			bool doNotRestore = false;

			// Todo MASSIVE HACK
            if (sceneName == "Map1" || sceneName == "BaseScene")
			{
				doNotRestore = true;
            }
                return new SaveData
			{
				doNotRestorePosition = doNotRestore,

                // Position

                positionX = gameObject.transform.position.x,
				positionY = gameObject.transform.position.y,
                
			};
		}

		public void RestoreState(object state)
		{
			var saveData = (SaveData)state;

			// Position
			if (saveData.doNotRestorePosition)
			{
				return;
			}
				Debug.Log("We are resetting the player's position.");
				gameObject.transform.position = new Vector3(saveData.positionX, saveData.positionY);
        }

		

		[Serializable]
		public struct SaveData
		{
			public bool doNotRestorePosition;
			
			// Position

			public float positionX;
			public float positionY;
		}
		#endregion
	}
}

