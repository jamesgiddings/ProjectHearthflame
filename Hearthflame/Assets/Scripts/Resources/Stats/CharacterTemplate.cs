using UnityEngine;
using System;
using UnityEditor;


namespace GramophoneUtils.Stats
{
   [CreateAssetMenu(fileName = "New Character Template", menuName = "Characters/Character Template")]
    public class CharacterTemplate : Resource
    {
        [SerializeField] private StatTemplate stats;
        [SerializeField] private CharacterClass characterClass;
        [SerializeField] private Color color;
        [SerializeField] private Brain brain;
        [SerializeField] private bool startsUnlocked;

        [SerializeField] private Sprite portrait;

        [SerializeField] private int startingLevel = 0;


        [SerializeField] private AnimationClip idle_Left;
        [SerializeField] private AnimationClip idle_Right;
        [SerializeField] private AnimationClip idle_Up;
        [SerializeField] private AnimationClip idle_Down;
        [SerializeField] private AnimationClip walk_Left;
        [SerializeField] private AnimationClip walk_Right;
        [SerializeField] private AnimationClip walk_Up;
        [SerializeField] private AnimationClip walk_Down;
        [SerializeField] private AnimationClip attack_Left;
        [SerializeField] private AnimationClip attack_Right;
        [SerializeField] private AnimationClip attack_Up;
        [SerializeField] private AnimationClip attack_Down;
        [SerializeField] private AnimationClip cast_Left;
        [SerializeField] private AnimationClip cast_Right;
        [SerializeField] private AnimationClip cast_Up;
        [SerializeField] private AnimationClip cast_Down;
        [SerializeField] private AnimationClip die;
        [SerializeField] private AnimationClip dead;
        [SerializeField] private AnimationClip shoot_Left;
        [SerializeField] private AnimationClip shoot_Right;
        [SerializeField] private AnimationClip shoot_Up;
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
        public int StartingLevel => startingLevel; // getter

        public Sprite Portrait => portrait; // getter

        public string AnimControllerPath;
        public string AnimControllerLoadPath;



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

