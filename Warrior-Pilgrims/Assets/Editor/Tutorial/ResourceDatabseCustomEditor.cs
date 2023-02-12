using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;

public class AssetHandler
{
    [OnOpenAsset()]
    public static bool OpenEditor(int instanceID, int line)
    {
        ResourceDatabase obj = EditorUtility.InstanceIDToObject(instanceID) as ResourceDatabase;
        if (obj != null)
        {
            ResourceDatabaseEditorWindow.Open(obj);
            return true;
        }
        return false;
    }
}


//[CustomEditor(typeof(ResourceDatabase))]
public class ResourceDatabseCustomEditor : Editor
{
    public override void OnInspectorGUI()
    {
        if (GUILayout.Button("Open Editor"))
        {
            ResourceDatabaseEditorWindow.Open((ResourceDatabase)target);
        }
    }
}
