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

		private int wounds;
				
		public int CurrentHealth => currentHealth; // getter
		public int MaxHealth => maxHealth;

		public bool IsDead => CheckIsDeadAndNotify();

		public int Wounds => wounds; // getter

		public HealthSystem(CharacterTemplate template)
		{
			this.currentHealth = template.CurrentHealth;
			this.maxHealth = template.MaxHealth;
		}

		public void IncrementCurrentHealth(int increment)
		{
			Debug.Log("My health was: " + currentHealth);
			currentHealth += increment;

			if (currentHealth > MaxHealth)
			{
				currentHealth = maxHealth;
			}

			NotifySubscribersHealthChanged();
			CheckIsDeadAndNotify();
			Debug.Log("My health is now: " + currentHealth);
		}



		public void IncrementMaxHealth(int increment)
		{
			maxHealth += increment;
			NotifySubscribersHealthChanged();
			CheckIsDeadAndNotify();
		}

		public void IncrementWounds(int increment)
		{
			wounds += increment;
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

		private bool CheckIsDeadAndNotify()
		{
			if (currentHealth <= 0 || maxHealth <= 0)
			{
				currentHealth = 0;
				if (maxHealth < 0)
				{
					maxHealth = 0;
				}
				OnCharacterDeath?.Invoke();
				return true;
			}
			return false;
		}

		public void AddDamage(Damage damage)
		{
			Debug.Log("Adding damage");
			IncrementCurrentHealth(-(int)damage.Value);
		}
		
		public void AddHealing(Healing healing)
		{
			Debug.Log("Adding damage");
			IncrementCurrentHealth((int)healing.Value);
		}
	}
}

