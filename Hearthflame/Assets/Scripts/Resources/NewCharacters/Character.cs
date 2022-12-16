using GramophoneUtils.Items.Containers;
using GramophoneUtils.Stats;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace GramophoneUtils.Actors
{
    [CreateAssetMenu(fileName = "New Character", menuName = "Characters/Character")]
    public class Character : InstantiableResource<Character>, IInstantiable<Character>
    {
        #region Attributes/Fields

        [VerticalGroup("General/Split/Left")]
        [TextArea(3, 5)]
        [SerializeField] private string _description;
        [BoxGroup("Class")]
        [SerializeField] private CharacterClass _characterClass;

        [BoxGroup("Stats")]
        [SerializeField] private StatTemplate _stats;
        [BoxGroup("Stats")]
        [SerializeField] private bool _startsUnlocked;
        [BoxGroup("Stats")]
        [SerializeField] private int _startingLevel = 0;
        [BoxGroup("Stats")]
        [SerializeField] private GameObject _characterPrefab;

        [BoxGroup("AI")]
        [SerializeField] private Brain _brain;

        [VerticalGroup("General/Split/Right/Sprites")]
        [PreviewField(60), LabelWidth(50)]
        [SerializeField] private Sprite _portrait;

        [VerticalGroup("General/Split/Left")]
        [SerializeField] private Color _color;

        private readonly StatSystem statSystem;
        private readonly HealthSystem healthSystem;
        private readonly LevelSystem levelSystem;
        private readonly CharacterClass characterClass;
        private readonly SkillSystem skillSystem;
        private EquipmentInventory _equipmentInventory;

        private bool isRear;
        private bool isPlayer;
        private bool isUnlocked;
        private bool isCurrentActor = false;

        private readonly Inventory partyInventory;

        private readonly CharacterTemplate characterTemplate;

        private readonly Brain brain;

        private Queue<BattlerNotificationImpl> notificationQueue = new Queue<BattlerNotificationImpl>();

        public readonly Dictionary<string, IStatType> StatTypeStringRefDictionary;
        public string Name => name; //getter
        public StatSystem StatSystem => statSystem; //getter
        public HealthSystem HealthSystem => healthSystem; //getter
        public LevelSystem LevelSystem => levelSystem; //getter
        public CharacterClass CharacterClass => characterClass; //getter
        public SkillSystem SkillSystem => skillSystem; //getter
        public EquipmentInventory EquipmentInventory => _equipmentInventory; //getter
        public bool IsPlayer { get { return isPlayer; } set { isPlayer = value; } }
        public bool IsRear { get { return isRear; } set { isRear = value; } }
        public bool IsUnlocked { get { return isUnlocked; } set { isUnlocked = value; } }
        public bool IsCurrentActor { get { return isCurrentActor; } }
        public Inventory PartyInventory => partyInventory; //getter
        public CharacterTemplate CharacterTemplate => characterTemplate; //getter
        public Brain Brain => brain; //getter
        public Character() { } //constructor 1

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
        [FoldoutGroup("Animation/Animation Clips")]
        [SerializeField] private AnimationClip _walk_Right;
        [FoldoutGroup("Animation/Animation Clips")]
        [SerializeField] private AnimationClip _walk_Up;
        [FoldoutGroup("Animation/Animation Clips")]
        [SerializeField] private AnimationClip _walk_Down;
        [FoldoutGroup("Animation/Animation Clips")]
        [SerializeField] private AnimationClip _attack_Left;
        [FoldoutGroup("Animation/Animation Clips")]
        [SerializeField] private AnimationClip _attack_Right;
        [FoldoutGroup("Animation/Animation Clips")]
        [SerializeField] private AnimationClip _attack_Up;
        [FoldoutGroup("Animation/Animation Clips")]
        [SerializeField] private AnimationClip _attack_Down;
        [FoldoutGroup("Animation/Animation Clips")]
        [SerializeField] private AnimationClip _cast_Left;
        [FoldoutGroup("Animation/Animation Clips")]
        [SerializeField] private AnimationClip _cast_Right;
        [FoldoutGroup("Animation/Animation Clips")]
        [SerializeField] private AnimationClip _cast_Up;
        [FoldoutGroup("Animation/Animation Clips")]
        [SerializeField] private AnimationClip _cast_Down;
        [FoldoutGroup("Animation/Animation Clips")]
        [SerializeField] private AnimationClip _die;
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

        public AnimationClip Walk_Left => _walk_Left;
        public AnimationClip Walk_Right => _walk_Right;
        public AnimationClip Walk_Up => _walk_Up;
        public AnimationClip Walk_Down => _walk_Down;
        public AnimationClip Attack_Left => _attack_Left;
        public AnimationClip Attack_Right => _attack_Right;
        public AnimationClip Attack_Up => _attack_Up;
        public AnimationClip Attack_Down => _attack_Down;
        public AnimationClip Cast_Left => _cast_Left;
        public AnimationClip Cast_Right => _cast_Right;
        public AnimationClip Cast_Up => _cast_Up;
        public AnimationClip Cast_Down => _cast_Down;
        public AnimationClip Die => _die;
        public StatTemplate Stats => _stats; //getter
        //public CharacterClass CharacterClass => _characterClass; //getter
        public Color Color => _color; //getter
        //public Brain Brain => _brain; // getter
        public bool StartsUnlocked => _startsUnlocked; // getter
        public string Description => _description; // getter
        public GameObject CharacterObject => _characterPrefab; // getter
        public int StartingLevel => _startingLevel; // getter

        public Sprite Portrait => _portrait; // getter

        #endregion

        #region Callbacks

#if UNITY_EDITOR
        public void OnValidate()
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
#endif

        #endregion

        #region API

        /*public Actor(CharacterTemplate characterTemplate, Inventory partyInventory) //constructor 2
        {
            //this.partyCharacterTemplate = partyCharacterTemplate;
            this.characterTemplate = characterTemplate;
            name = characterTemplate.Name;
            statSystem = new StatSystem(characterTemplate, this);
            StatTypeStringRefDictionary = statSystem.StatTypeStringRefDictionary;
            characterClass = characterTemplate.CharacterClass;
            healthSystem = new HealthSystem(characterClass);

            healthSystem.OnHealthChangedNotification += EnqueueBattlerNotification; // TODO UNSUBSCRIBE
            statSystem.OnStatSystemNotification += EnqueueBattlerNotification; // TODO UNSUBSCRIBE

            // subscribe to OnDeathEvent here? also, inject a reference to Character if needed

            this.skillSystem = new SkillSystem(this);
            this.levelSystem = new LevelSystem(characterClass, this);
            levelSystem.OnLevelChanged += characterClass.LevelUp; // TODO UNSUBSCRIBE
            _equipmentInventory = new EquipmentInventory(this);
            _equipmentInventory.onInventoryItemsUpdated = partyInventory.onInventoryItemsUpdated;
            this.partyInventory = partyInventory;

            this.brain = CharacterTemplate.Brain;
            this.IsUnlocked = CharacterTemplate.StartsUnlocked;
            skillSystem.Initialise();
            if (brain != null)
            {
                brain.Initialise(this);
            }
        }*/

        public override Character Instance(Character instantiable)
        {
            throw new NotImplementedException();
        }


        public BattlerNotificationImpl DequeueBattlerNoticiation()
        {
            return notificationQueue.Dequeue();
        }

        public bool GetIsAnyNotificationInQueue()
        {
            if (notificationQueue.Count > 0)
            {
                return true;
            }
            return false;
        }

        public void SetIsCurrentActor(bool value)
        {
            isCurrentActor = value;
        }

        public TargetAreaFlag GetTargetAreaFlag(bool IsOriginatorPlayer)
        {
            TargetAreaFlag characaterTargetAreaFlag = 0;
            switch (IsOriginatorPlayer)
            {
                case true:
                    if (isPlayer && !isRear)
                    {
                        characaterTargetAreaFlag += (int)TargetAreaFlag.AllyFront;
                    }
                    else if (isPlayer && isRear)
                    {
                        characaterTargetAreaFlag += (int)TargetAreaFlag.AllyRear;
                    }
                    else if (!isPlayer && !isRear)
                    {
                        characaterTargetAreaFlag += (int)TargetAreaFlag.OpponentFront;
                    }
                    else if (!isPlayer && isRear)
                    {
                        characaterTargetAreaFlag += (int)TargetAreaFlag.OpponentRear;
                    }
                    break;
                case false:
                    if (isPlayer && !isRear)
                    {
                        characaterTargetAreaFlag += (int)TargetAreaFlag.OpponentFront;
                    }
                    else if (isPlayer && isRear)
                    {
                        characaterTargetAreaFlag += (int)TargetAreaFlag.OpponentRear;
                    }
                    else if (!isPlayer && !isRear)
                    {
                        characaterTargetAreaFlag += (int)TargetAreaFlag.AllyFront;
                    }
                    else if (!isPlayer && isRear)
                    {
                        characaterTargetAreaFlag += (int)TargetAreaFlag.AllyRear;
                    }
                    break;
            }
            return characaterTargetAreaFlag;
        }

        public TargetTypeFlag GetTargetTypeFlag()
        {
            if (healthSystem.IsDead)
            {
                return TargetTypeFlag.Dead;
            }
            else
            {
                return TargetTypeFlag.Alive;
            }
        }

        #endregion

        #region Utilities

        private void EnqueueBattlerNotification(BattlerNotificationImpl notification)
        {
            notificationQueue.Enqueue(notification);
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

#endif

        #endregion
    }
}