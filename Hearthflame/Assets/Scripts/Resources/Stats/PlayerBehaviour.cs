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
	// This is the MonoBehvaiour which attaches to the player object in the scene
	//

	public class PlayerBehaviour : MonoBehaviour, ISaveable
	{
		private string transitionTargetNameCache = "";

		#region SavingLoading
		public object CaptureState()
		{
			
			return new SaveData
			{
				// Position

				positionX = gameObject.transform.position.x,
				positionY = gameObject.transform.position.y,
                
			};
		}

		public void RestoreState(object state)
		{
			var saveData = (SaveData)state;

			// Position
			Debug.Log("We are resetting the player's position.");
			gameObject.transform.position = new Vector3(saveData.positionX, saveData.positionY);

			
		}

		

		[Serializable]
		public struct SaveData
		{
			// Position

			public float positionX;
			public float positionY;
		}
		#endregion
	}
}

