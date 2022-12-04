using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GramophoneUtils.Stats
{
    public class StatSystem

    {
		private Character character;

		private Dictionary<string, IStatType> statTypeStringRefDictionary = new Dictionary<string, IStatType>();

		private readonly Dictionary<IStatType, Stat> stats = new Dictionary<IStatType, Stat>();

        public Action<BattlerNotificationImpl> OnStatSystemNotification;
        public Dictionary<IStatType, Stat> Stats => stats; //getter
		public StatSystem() { } //constructor 1
		public StatSystem(CharacterTemplate template, Character character) //constructor 2
		{			
			foreach (var stat in template.Stats.Stats)
			{
				stats.Add(stat.StatType, new Stat(stat.Value));
				statTypeStringRefDictionary.Add(stat.StatType.Name, stat.StatType);
			}
			this.character = character;
		}

		public Dictionary<string, IStatType> StatTypeStringRefDictionary => statTypeStringRefDictionary;

		public void AddModifier(StatModifier modifier)
		{
			if (!stats.TryGetValue(modifier.StatType, out Stat stat))
			{
				stat = new Stat(modifier.StatType);
				stats.Add(modifier.StatType, stat);
			}
			stat.AddModifier(modifier);
			modifier.OnDurationElapsed += RemoveModifier;
		}

		public void UpdateStatBaseValue(StatModifierBlueprint statComponentBlueprint)
		{
			if (!stats.TryGetValue(statComponentBlueprint.StatType, out Stat stat))
			{
				stat = new Stat(statComponentBlueprint.StatType);
				stats.Add(statComponentBlueprint.StatType, stat);
			}
			stat.UpdateBaseValue(statComponentBlueprint.Value);
		}
		
		public void IncrementStatBaseValue(StatModifierBlueprint statComponentBlueprint)
		{
			if (!stats.TryGetValue(statComponentBlueprint.StatType, out Stat stat))
			{
				stat = new Stat(statComponentBlueprint.StatType);
				stats.Add(statComponentBlueprint.StatType, stat);
			}
			stat.IncrementBaseValue(statComponentBlueprint.Value);
		}

		public Stat GetStat(IStatType type)
		{
			if (!stats.TryGetValue(type, out Stat stat))
			{
				stat = new Stat(type);
				stats.Add(type, stat);
			}
			return stat;
		}

		public float GetStatValue(IStatType type)
		{
			if (!stats.TryGetValue(type, out Stat stat))
			{
				stat = new Stat(type);
				stats.Add(type, stat);
			}

			return stat.Value;
		}

		public float GetBaseStatValue(IStatType type)
		{
			if (!stats.TryGetValue(type, out Stat stat))
			{
				stat = new Stat(type);
				stats.Add(type, stat);
			}

			return stat.GetBaseValue();
		}

		public void RemoveModifier(StatModifier modifier)
		{
			if (!stats.TryGetValue(modifier.StatType, out Stat stat))
			{
				return;
			}
			stat.RemoveModifier(modifier);
			modifier.OnDurationElapsed -= RemoveModifier;
		}

		#region Derived Stats

		public float GetMeleeAccuracy()
		{
			return GameManager.Instance.StatSystemConstants.BaseMeleeAccuracy + ((GetStatValue(StatTypeStringRefDictionary["Strength"]) * GameManager.Instance.StatSystemConstants.StrengthMultiplier)) + GetStatValue(StatTypeStringRefDictionary["Melee Accuracy"]);
		}

		public float GetRangedAccuracy()
		{
			return GameManager.Instance.StatSystemConstants.BaseRangedAccuracy + ((GetStatValue(StatTypeStringRefDictionary["Dexterity"]) * GameManager.Instance.StatSystemConstants.DexterityMultiplier)) + GetStatValue(StatTypeStringRefDictionary["Ranged Accuracy"]);
		}

		public float GetMagicAccuracy()
		{
			return GameManager.Instance.StatSystemConstants.BaseRangedAccuracy + ((GetStatValue(StatTypeStringRefDictionary["Magic"]) * GameManager.Instance.StatSystemConstants.MagicMultiplier)) + GetStatValue(StatTypeStringRefDictionary["Magic Accuracy"]);
		}

		public float GetMeleeEvasion()
		{
			return GameManager.Instance.StatSystemConstants.BaseMeleeEvasion + ((GetStatValue(StatTypeStringRefDictionary["Speed"]) * GameManager.Instance.StatSystemConstants.SpeedMultiplier)) + GetStatValue(StatTypeStringRefDictionary["Melee Evasion"]);
		}

		public float GetRangedEvasion()
		{
			return GameManager.Instance.StatSystemConstants.BaseRangedEvasion + ((GetStatValue(StatTypeStringRefDictionary["Speed"]) * GameManager.Instance.StatSystemConstants.SpeedMultiplier)) + GetStatValue(StatTypeStringRefDictionary["Ranged Evasion"]);
		}

		public float GetMagicEvasion()
		{
			return GameManager.Instance.StatSystemConstants.BaseMagicEvasion + ((GetStatValue(StatTypeStringRefDictionary["Speed"]) * GameManager.Instance.StatSystemConstants.SpeedMultiplier)) + GetStatValue(StatTypeStringRefDictionary["Magic Evasion"]);
		}

		private List<Damage> FilterMisses(List<Damage> receivedDamages, Character originator)
		{
			List<Damage> filteredDamages = new List<Damage>();
			foreach (Damage damage in receivedDamages)
			{
				if (!GetIfMisses(damage, originator))
				{
					filteredDamages.Add(damage);
				}
			}
			return filteredDamages;
		}

		private bool GetIfMisses(Damage damage, Character originator)
		{
			switch (damage.AttackType)
			{
				case AttackType.Magic:
					if (GetMagicEvasion() > originator.StatSystem.GetMagicAccuracy())
					{
						return true;
					}
					else return false;			
				case AttackType.Ranged:
					if (GetRangedEvasion() > originator.StatSystem.GetRangedAccuracy())
					{
						return true;
					}
					else return false;
				case AttackType.Melee:
					if (GetMeleeEvasion() > originator.StatSystem.GetMeleeAccuracy())
					{
						return true;
					}
					else return false;
				default:
					return false;
			}
		}

		public void ReceiveModifiedDamageStructs(List<Damage> receivedDamages, Character originator)
		{
			List<Damage> filteredDamages = FilterMisses(receivedDamages, originator);
			if (filteredDamages.Count == 0 && receivedDamages.Count > 0)
			{
				string evasionString = receivedDamages[0].AttackType == AttackType.Magic ? "Resist!" : "Miss!";
				OnStatSystemNotification?.Invoke(new BattlerNotificationImpl(evasionString)); // TODO move this string to a constants file
				return;
			}
			List<Damage> modifiedDamages = new List<Damage>();
			foreach (Damage damage in filteredDamages)
			{
				modifiedDamages.Add(ModifyIncomingDamage(damage));
			}
			ApplyModifiedDamageObjects(modifiedDamages);
		}

		public void ReceiveModifiedHealingStructs(List<Healing> receivedHealings, Character originator)
		{
			List<Healing> modifiedHealings = new List<Healing>();
			foreach (Healing healing in receivedHealings)
			{
				modifiedHealings.Add(ModifyIncomingHealing(healing));
			}

			ApplyModifiedHealingObjects(modifiedHealings);
		}

		private void ApplyModifiedDamageObjects(List<Damage> modifiedDamages)
		{
			foreach (Damage damage in modifiedDamages)
			{
				character.HealthSystem.AddDamage(damage);
			}
		}
		
		private void ApplyModifiedHealingObjects(List<Healing> modifiedDamages)
		{
			foreach (Healing healing in modifiedDamages)
			{
				character.HealthSystem.AddHealing(healing);
			}
		}

		public Damage ModifyIncomingDamage(Damage damage)
		{
			float newValue = damage.Value - GameManager.Instance.StatSystemConstants.BasePhysicalArmour;

			return new Damage(newValue, damage.Element, damage.AttackType, damage.source);
		}

		public Healing ModifyIncomingHealing(Healing healing)
		{
			float newValue = healing.Value; // Find modifier here

			return new Healing(newValue, healing.source);
		}

		public Damage ModifyOutgoingDamage(Damage damage)
		{
			float newValue = damage.Value + (GetStatValue(StatTypeStringRefDictionary["Strength"]) * GameManager.Instance.StatSystemConstants.StrengthMultiplier);

			return new Damage(newValue, damage.Element, damage.AttackType, damage.source);
		}		
		
		public Healing ModifyOutgoingHealing(Healing healing)
		{
			float newValue = healing.Value + (GetStatValue(StatTypeStringRefDictionary["Magic"]) * GameManager.Instance.StatSystemConstants.MagicMultiplier);

			return new Healing(newValue, healing.source);
		}

		#endregion
	}
}
