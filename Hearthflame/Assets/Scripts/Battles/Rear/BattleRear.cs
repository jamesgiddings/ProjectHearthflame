using GramophoneUtils.Stats;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GramophoneUtils.Battles
{
    [CreateAssetMenu(fileName = "BattleRear", menuName = "Battles/BattleRear")]
    public class BattleRear : ScriptableObject
    {
        [SerializeField] private List<Check> _anyOfObjectRef;
        [SerializeField] private List<Check> _allOfObjectRef;

        private List<CheckInstance> _anyOf;
        private List<CheckInstance> _allOf;

        private bool _isInitialised = false;

        public List<CheckInstance> AnyOf => _anyOf;
        public List<CheckInstance> AllOf => _allOf;

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

        private bool _hideAnyOf
        {
            get
            {
                if (_allOfCount > 0)
                {
                    _anyOf = new List<CheckInstance>();
                    return true;
                }

                return false;
            }
        }

        private bool _hideAllOf
        {
            get
            {
                if (_anyOfCount > 0)
                {
                    _allOf = new List<CheckInstance>();
                    return true;
                }
                return false;
            }
        }

        #endregion

        #region Callbacks

        private void OnEnable()
        {
            Initialise();

            _isInitialised = true;
        }

        private void OnDisable()
        {
            _anyOf = null;
            _allOf = null;

            _isInitialised = false;
        }

        #endregion

        #region API

        public void MakeChecks(List<Character> rearCharacters)
        {
            foreach (Character character in rearCharacters)
            {
                foreach (CheckInstance checkInstance in _anyOf)
                {
                    checkInstance.MakeCheck(character);
                }
                foreach (CheckInstance checkInstance in _allOf)
                {
                    checkInstance.MakeCheck(character);
                }
            }
        }

        public bool GetIsWon()
        {
            bool anyOfCompleted = false;
            if (_anyOfCount > 0)
            {
                foreach (CheckInstance checkInstance in _anyOf)
                {
                    if (checkInstance.IsCompleted())
                    {
                        anyOfCompleted = true;
                        break;
                    }
                }
            } 
            else
            {
                anyOfCompleted = true;
            }

            bool allOfCompleted = true;
            if (_allOfCount > 0)
            {
                foreach (CheckInstance checkInstance in _allOf)
                {
                    if (!checkInstance.IsCompleted())
                    {
                        anyOfCompleted = false;
                        break;
                    }
                }
            }

            return anyOfCompleted && allOfCompleted;
        }

        public List<CheckInstance> GetAllCheckInstances()
        {
            if (!_isInitialised)
            {
                Initialise();
            }

            List<CheckInstance> all = new List<CheckInstance>();

            if (_allOf != null)
            {
                all.AddRange(_allOf);
            }
            if(_anyOf != null)
            {
                all.AddRange(_anyOf);
            }
           
            return all;
        }

        #endregion

        #region Utilities

        private void Initialise()
        {
            if (_isInitialised)
            {
                return;
            }
            _anyOf = new List<CheckInstance>();
            foreach (Check check in _anyOfObjectRef)
            {
                _anyOf.Add(new CheckInstance(check));
            }
            _allOf = new List<CheckInstance>();
            foreach (Check check in _allOfObjectRef)
            {
                _allOf.Add(new CheckInstance(check));
            }
        }

        #endregion
    }
}
