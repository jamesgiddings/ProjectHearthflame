using GramophoneUtils.Battles;
using GramophoneUtils.Characters;
using GramophoneUtils.Stats;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GramophoneUtils.Battles
{
    /// <summary>
    /// This object allows the designer to create the conditions for victory in a battle.
    /// If _rearWin is set to true, then the rear battle must be successful for victory.
    /// The EnemiesKilled object exposes for lists, which allow the designer to set which
    /// enemies in the front must be killed (i.e. any of the specified characters, or
    /// all of them, or any of a specific class, or all of a specific class.
    /// This allows the designer to create victory conditions if the boss is slain, for example,
    /// or once all the necromancers have been slain in an undead battle.
    /// 
    /// The class also provides a method that can be called by a class which has access to the 
    /// battleDataModel to find out if the victory conditions have been met for the front, the rear
    /// and the battle overall.
    /// </summary>
    [CreateAssetMenu(fileName = "BattleWinConditions", menuName = "Battles/BattleWinConditions")]
    public class BattleWinConditions : ScriptableObject
    {
        [SerializeField] private bool _isRearWinRequired;
        [SerializeField] private EnemiesKilledConditions _enemiesKilled;
        [SerializeField, Tooltip("Not yet implemented")] private int _roundsSurvived; // todo not yet implemented

        // TODO, in OnValidate, we should add a check to be sure that the EnemiesKilledConditions only contains opponents that are in the battle

        #region API

        public bool IsPlayerVictory(BattleDataModel battleDataModel)
        {
            if (IsFrontLost(battleDataModel))
            {
                return false;
            }

            if (!_isRearWinRequired)
            {
                return IsFrontWon(battleDataModel);
            }

            return IsRearWon(battleDataModel);
        }

        public bool IsEnemyVictory(BattleDataModel battleDataModel)
        {
            return IsFrontLost(battleDataModel);
        }

        public bool IsFrontWon(BattleDataModel battleDataModel)
        {
            return _enemiesKilled.HaveEnemiesKilledConditionsBeenMet(battleDataModel);
        }

        public bool IsFrontLost(BattleDataModel battleDataModel)
        {
            bool allDead = true;
            foreach (Character character in ServiceLocator.Instance.CharacterModel.FrontPlayerCharactersList)
            {
                if (!character.HealthSystem.IsDead)
                {
                    allDead = false;
                }
            }
            return allDead;
        }

        public bool IsRearWon(BattleDataModel battleDataModel)
        {
            return battleDataModel.BattleRear.GetIsWon();
        }

        #endregion
    }
}