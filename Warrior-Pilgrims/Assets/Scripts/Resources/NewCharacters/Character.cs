using AYellowpaper;
using GramophoneUtils.Items.Containers;
using GramophoneUtils.Stats;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace GramophoneUtils.Characters
{
    [CreateAssetMenu(fileName = "New Character", menuName = "Characters/Character")]
    public class Character : InstantiableResource<Character>, IInstantiable<Character>
    {
        #region Attributes/Fields

        [VerticalGroup("General/Split/Left")]
        [TextArea(3, 5)]
        [SerializeField] private string _description;
        public string Description => _description;

        [BoxGroup("Class")]
        [SerializeField] private CharacterClass _characterClass;
        public CharacterClass CharacterClass => _characterClass;

        [BoxGroup("Stats")]
        [SerializeField] private StatTemplate _statTemplate;
        public StatTemplate Stats => _statTemplate;

        [BoxGroup("Stats")]
        [SerializeField] private bool _startsUnlocked;
        public bool StartsUnlocked => _startsUnlocked;

        [BoxGroup("Stats")]
        [SerializeField] private int _startingLevel = 0;
        public int StartingLevel => _startingLevel;

        [BoxGroup("Stats")]
        [SerializeField] private GameObject _characterPrefab;
        public GameObject CharacterPrefab => _characterPrefab;

        [BoxGroup("AI")]
        [SerializeField] InterfaceReference<IBrain> _brain;
        public IBrain Brain => _brain.Value;

        [VerticalGroup("General/Split/Right/Sprites")]
        [PreviewField(60), LabelWidth(50)]
        [SerializeField] private Sprite _portrait;
        public Sprite Portrait => _portrait;

        [VerticalGroup("General/Split/Left")]
        [SerializeField] private Color _color;
        public Color Color => _color;

        private StatSystem _statSystem;
        public StatSystem StatSystem => _statSystem;

        private HealthSystem _healthSystem;
        public HealthSystem HealthSystem => _healthSystem;

        private LevelSystem _levelSystem;
        public LevelSystem LevelSystem => _levelSystem;

        private SkillSystem _skillSystem;
        public SkillSystem SkillSystem => _skillSystem;

        [SerializeField] private EquipmentInventory _equipmentInventory;
        public EquipmentInventory EquipmentInventory
        {
            get
            {
                return _equipmentInventory;
            }
            set
            {
                _equipmentInventory = value;
            }
        }

        private bool _isRear;
        public bool IsRear { get { return _isRear; } set { _isRear = value; } }

        [VerticalGroup("General/Split/Left")]
        [SerializeField] private bool _isPlayer;
        public bool IsPlayer { get { return _isPlayer; } set { _isPlayer = value; } }

        private bool _isUnlocked;
        public bool IsUnlocked { get { return _isUnlocked; } set { _isUnlocked = value; } }

        private bool _isCurrentActor = false;
        public bool IsCurrentActor { get { return _isCurrentActor; } }

        private Inventory _partyInventory;
        public Inventory PartyInventory { get { return _partyInventory; } set { _partyInventory = value; } } // TODO hack, find a better place to set the partyInventory 

        private Queue<BattlerNotificationImpl> _notificationQueue = new Queue<BattlerNotificationImpl>();

        public Dictionary<string, IStatType> StatTypeStringRefDictionary;

        public Action OnCharacterTurnElapsed;

#if UNITY_EDITOR
        private UnityEditor.Animations.AnimatorController animatorController;
        public UnityEditor.Animations.AnimatorController AnimatorController => animatorController; //getter
#endif

        [FoldoutGroup("Animation")]
        public string AnimControllerPath;

        [FoldoutGroup("Animation")]
        public string AnimControllerLoadPath;

        [FoldoutGroup("Animation/Animation Clips")]
        [SerializeField] private AnimationClip _idle_Left;

        [FoldoutGroup("Animation/Animation Clips")]
        [SerializeField] private AnimationClip _idle_Right;

        [FoldoutGroup("Animation/Animation Clips")]
        [SerializeField] private AnimationClip _idle_Up;

        [FoldoutGroup("Animation/Animation Clips")]
        [SerializeField] private AnimationClip _idle_Down;

        [FoldoutGroup("Animation/Animation Clips")]
        [SerializeField] private AnimationClip _walk_Left;

        public AnimationClip Walk_Left => _walk_Left;

        [FoldoutGroup("Animation/Animation Clips")]
        [SerializeField] private AnimationClip _walk_Right;
        public AnimationClip Walk_Right => _walk_Right;

        [FoldoutGroup("Animation/Animation Clips")]
        [SerializeField] private AnimationClip _walk_Up;
        public AnimationClip Walk_Up => _walk_Up;

        [FoldoutGroup("Animation/Animation Clips")]
        [SerializeField] private AnimationClip _walk_Down;
        public AnimationClip Walk_Down => _walk_Down;

        [FoldoutGroup("Animation/Animation Clips")]
        [SerializeField] private AnimationClip _attack_Left;
        public AnimationClip Attack_Left => _attack_Left;

        [FoldoutGroup("Animation/Animation Clips")]
        [SerializeField] private AnimationClip _attack_Right;
        public AnimationClip Attack_Right => _attack_Right;

        [FoldoutGroup("Animation/Animation Clips")]
        [SerializeField] private AnimationClip _attack_Up;
        public AnimationClip Attack_Up => _attack_Up;

        [FoldoutGroup("Animation/Animation Clips")]
        [SerializeField] private AnimationClip _attack_Down;
        public AnimationClip Attack_Down => _attack_Down;

        [FoldoutGroup("Animation/Animation Clips")]
        [SerializeField] private AnimationClip _cast_Left;
        public AnimationClip Cast_Left => _cast_Left;

        [FoldoutGroup("Animation/Animation Clips")]
        [SerializeField] private AnimationClip _cast_Right;
        public AnimationClip Cast_Right => _cast_Right;

        [FoldoutGroup("Animation/Animation Clips")]
        [SerializeField] private AnimationClip _cast_Up;
        public AnimationClip Cast_Up => _cast_Up;

        [FoldoutGroup("Animation/Animation Clips")]
        [SerializeField] private AnimationClip _cast_Down;
        public AnimationClip Cast_Down => _cast_Down;

        [FoldoutGroup("Animation/Animation Clips")]
        [SerializeField] private AnimationClip _die;
        public AnimationClip Die => _die;

        [FoldoutGroup("Animation/Animation Clips")]
        [SerializeField] private AnimationClip _dead;

        [FoldoutGroup("Animation/Animation Clips")]
        [SerializeField] private AnimationClip _shoot_Left;

        [FoldoutGroup("Animation/Animation Clips")]
        [SerializeField] private AnimationClip _shoot_Right;

        [FoldoutGroup("Animation/Animation Clips")]
        [SerializeField] private AnimationClip _shoot_Up;

        [FoldoutGroup("Animation/Animation Clips")]
        [SerializeField] private AnimationClip _shoot_Down;
        
        #endregion

        #region Constructors

        private Character() { }

        #endregion

        #region Callbacks


#if UNITY_EDITOR
        private void OnValidate()
        {
            AnimControllerPath = "Assets/Resources/Animations/AnimationControllers/" + Name + "AnimationController.asset";
            AnimControllerLoadPath = "Animations/AnimationControllers/" + Name + "AnimationController";

            var _exists = AssetDatabase.LoadAssetAtPath(AnimControllerPath, typeof(UnityEditor.Animations.AnimatorController));

            if (animatorController == null)
            {
                if (_exists != null)
                {
                    animatorController = (UnityEditor.Animations.AnimatorController)_exists;
                }
                else
                {
                    CreateController();
                }
            }
        }

        private void OnDisable()
        {
            if (_healthSystem != null)
            {
                _healthSystem.OnHealthChangedNotification = null;
            }
            if (_statSystem != null)
            {
                _statSystem.OnStatSystemNotification = null;
            }            
        }

#endif

        #endregion

        #region Public Functions

        public override Character Instance()
        {
            Character instancedCharacter = CreateInstance<Character>();

            InitialiseInstancedCharacter(instancedCharacter);

            return instancedCharacter;
        }

        public BattlerNotificationImpl DequeueBattlerNoticiation()
        {
            return _notificationQueue.Dequeue();
        }

        public bool GetIsAnyNotificationInQueue()
        {
            if (_notificationQueue.Count > 0)
            {
                return true;
            }
            return false;
        }

        public void SetIsCurrentActor(bool value)
        {
            _isCurrentActor = value;
        }

        public TargetAreaFlag GetTargetAreaFlag(bool IsOriginatorPlayer)
        {
            TargetAreaFlag characaterTargetAreaFlag = 0;
            switch (IsOriginatorPlayer)
            {
                case true:
                    if (_isPlayer && !_isRear)
                    {
                        characaterTargetAreaFlag += (int)TargetAreaFlag.AllyFront;
                    }
                    else if (_isPlayer && _isRear)
                    {
                        characaterTargetAreaFlag += (int)TargetAreaFlag.AllyRear;
                    }
                    else if (!_isPlayer && !_isRear)
                    {
                        characaterTargetAreaFlag += (int)TargetAreaFlag.OpponentFront;
                    }
                    else if (!_isPlayer && _isRear)
                    {
                        characaterTargetAreaFlag += (int)TargetAreaFlag.OpponentRear;
                    }
                    break;
                case false:
                    if (_isPlayer && !_isRear)
                    {
                        characaterTargetAreaFlag += (int)TargetAreaFlag.OpponentFront;
                    }
                    else if (_isPlayer && _isRear)
                    {
                        characaterTargetAreaFlag += (int)TargetAreaFlag.OpponentRear;
                    }
                    else if (!_isPlayer && !_isRear)
                    {
                        characaterTargetAreaFlag += (int)TargetAreaFlag.AllyFront;
                    }
                    else if (!_isPlayer && _isRear)
                    {
                        characaterTargetAreaFlag += (int)TargetAreaFlag.AllyRear;
                    }
                    break;
            }
            return characaterTargetAreaFlag;
        }

        public TargetTypeFlag GetTargetTypeFlag()
        {
            if (_healthSystem.IsDead)
            {
                return TargetTypeFlag.Dead;
            }
            else
            {
                return TargetTypeFlag.Alive;
            }
        }

        /// <summary>
        /// Unlike the global Turn class, this offers the same functionality, but at a character level.
        /// This is advanced during battle, the Turn class is advanced outside of battle.
        /// </summary>
        public void AdvanceCharacterTurn()
        {
            OnCharacterTurnElapsed?.Invoke();
        }

#if UNITY_EDITOR
        public void CreateController()
        {
            // Creates the controller
            animatorController = UnityEditor.Animations.AnimatorController.CreateAnimatorControllerAtPath(AnimControllerPath);

            if (_walk_Left != null)
            {
                UnityEditor.Animations.AnimatorState idleLeft = animatorController.AddMotion(_idle_Left);

                animatorController.AddMotion(_idle_Right);
                animatorController.AddMotion(_idle_Up);
                animatorController.AddMotion(_idle_Down);
                animatorController.AddMotion(_walk_Left);
                animatorController.AddMotion(_walk_Right);
                animatorController.AddMotion(_walk_Up);
                animatorController.AddMotion(_walk_Down);
                animatorController.AddMotion(_attack_Left);
                animatorController.AddMotion(_attack_Right);
                animatorController.AddMotion(_attack_Up);
                animatorController.AddMotion(_attack_Down);
                animatorController.AddMotion(_cast_Left);
                animatorController.AddMotion(_cast_Right);
                animatorController.AddMotion(_cast_Up);
                animatorController.AddMotion(_cast_Down);
                animatorController.AddMotion(_die);
                animatorController.AddMotion(_dead);
                animatorController.AddMotion(_shoot_Left);
                animatorController.AddMotion(_shoot_Right);
                animatorController.AddMotion(_shoot_Up);
                animatorController.AddMotion(_shoot_Down);
            }
        }

        [Button("Create Random Brain")]
        public void CreateRandomBrain()
        {
            RandomBrain randomBrain = CreateInstance(typeof(RandomBrain)) as RandomBrain;
            string directory = _isPlayer ? "Player Characters" : "Enemy Characters";
            string path = "Assets/Resources/Characters/" + directory + "/" + this.Name + "/" + this.Name + "RandomBrain.asset";
            Debug.Log(path);
            string assetPath = AssetDatabase.GenerateUniqueAssetPath("Assets/Resources/Characters/" + directory + "/" + this.Name + "/" + this.Name + "RandomBrain.asset");
            Debug.Log(assetPath);
            AssetDatabase.CreateAsset(randomBrain, assetPath);
            RandomSkillObjectCollection randomSkillCollectionCollection = AssetDatabase.LoadAssetAtPath<RandomSkillObjectCollection>("Assets/Resources/Characters/Classes/Random Skill Object Collections/" + _characterClass.Name + "RandomSkillObjectCollection.asset");
            randomBrain.SetRandomSkillObjectCollection(randomSkillCollectionCollection);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            this._brain.Value = randomBrain;
            this._brain.UnderlyingValue = randomBrain;
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            EditorUtility.SetDirty(randomBrain);
            EditorUtility.SetDirty(this);
        }

        [Button("Create Equipment Inventory")]
        public void CreateEquipmentInventory()
        {
            EquipmentInventory equipmentInventory = CreateInstance(typeof(EquipmentInventory)) as EquipmentInventory;
            string directory = _isPlayer ? "Player Characters" : "Enemy Characters";
            string assetPath = AssetDatabase.GenerateUniqueAssetPath("Assets/Resources/Characters/" + directory + "/" + this.Name + "/" + this.Name + "EquipmentInventory.asset");
            AssetDatabase.CreateAsset(equipmentInventory, assetPath);
            equipmentInventory.Initialise();
            _equipmentInventory = equipmentInventory;
            EditorUtility.SetDirty(equipmentInventory);
            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }


#endif

        #endregion

        #region Private Functions

        private void InitialiseInstancedCharacter(Character instancedCharacter)
        {
            instancedCharacter.name = this.name;
            instancedCharacter._sprite = this._sprite;
            instancedCharacter._portrait = this._portrait;
            instancedCharacter._statTemplate = this._statTemplate;
            instancedCharacter._statSystem = new StatSystem(instancedCharacter);
            instancedCharacter.StatTypeStringRefDictionary = instancedCharacter.StatSystem.StatTypeStringRefDictionary;
            instancedCharacter._characterClass = this._characterClass;
            instancedCharacter._healthSystem = new HealthSystem(this._characterClass);

            instancedCharacter._healthSystem.OnHealthChangedNotification += instancedCharacter.EnqueueBattlerNotification; // TODO UNSUBSCRIBE
            instancedCharacter._statSystem.OnStatSystemNotification += instancedCharacter.EnqueueBattlerNotification; // TODO UNSUBSCRIBE

            // subscribe to OnDeathEvent here? also, inject a reference to Character if needed

            instancedCharacter._skillSystem = new SkillSystem(instancedCharacter);
            instancedCharacter._skillSystem.Initialise();
            instancedCharacter._levelSystem = new LevelSystem(instancedCharacter._characterClass, instancedCharacter);
            instancedCharacter._levelSystem.OnLevelChanged += _characterClass.LevelUp; // TODO UNSUBSCRIBE
            instancedCharacter._equipmentInventory = _equipmentInventory;
            instancedCharacter._equipmentInventory.Initialise(4);
            instancedCharacter._brain = this._brain;
            instancedCharacter.IsUnlocked = this._startsUnlocked;
            instancedCharacter._characterPrefab = this._characterPrefab;
            instancedCharacter.AnimControllerLoadPath = this.AnimControllerLoadPath;
            instancedCharacter.AnimControllerPath = this.AnimControllerPath;

        }

        private void EnqueueBattlerNotification(BattlerNotificationImpl notification)
        {
            _notificationQueue.Enqueue(notification);
        }

        #endregion
    }
}