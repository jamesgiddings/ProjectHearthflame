using System;
using UnityEngine;
using GramophoneUtils.SavingLoading;
using UnityEngine.Events;

namespace GramophoneUtils.Stats
{
    public class StatSystemBehaviour : MonoBehaviour, ISaveable
    {
		[SerializeField] private BaseStats baseStats;

		public UnityEvent onStatsChanged;

		private StatSystem statSystem;
        public StatSystem StatSystem
		{
			get
			{
				if (statSystem != null) { return statSystem;  }
				statSystem = new StatSystem(baseStats);
				return statSystem;
			}
		}

		private void Start()
		{
			RegisterOnStatsChangedEvent();
		}

		private void RegisterOnStatsChangedEvent()
		{
			foreach (var stat in StatSystem.Stats)
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
			foreach (var stat in statSystem.Stats)
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

