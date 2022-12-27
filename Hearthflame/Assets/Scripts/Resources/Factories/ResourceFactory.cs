using GramophoneUtils.Items;
using GramophoneUtils.Items.Containers;
using GramophoneUtils.Stats;
using Dialogue;
using UnityEditor;
using UnityEngine;
using GramophoneUtils.Characters;

#if UNITY_EDITOR
public class ResourceFactory
{
    public static Resource Create(string typeName)
    {
        Resource resource = null;
        switch (typeName)
        {
            case "ConsumableItem":
                resource = Create<ConsumableItem>();
                return resource;
            case "Character":
                resource = Create<Character>();
                return resource;
            case "CharacterClass":
                resource = Create<CharacterClass>();
                return resource;
            case "DialogueGraph":
                resource = Create<DialogueGraph>();
                return resource;
            case "EquipmentItem":
                resource = Create<EquipmentItem>();
                return resource;
            case "Skill":
                resource = Create<Skill>();
                return resource;
            case "SequenceBrain":
                resource = Create<SequenceBrain>();                
                return resource;
            case "RandomBrain":
                resource = Create<RandomBrain>();
                return resource;
            default:
                break;
        }
        return null;
    }

    public static T Create<T>() where T : Resource
    {
        T resource = ScriptableObject.CreateInstance(typeof(T)) as T;

        string typeName = resource.GetType().Name;
        string path = AssetDatabase.GenerateUniqueAssetPath(ResourceFactoryUtilities.GenerateAssetPath(resource));

        Debug.Log(ResourceFactoryUtilities.GenerateAssetPath(resource));

        AssetDatabase.CreateAsset(resource, path);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        T loadedAsset = (T)AssetDatabase.LoadAssetAtPath<T>(path);

        

        return resource;
    }
}
#endif