using AYellowpaper;
using GramophoneUtils.Stats;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Scripting;
using static GramophoneUtils.Stats.LevelSystem;

[Serializable]
[CreateAssetMenu(fileName = "New Character Class", menuName = "Character Classes/Character Class")]
public class CharacterClass : Resource
{
    #region Attributes/Fields/Properties

    [VerticalGroup("General/Split/Left"), TextArea(3, 5)]
    [SerializeField] private string _description;
    public string Description => _description;

    [ProgressBar(0, 300), SerializeField] private int _baseMaxHealth;
    public int BaseMaxHealth => _baseMaxHealth;

    [ProgressBar(0, 300), SerializeField] private int baseHealth;
    public int BaseHealth => baseHealth;

    [ProgressBar(1, 40), SerializeField] private int _maxLevel;
    public int MaxLevel => _maxLevel;

    [SerializeField] private ExperienceData _experienceData;
    public ExperienceData ExperienceData => _experienceData;

    [SerializeField] List<InterfaceReference<ISkill>> _skillsAvailable;
    public List<ISkill> SkillsAvailable
    {
        get
        {
            return _skillsAvailable.Select(x => x.Value).ToList();
        }
    }

    [FoldoutGroup("Level Progression"), TableList(IsReadOnly = false, AlwaysExpanded = true), SerializeField] private List<LevelStatEffect> _levelStatEffects;
    public List<LevelStatEffect> LevelStatEffects => _levelStatEffects;

    [SerializeField] private int _size = 1;
    public int Size => _size;

    #endregion

    #region Constructors
    #endregion

    #region Callbacks
    #endregion

    #region Public Functions

    public void LevelUp(System.Object sender, EventArgs args)
    {
        // do levelling up here
        LevelChangedEventArgs castArgs = args as LevelChangedEventArgs;
        IncrementStatBaseValues(castArgs);
        IncrementHealthBaseValues(castArgs);
    }

#if UNITY_EDITOR

    [Button("Create Random Skill Object Collection")] public void CreateRandomSkillObjectCollection()
    {
        RandomSkillObjectCollection randomSkillObjectCollection = CreateInstance(typeof(RandomSkillObjectCollection)) as RandomSkillObjectCollection;
        string assetPath = AssetDatabase.GenerateUniqueAssetPath("Assets/Resources/Characters/Classes/Random Skill Object Collections/" + this.Name + "RandomSkillObjectCollection.asset");
        AssetDatabase.CreateAsset(randomSkillObjectCollection, assetPath);
        foreach (ISkill skill in SkillsAvailable)
        {
            RandomSkillObject skillAsset = AssetDatabase.LoadAssetAtPath<RandomSkillObject>("Assets/Resources/Skills/Random Skill Objects/" + skill.Name + "RandomSkillObject.asset");
            randomSkillObjectCollection.AddSkillObject(skillAsset);
        }
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        EditorUtility.SetDirty(randomSkillObjectCollection);
    }

    [Button("Create Random Character Class Object")] public void CreateRandomCharacterClassObject()
    {
        string guid = AssetDatabase.FindAssets(this.Name)[0];
        string classPath = AssetDatabase.GUIDToAssetPath(guid);
        Debug.Log(classPath);
        CharacterClass characterClass = AssetDatabase.LoadAssetAtPath<CharacterClass>(classPath);

        RandomCharacterClassObject randomCharacterClassObject = CreateInstance(typeof(RandomCharacterClassObject)) as RandomCharacterClassObject;
        string assetPath = AssetDatabase.GenerateUniqueAssetPath("Assets/Resources/Characters/Classes/Character Class Targeting/" + this.Name + "RandomCharacterClassObject.asset");
        AssetDatabase.CreateAsset(randomCharacterClassObject, assetPath);


        randomCharacterClassObject.WeightedObject = characterClass;
        randomCharacterClassObject.Weighting = 1f;
        EditorUtility.SetDirty(randomCharacterClassObject);
        EditorUtility.SetDirty(characterClass);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

#endif

    #endregion

    #region Protected Functions
    #endregion

    #region Private Functions

    private void IncrementHealthBaseValues(LevelChangedEventArgs castArgs)
    {
        castArgs.Character.HealthSystem.IncrementMaxHealth(_levelStatEffects[castArgs.Level].BaseHealthIncrement);
        castArgs.Character.HealthSystem.IncrementCurrentHealth(_levelStatEffects[castArgs.Level].BaseHealthIncrement);
    }

    private void IncrementStatBaseValues(LevelChangedEventArgs castArgs)
    {
        foreach (StatModifierBlueprint statComponentBlueprint in _levelStatEffects[castArgs.Level].StatComponentBlueprints)
        {
            castArgs.Character.StatSystem.IncrementStatBaseValue(statComponentBlueprint);
        }
    }

    #endregion

    #region Inner Classes

    [Serializable]
    public class LevelStatEffect
    {
        [TableList(AlwaysExpanded = true)]
        [SerializeField] private List<ISkill> skillsUnlocked;
        [SerializeField] private int baseHealthIncrement;
        [TableList(AlwaysExpanded = true)]
        [SerializeField] private List<StatModifierBlueprint> statComponentBlueprints;

        public List<StatModifierBlueprint> StatComponentBlueprints => statComponentBlueprints;
        public int BaseHealthIncrement => baseHealthIncrement;
    }

    #endregion
}
