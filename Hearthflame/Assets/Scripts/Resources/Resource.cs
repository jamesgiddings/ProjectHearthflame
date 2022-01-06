using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "New Test Item", menuName = "Items/Test Item")]
public class Resource : ScriptableObject
{
    [ReadOnly] public string UID;  
    private void OnValidate()
    {
#if UNITY_EDITOR
        if (UID == "")
		{
			UID = GUID.Generate().ToString();
 
            UnityEditor.EditorUtility.SetDirty(this);
        }
#endif
        ResourceDatabase.AddResource(this);
    }
}
