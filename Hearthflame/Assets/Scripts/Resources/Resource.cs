using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "New Test Item", menuName = "Items/Test Item")]
public class Resource : ScriptableObject
{
    [ReadOnly] public string UID;
    [SerializeField] protected Sprite icon;
    [SerializeField] protected new string name;

    public Sprite Icon => icon; //getter
    public string Name => name;

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
