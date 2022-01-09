using System;
using UnityEngine;
using GramophoneUtils.SavingLoading;
using UnityEngine.Events;

namespace GramophoneUtils.Stats
{
    public class CharacterBehaviour : MonoBehaviour, ISaveable
    {
		[SerializeField] private CharacterTemplate template;

		public UnityEvent onStatsChanged;

		private Character character;
        public Character Character
		{
			get
			{
				if (character != null) { return character;  }
				character = new Character(template);
				return character;
			}
		}

		private void Start()
		{
			RegisterOnStatsChangedEvent();
		}

		private void RegisterOnStatsChangedEvent()
		{
			foreach (var stat in Character.StatSystem.Stats)
			{
				stat.Value.OnStatChanged += OnStatsChanged;
			}
		}
		
		private void OnStatsChanged()
		{	
			onStatsChanged.Invoke();
		}

		private void OnDestroy() // just deregister from the unity event
		{
			foreach (var stat in character.StatSystem.Stats)
			{
				stat.Value.OnStatChanged -= OnStatsChanged;
			}
		}

		#region SavingLoading
		public object CaptureState()
		{
			return new SaveData
			{
				TestNumber = 1,
			};
		}

		public void RestoreState(object state)
		{
			var saveData = (SaveData)state;

			//testNumber = saveData.TestNumber;
		}

		[Serializable]
		private struct SaveData
		{
			public int TestNumber;
		}
		#endregion
	}
}

