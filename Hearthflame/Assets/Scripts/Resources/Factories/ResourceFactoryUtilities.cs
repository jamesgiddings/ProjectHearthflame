using System;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
public class ResourceFactoryUtilities
{
    public static string GenerateAssetPath(Resource resource, string name = "")
    {
        string path;

        StringBuilder stringBuilderPath = new StringBuilder("Assets/Resources"); // start the stringBuilder path with the proper location for unity AssetDatabase resources

        Type type = resource.GetType(); // get the type of the resource i.e. EquipmentItem
        StringBuilder stringBuilderPathSuffix = new StringBuilder(); // create a new stringBuilder to hold the suffix while we build it
        
        if (name == "")
        {
            stringBuilderPathSuffix.Append(type.Name); // add the type to the builder (this will be what the AssetDatabase uses to generate a meaningful unique name if no name is passed as an argument
        }
        else
        {
            stringBuilderPathSuffix.Append(name); // else use the name passed
        }
            
        stringBuilderPathSuffix.Append(".asset"); // add the type to the builder
                
        stringBuilderPathSuffix.Insert(0, "/"); // prepend it with a slash
        stringBuilderPathSuffix.Insert(0, type.Name); // prepend it with the type again this time to determine the folder 
        stringBuilderPathSuffix.Insert(0, "/"); // prepend it with a slash

        while (type.BaseType.Name != "Resource") // loop until we get beack to Resource
        {
            type = type.BaseType; // move backwards through the chain

            stringBuilderPathSuffix.Insert(0, type.Name); // prepend the new type
            stringBuilderPathSuffix.Insert(0, "/"); // prepend slash
        }

        stringBuilderPath.Append(stringBuilderPathSuffix); // join the two string builders
        path = stringBuilderPath.ToString(); // convert to string before returning
        
        return path;
    }

    public static string GetResourceAssetPath(Resource resource)
    {
        string filter = "t:" + resource.GetType().Name;
        string[] guids = AssetDatabase.FindAssets(filter, new[] {"Assets/Resources"});
        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            if (AssetDatabase.LoadAssetAtPath<Resource>(path).UID == resource.UID)
            {
                return path;
            }
        }
        return null;
    }

    public static void DeleteResourceAsset(Resource resource)
    {
        AssetDatabase.DeleteAsset(GetResourceAssetPath(resource));
    }
}
#endif