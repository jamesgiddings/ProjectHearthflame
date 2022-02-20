using UnityEngine;
using UnityEditor;

public class ExampleWindow : EditorWindow
{
	string myString = "Hello, World!";
	
	[MenuItem("Window/Example")]
	public static void ShowWindow()
	{
		GetWindow<ExampleWindow>("Example");
	}

	private void OnGUI()
	{
		// Window Code
		GUILayout.Label("This is a label.", EditorStyles.boldLabel);

		myString = EditorGUILayout.TextField("Name", myString);

		if (GUILayout.Button("Press Me"))
		{
			Debug.Log("Mutton was pressed.");

			myString = "Mutton";
		}
	}

	//public class MakeScriptableObject
	//{
	//	[MenuItem("Assets/Create/My Scriptable Object")]
	//	public static void CreateMyAsset()
	//	{
	//		MyScriptableObjectClass asset = ScriptableObject.CreateInstance<MyScriptableObjectClass>();

	//		string name = UnityEditor.AssetDatabase.GenerateUniqueAssetPath("Assets/NewScripableObject.asset");
	//		AssetDatabase.CreateAsset(asset, name);
	//		AssetDatabase.SaveAssets();

	//		EditorUtility.FocusProjectWindow();

	//		Selection.activeObject = asset;
	//	}
	//}

}
