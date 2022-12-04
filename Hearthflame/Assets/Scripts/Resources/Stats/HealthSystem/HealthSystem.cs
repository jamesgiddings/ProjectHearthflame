using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GramophoneUtils.Stats
{
	[Serializable]
	public class HealthSystem
	{
		public Action<int> OnHealthChanged;
		public Action<BattlerNotificationImpl> OnHealthChangedNotification;
		public Action OnCharacterDeath;
		
		private int currentHealth;
		private int maxHealth;

		private int wounds;
				
		public int CurrentHealth => currentHealth; // getter
		public int MaxHealth => maxHealth;

		public bool IsDead => CheckIsDeadAndNotify();

		public int Wounds => wounds; // getter

		public HealthSystem(CharacterClass characterClass)
		{
			this.currentHealth = characterClass.BaseHealth;
			this.maxHealth = characterClass.BaseMaxHealth;
		}

		public void IncrementCurrentHealth(int increment)
		{
			currentHealth += increment;

			if (currentHealth > MaxHealth)
			{
				currentHealth = maxHealth;
			}

			NotifySubscribersHealthChanged(increment);
			CheckIsDeadAndNotify();
		}

		public void IncrementCurrentHealthAsPercentageOfCurrentHealth(float percentageAsDecimal)
		{
			IncrementCurrentHealth((int)(currentHealth * percentageAsDecimal));
		}

		public void IncrementCurrentHealthAsPercentageOfMaxHealth(float percentageAsDecimal)
		{
			IncrementCurrentHealth((int)(maxHealth * percentageAsDecimal));
		}		
		
		public void IncrementMaxHealth(int increment)
		{
			maxHealth += increment;
			NotifySubscribersHealthChanged(increment);
			CheckIsDeadAndNotify();
		}

		public void IncrementMaxHealthAsPercentageOfMaxHealth(float percentageAsDecimal)
		{
			// get percentage 
			IncrementMaxHealth((int)(maxHealth * percentageAsDecimal));
		}

		public void IncrementWounds(int increment)
		{
			wounds += increment;
		}

		public void SetCurrentHealthToPercentage(float percentageAsDecimal)
		{
			SetCurrentHealth((int)(currentHealth * percentageAsDecimal));
		}

		public void SetMaxHealthToPercentage(float percentageAsDecimal)
		{
			SetMaxHealth((int)(currentHealth * percentageAsDecimal));
		}

		public void SetMaxHealth(int value)
		{
			int cache = maxHealth - value;
			maxHealth = value;
			NotifySubscribersHealthChanged(cache * -1);
			CheckIsDeadAndNotify();
		}

		public void SetCurrentHealth(int value)
		{
			int cache = currentHealth - value;
			currentHealth = value;
			NotifySubscribersHealthChanged(cache * -1);
			CheckIsDeadAndNotify();
		}
		private void NotifySubscribersHealthChanged(int value)
		{
			OnHealthChanged?.Invoke(value);
			BattlerNotificationImpl battlerNotificationImpl = new BattlerNotificationImpl(value.ToString(), value <= 0 ? Color.red : Color.green);
			OnHealthChangedNotification?.Invoke(battlerNotificationImpl);
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
			IncrementCurrentHealth(-(int)damage.Value);
		}
		
		public void AddHealing(Healing healing)
		{
			IncrementCurrentHealth((int)healing.Value);
		}
	}
}

