using GramophoneUtils.Characters;
using GramophoneUtils.Stats;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace GramophoneUtils.Battles
{
    [CreateAssetMenu(fileName = "EnemiesKilled", menuName = "Battles/EnemiesKilled")]
    public class EnemiesKilledConditions : ScriptableObject
    {
        [SerializeField] private bool _killAll;

        [SerializeField, HideIf("_hideAnyOf"), Tooltip("Only one of these lists should be used.")] private List<Character> _anyOf;
        [SerializeField, HideIf("_hideAllOf"), Tooltip("Only one of these lists should be used.")] private List<Character> _allOf;
        [SerializeField, HideIf("_hideAnyOfClass"), Tooltip("Only one of these lists should be used.")] private List<CharacterClass> _anyOfClass;
        [SerializeField, HideIf("_hideAllOfClass"), Tooltip("Only one of these lists should be used.")] private List<CharacterClass> _allOfClass;
       
        #region Only show one list in editor

        // properties which hide fields based on one of the lists being used.

        private int _anyOfCount
        {
            get
            {
                if (_anyOf != null) { return _anyOf.Count; }
                return 0;
            }
        }

        private int _allOfCount
        {
            get
            {
                if (_allOf != null) { return _allOf.Count; }
                return 0;
            }
        }

        private int _anyOfClassCount
        {
            get
            {
                if (_anyOfClass != null) { return _anyOfClass.Count; }
                return 0;
            }
        }

        private int _allOfClassCount
        {
            get
            {
                if (_allOfClass != null) { return _allOfClass.Count; }
                return 0;
            }
        }

        private bool _hideAnyOf
        {
            get
            {
                if (_allOfCount > 0 || _anyOfClassCount > 0 || _allOfClassCount > 0 || _killAll)
                {
                    _anyOf = new List<Character>();
                    return true;
                }
                
                return false;
            }
        }

        private bool _hideAllOf
        {
            get
            {
                if (_anyOfCount > 0 || _anyOfClassCount > 0 || _allOfClassCount > 0 || _killAll)
                {
                    _allOf = new List<Character>();
                    return true;
                }
                return false;
            }
        }

        private bool _hideAnyOfClass
        {
            get
            {
                if (_anyOfCount > 0 || _allOfCount > 0 || _allOfClassCount > 0 || _killAll)
                {
                    _anyOfClass = new List<CharacterClass>();
                    return true;
                }
                return false;
            }
        }

        private bool _hideAllOfClass
        {
            get
            {
                if (_allOfCount > 0 || _anyOfClassCount > 0 || _anyOfCount > 0 || _killAll)
                {
                    _allOfClass = new List<CharacterClass>();
                    return true;
                }
                return false;
            }
        }
#endregion 

        public bool KillAll => _killAll;

        public List<Character> AnyOf => _anyOf;
        public List<Character> AllOf => _allOf;
        public List<CharacterClass> AnyOfClass => _anyOfClass;
        public List<CharacterClass> AllOfClass => _allOfClass;

        #region API
        public bool HaveEnemiesKilledConditionsBeenMet(BattleDataModel battleDataModel)
        {
            CharacterModel characterModel = ServiceLocator.Instance.CharacterModel;

            if (_killAll) 
            {
                return AreAllEnemiesKilled(characterModel); // this condition supersedes the others so we can return early if condition is met.
            }

            if (_allOfCount > 0)
            {
                if (AreAnyEnemiesInTheAllOfListStillAlive(characterModel))
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }

            if (_allOfClassCount > 0)
            {
                if (AreAnyEnemiesInTheAllOfClassListStillAlive(characterModel))
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }


            if (_anyOfCount > 0)
            {
                if (HaveNoEnemiesInTheAnyOfListBeenKilled(characterModel))
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }

            if (_anyOfClassCount > 0)
            {
                if (HaveNoEnemiesOfClassInTheAnyOfClassListBeenKilled(characterModel))
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }


            return true;
        }

        #endregion

        #region Utilities
        private bool AreAllEnemiesKilled(CharacterModel characterModel)
        {
            bool allDead = true;
            foreach (Character enemyCharacter in characterModel.EnemyCharacters)
            {
                if (!enemyCharacter.HealthSystem.IsDead)
                {
                    allDead = false;
                }
            }
            foreach (Character enemyCharacter in characterModel.ReserveEnemyCharacters)
            {
                if (!enemyCharacter.HealthSystem.IsDead)
                {
                    allDead = false;
                }
            }

            return allDead;
        }

        private bool AreAnyEnemiesInTheAllOfListStillAlive(CharacterModel characterModel)
        {
            foreach (Character character in _allOf) // foreach characterTemplate that must be killed, if a character in the enemyList has that characterTemplate, return false.
            {
                foreach (Character enemyCharacter in characterModel.EnemyCharacters)
                {
                    if (character.UID.Equals(enemyCharacter.UID))
                    {
                        if (!enemyCharacter.HealthSystem.IsDead)
                        {
                            return true;
                        }
                    }
                }
                foreach (Character enemyCharacter in characterModel.ReserveEnemyCharacters)
                {
                    if (character.UID.Equals(enemyCharacter.UID))
                    {
                        if (!enemyCharacter.HealthSystem.IsDead)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        private bool AreAnyEnemiesInTheAllOfClassListStillAlive(CharacterModel characterModel)
        {
            foreach (CharacterClass characterClass in _allOfClass) // foreach characterTemplate that must be killed, if a character in the enemyList has that characterTemplate, return false.
            {
                foreach (Character character in characterModel.EnemyCharacters)
                {
                    if (characterClass.UID.Equals(character.CharacterClass.UID))
                    {
                        if (!character.HealthSystem.IsDead)
                        {
                            return true;
                        }
                    }
                }
                foreach (Character character in characterModel.ReserveEnemyCharacters)
                {
                    if (characterClass.UID.Equals(character.CharacterClass.UID))
                    {
                        if (!character.HealthSystem.IsDead)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }


        private bool HaveNoEnemiesInTheAnyOfListBeenKilled(CharacterModel characterModel)
        {
            foreach (Character character in _anyOf)
            {
                foreach (Character enemyCharacter in characterModel.EnemyCharacters)
                {
                    if (character.UID.Equals(enemyCharacter.UID))
                    {
                        if (enemyCharacter.HealthSystem.IsDead)
                        {
                            return false;
                        }
                    }
                }
                foreach (Character enemyCharacter in characterModel.ReserveEnemyCharacters)
                {
                    if (character.UID.Equals(enemyCharacter.UID))
                    {
                        if (enemyCharacter.HealthSystem.IsDead)
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }

        private bool HaveNoEnemiesOfClassInTheAnyOfClassListBeenKilled(CharacterModel characterModel)
        {
            foreach (CharacterClass characterClass in _anyOfClass) // foreach characterTemplate that must be killed, if a character in the enemyList has that characterTemplate, return false.
            {
                foreach (Character character in characterModel.EnemyCharacters)
                {
                    if (characterClass.UID.Equals(character.CharacterClass.UID))
                    {
                        if (character.HealthSystem.IsDead)
                        {
                            return false;
                        }
                    }
                }
                foreach (Character character in characterModel.ReserveEnemyCharacters)
                {
                    if (characterClass.UID.Equals(character.CharacterClass.UID))
                    {
                        if (character.HealthSystem.IsDead)
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_killAll)
            {
                _anyOf = null;
                _allOf = null;
                _anyOfClass = null;
                _allOfClass = null;
            }
        }
#endif

        #endregion
    }
}