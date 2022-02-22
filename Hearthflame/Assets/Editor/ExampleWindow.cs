using UnityEngine;
using UnityEditor;
using GramophoneUtils.Stats;

public class ExampleWindow : EditorWindow
{
	string myString = "Hello, World!";
	int tab = 0;
	string[] toolbarStrings = { "Characters", "Items", "Locations" };
	private TestDatabase testDatabase;
	CharacterTemplate characterTemplate = null;
	string path = "";


	//[SerializeField] CharacterTemplate characterTemplate;
	public Object characterTemplateR;

	public Object newlyCreatedTemplate;

	[MenuItem("Window/Example")]
	public static void ShowWindow()
	{
		GetWindow<ExampleWindow>("Example");
	}

	private void OnEnable()
	{
		testDatabase = (TestDatabase)AssetDatabase.LoadAssetAtPath<TestDatabase>("Assets/Resources/Database.asset");
	}


	private void OnGUI()
	{
		// Window Code
		GUILayout.Label("This is a label.", EditorStyles.boldLabel);

		myString = EditorGUILayout.TextField("Name", myString);
		EditorGUILayout.BeginHorizontal();
		characterTemplateR = EditorGUILayout.ObjectField(characterTemplateR, typeof(CharacterTemplate), true);
		EditorGUILayout.EndHorizontal();

		if (GUILayout.Button("Press Me To Add to databse"))
		{
			testDatabase.CharacterTemplates.Add((CharacterTemplate)characterTemplateR);
			EditorUtility.SetDirty(testDatabase);
		}

		switch (tab)
		{
			case 0:
				tab = GUILayout.Toolbar(tab, toolbarStrings);
				string characterName = "";
				characterName = EditorGUILayout.TextField("Character Name", characterName);
				
				Editor myEditor = Editor.CreateEditor(characterTemplate);
				if (characterTemplate != null)
					myEditor.OnInspectorGUI();

				if (characterTemplate == null)
				{
					if (GUILayout.Button("New Character"))
					{
						characterTemplate = ScriptableObject.CreateInstance(typeof(CharacterTemplate)) as CharacterTemplate;

						path = AssetDatabase.GenerateUniqueAssetPath("Assets/Resources/Character.asset");

						AssetDatabase.CreateAsset(characterTemplate, path);
						AssetDatabase.SaveAssets();
						AssetDatabase.Refresh();

						CharacterTemplate loadedAsset = (CharacterTemplate)AssetDatabase.LoadAssetAtPath<CharacterTemplate>(path);
						//testDatabase.CharacterTemplates.Add(loadedAsset);

						newlyCreatedTemplate = EditorGUILayout.ObjectField(newlyCreatedTemplate, typeof(CharacterTemplate), true);
						//EditorUtility.FocusProjectWindow();

						//Selection.activeObject = example;
					}
				}
					

				if (characterTemplate != null)
					if (GUILayout.Button("Add Character to database"))
					{
						testDatabase.CharacterTemplates.Add((CharacterTemplate)characterTemplate);
						EditorUtility.SetDirty(testDatabase);
						if (characterTemplate.Name != "")
						{
							string extension = string.Format("Assets/Resources/{0}.asset", characterTemplate.Name);
							Debug.Log("extension: " + extension);
							Debug.Log(path);

							Debug.Log(AssetDatabase.GenerateUniqueAssetPath(extension));
							string AssetRenameErrorMessage = AssetDatabase.RenameAsset(path, AssetDatabase.GenerateUniqueAssetPath(characterTemplate.Name));
							if (AssetRenameErrorMessage != null)
							{
								Debug.LogError(AssetRenameErrorMessage);
							}
						}


					}
				if (characterTemplate != null)
					if (GUILayout.Button("Delete Character"))
					{
						if (testDatabase.CharacterTemplates.Contains(characterTemplate))
						{
							testDatabase.CharacterTemplates.Remove(characterTemplate);
						}
						if (path != "")
						{
							AssetDatabase.DeleteAsset(path);
						}

						EditorUtility.SetDirty(testDatabase);
						characterTemplate = null;
					}

				//if (GUILayout.Button("Press me to create new character and add to database."))
				//{
				//	CharacterTemplate characterTemplate = ScriptableObject.CreateInstance(typeof(CharacterTemplate)) as CharacterTemplate;
				//	//characterTemplate = (CharacterTemplate)scriptObj;
				//	Debug.Log(characterTemplate.IsPlayer);

				//	Editor m_MyScriptableObjectEditor = Editor.CreateEditor(characterTemplate);
				//	m_MyScriptableObjectEditor.OnInspectorGUI();

				//	testDatabase.CharacterTemplates.Add(characterTemplate);
				//	EditorUtility.SetDirty(testDatabase);
				//}
				break;
			case 1:
				tab = GUILayout.Toolbar(tab, toolbarStrings);
				break;			
			case 2:
				tab = GUILayout.Toolbar(tab, toolbarStrings);
				break;
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
