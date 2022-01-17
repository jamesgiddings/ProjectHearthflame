using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GramophoneUtils.Stats
{
	[Serializable]
	public class HealthSystem
	{
		public event EventHandler OnHealthChanged;
		public event EventHandler OnCharacterDeath;
		
		private int currentHealth;
		private int maxHealth;
		
		public int CurrentHealth => currentHealth; // getter
		public int MaxHealth => maxHealth;

		public HealthSystem(CharacterTemplate template)
		{
			this.currentHealth = template.CurrentHealth;
			this.maxHealth = template.MaxHealth; ;
		}

		public void IncrementCurrentHealth(int increment)
		{
			currentHealth += increment;

			if (currentHealth > MaxHealth)
			{
				currentHealth = maxHealth;
			}

			if (OnHealthChanged != null)
			{
				OnHealthChanged(this, EventArgs.Empty);
			}

			if (currentHealth <= 0)
			{
				currentHealth = 0;

				if (OnCharacterDeath != null)
				{
					OnCharacterDeath(this, EventArgs.Empty);
				}
			}
		}


		public void IncrementMaxHealth(int increment)
		{
			maxHealth += increment;

			if (OnHealthChanged != null)
			{
				OnHealthChanged(this, EventArgs.Empty);
			}

			if (maxHealth <= 0)
			{
				maxHealth = 0;

				if (OnCharacterDeath != null)
				{
					OnCharacterDeath(this, EventArgs.Empty);
				}
			}
		}
	}
}

