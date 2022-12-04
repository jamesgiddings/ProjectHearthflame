using UnityEngine;
using System;
using UnityEditor;
using Sirenix.OdinInspector;


namespace GramophoneUtils.Stats
{
   [CreateAssetMenu(fileName = "New Character Template", menuName = "Characters/Character Template")]
    public class CharacterTemplate : Resource
    {
        
        [VerticalGroup("General/Split/Left")]
        [TextArea(3, 5)]
        [SerializeField] private string description;
        [BoxGroup("Class")]
        [SerializeField] private CharacterClass characterClass;

        [BoxGroup("Stats")]
        [SerializeField] private StatTemplate stats;
        [BoxGroup("Stats")]
        [SerializeField] private bool startsUnlocked;
        [BoxGroup("Stats")]
        [SerializeField] private int startingLevel = 0;
        [BoxGroup("Stats")]
        [SerializeField] private GameObject characterPrefab;

        [BoxGroup("AI")]
        [SerializeField] private Brain brain;

        [VerticalGroup("General/Split/Right/Sprites")]
        [PreviewField(60), LabelWidth(50)]
        [SerializeField] private Sprite portrait;

        [VerticalGroup("General/Split/Left")]
        [SerializeField] private Color color;
        
        [FoldoutGroup("Animation")]
        public string AnimControllerPath;
        [FoldoutGroup("Animation")]
        public string AnimControllerLoadPath;

        [FoldoutGroup("Animation/Animation Clips")]
        [SerializeField] private AnimationClip idle_Left;
        [FoldoutGroup("Animation/Animation Clips")]
        [SerializeField] private AnimationClip idle_Right;
        [FoldoutGroup("Animation/Animation Clips")]
        [SerializeField] private AnimationClip idle_Up;
        [FoldoutGroup("Animation/Animation Clips")]
        [SerializeField] private AnimationClip idle_Down;
        [FoldoutGroup("Animation/Animation Clips")]
        [SerializeField] private AnimationClip walk_Left;
        [FoldoutGroup("Animation/Animation Clips")]
        [SerializeField] private AnimationClip walk_Right;
        [FoldoutGroup("Animation/Animation Clips")]
        [SerializeField] private AnimationClip walk_Up;
        [FoldoutGroup("Animation/Animation Clips")]
        [SerializeField] private AnimationClip walk_Down;
        [FoldoutGroup("Animation/Animation Clips")]
        [SerializeField] private AnimationClip attack_Left;
        [FoldoutGroup("Animation/Animation Clips")]
        [SerializeField] private AnimationClip attack_Right;
        [FoldoutGroup("Animation/Animation Clips")]
        [SerializeField] private AnimationClip attack_Up;
        [FoldoutGroup("Animation/Animation Clips")]
        [SerializeField] private AnimationClip attack_Down;
        [FoldoutGroup("Animation/Animation Clips")]
        [SerializeField] private AnimationClip cast_Left;
        [FoldoutGroup("Animation/Animation Clips")]
        [SerializeField] private AnimationClip cast_Right;
        [FoldoutGroup("Animation/Animation Clips")]
        [SerializeField] private AnimationClip cast_Up;
        [FoldoutGroup("Animation/Animation Clips")]
        [SerializeField] private AnimationClip cast_Down;
        [FoldoutGroup("Animation/Animation Clips")]
        [SerializeField] private AnimationClip die;
        [FoldoutGroup("Animation/Animation Clips")]
        [SerializeField] private AnimationClip dead;
        [FoldoutGroup("Animation/Animation Clips")]
        [SerializeField] private AnimationClip shoot_Left;
        [FoldoutGroup("Animation/Animation Clips")]
        [SerializeField] private AnimationClip shoot_Right;
        [FoldoutGroup("Animation/Animation Clips")]
        [SerializeField] private AnimationClip shoot_Up;
        [FoldoutGroup("Animation/Animation Clips")]
        [SerializeField] private AnimationClip shoot_Down;

        public AnimationClip Walk_Left => walk_Left;
        public AnimationClip Walk_Right => walk_Right;
        public AnimationClip Walk_Up => walk_Up;
        public AnimationClip Walk_Down => walk_Down;
        public AnimationClip Attack_Left => attack_Left;
        public AnimationClip Attack_Right => attack_Right;
        public AnimationClip Attack_Up => attack_Up;
        public AnimationClip Attack_Down => attack_Down;
        public AnimationClip Cast_Left => cast_Left;
        public AnimationClip Cast_Right => cast_Right;
        public AnimationClip Cast_Up => cast_Up;
        public AnimationClip Cast_Down => cast_Down;
        public AnimationClip Die => die;

        public StatTemplate Stats => stats; //getter
        public CharacterClass CharacterClass => characterClass; //getter
        public Color Color => color; //getter
        public Brain Brain => brain; // getter
        public bool StartsUnlocked => startsUnlocked; // getter
        public string Description => description; // getter

        public GameObject CharacterObject => characterPrefab; // getter
        public int StartingLevel => startingLevel; // getter

        
        public Sprite Portrait => portrait; // getter





#if UNITY_EDITOR

        private UnityEditor.Animations.AnimatorController animatorController;
        public UnityEditor.Animations.AnimatorController AnimatorController => animatorController; //getter

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

        public void CreateController()
        {
            // Creates the controller
            animatorController = UnityEditor.Animations.AnimatorController.CreateAnimatorControllerAtPath(AnimControllerPath);


            if (walk_Left != null)
			{
                UnityEditor.Animations.AnimatorState idleLeft = animatorController.AddMotion(idle_Left);
                
                animatorController.AddMotion(idle_Right);
                animatorController.AddMotion(idle_Up);
                animatorController.AddMotion(idle_Down);
                animatorController.AddMotion(walk_Left);
                animatorController.AddMotion(walk_Right);
                animatorController.AddMotion(walk_Up);
                animatorController.AddMotion(walk_Down);
                animatorController.AddMotion(attack_Left);
                animatorController.AddMotion(attack_Right);
                animatorController.AddMotion(attack_Up);
                animatorController.AddMotion(attack_Down);
                animatorController.AddMotion(cast_Left);
                animatorController.AddMotion(cast_Right);
                animatorController.AddMotion(cast_Up);
                animatorController.AddMotion(cast_Down);
                animatorController.AddMotion(die);
                animatorController.AddMotion(dead);
                animatorController.AddMotion(shoot_Left);
                animatorController.AddMotion(shoot_Right);
                animatorController.AddMotion(shoot_Up);
                animatorController.AddMotion(shoot_Down);

                //AnimatorState castLeft = animatorController.AddMotion(cast_Left);
                //castLeft.AddTransition(idleLeft);
                //AnimatorState castRight = animatorController.AddMotion(cast_Right);
                //castLeft.AddTransition(idleLeft);
                //AnimatorState castUp = animatorController.AddMotion(cast_Up);
                //castLeft.AddTransition(idleLeft);
                //AnimatorState castDown = animatorController.AddMotion(cast_Down);
                //castLeft.AddTransition(idleLeft);

                //UnityEditor.Animations.AnimatorStateMachine asm = animatorController.layers[0].stateMachine;
                //            asm.defaultState = idleLeft;


            }
        }
#endif
    }
}

