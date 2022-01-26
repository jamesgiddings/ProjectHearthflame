using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GramophoneUtils.Stats
{
	[Serializable]
	public class HealthSystem
	{
		public Action OnHealthChanged;
		public Action OnCharacterDeath;
		
		private int currentHealth;
		private int maxHealth;
		
		public int CurrentHealth => currentHealth; // getter
		public int MaxHealth => maxHealth;

		public HealthSystem(CharacterTemplate template)
		{
			this.currentHealth = template.CurrentHealth;
			this.maxHealth = template.MaxHealth;
		}

		public void IncrementCurrentHealth(int increment)
		{
			currentHealth += increment;

			if (currentHealth > MaxHealth)
			{
				currentHealth = maxHealth;
			}

			NotifySubscribersHealthChanged();
			CheckIsDeadAndNotify();
		}



		public void IncrementMaxHealth(int increment)
		{
			maxHealth += increment;
			NotifySubscribersHealthChanged();
			CheckIsDeadAndNotify();
		}


		public void SetMaxHealth(int value)
		{
			maxHealth = value;
			NotifySubscribersHealthChanged();
			CheckIsDeadAndNotify();
		}

		public void SetCurrentHealth(int value)
		{
			currentHealth = value;
			NotifySubscribersHealthChanged();
			CheckIsDeadAndNotify();
		}
		private void NotifySubscribersHealthChanged()
		{
			OnHealthChanged?.Invoke();
		}

		private void CheckIsDeadAndNotify()
		{
			if (currentHealth <= 0 || maxHealth <= 0)
			{
				currentHealth = 0;
				if (maxHealth < 0)
				{
					maxHealth = 0;
				}
				OnCharacterDeath?.Invoke();
			}
		}
	}
}

