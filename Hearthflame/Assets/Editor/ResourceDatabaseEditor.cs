#if (UNITY_EDITOR)
using UnityEngine;
using UnityEditor;
using GramophoneUtils.Items.Containers;

[CustomEditor(typeof(Resource), true)]
public class ResourceDatabaseEditor : Editor
{
    ResourceDatabase db;
    private bool isInitiated = false;
    private bool currentToggleValue;
    private bool lastToggleValue;

    public override void OnInspectorGUI()
    {
        var resource = (Resource)target;

        base.OnInspectorGUI();

		if (GUILayout.Button("Print Database"))
		{
			db.Print();
		}

		if (!isInitiated)
        {
            db = (ResourceDatabase)AssetDatabase.LoadAssetAtPath("Assets/Scripts/Resources/Resource Database.asset", typeof(ResourceDatabase));
            currentToggleValue = db.Contains(resource.UID);
            lastToggleValue = currentToggleValue;
            isInitiated = true;
        }

        currentToggleValue = GUILayout.Toggle(currentToggleValue, "Added to Database", GUILayout.Height(20));
        if (lastToggleValue != currentToggleValue)
        {
            if (currentToggleValue)
            {
                db.AddResource(resource);
                EditorUtility.SetDirty(db);
            }
            else
            {
                db.RemoveResource(resource);
                EditorUtility.SetDirty(db);
            }

            lastToggleValue = currentToggleValue;
        }
    }
}
#endif