using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using GramophoneUtils.Stats;
using GramophoneUtils.Items.Containers;

public class ResourceDatabaseEditorWindow : ExtendedEditorWindow
{
    public static ResourceDatabase _resourceDatabaseObject;

    private IEnumerable<System.Type> resourceTypes;
    private List<System.Type> resTypes;
    private string[] resTypeStrings;

    string searchText = "";
    string searchTextField = "";

    static int selected;

    private Vector2 scrollPos = Vector2.zero;
    float currentScrollViewHeight;
    bool resize = false;
    Rect cursorChangeRect;
    private enum tabs
    {
        Characters, Items, Dialogue, Battle, Skills, Stats
    }

    private tabs tab;

    string[] toolbarStrings = { "Characters", "Items", "Dialogue", "Battle", "Skills", "Stats"};
    CharacterTemplate characterTemplate = null;
    EquipmentItem equipmentItem = null;
    string path = "";


    private Object newlyCreatedResource;
    private Resource resource = null;

    [MenuItem("Window/Game Database")]
    public static void ShowWindow()
    {
        Open((ResourceDatabase)AssetDatabase.LoadAssetAtPath<ResourceDatabase>("Assets/Resources/Resource Database.asset"));
    }

    public static void Open(ResourceDatabase resourceDatabaseObject)
    {
        ResourceDatabaseEditorWindow window = GetWindow<ResourceDatabaseEditorWindow>("Game Database");
        window.serializedObject = new SerializedObject(resourceDatabaseObject);
        _resourceDatabaseObject = resourceDatabaseObject;
    }

    private void OnEnable()
    {
        CreateFilterDropdownList();
        this.position = new Rect(200, 200, 400, 300);
        currentScrollViewHeight = this.position.height / 2;
        cursorChangeRect = new Rect(0, currentScrollViewHeight, this.position.width, 5f);
    }

    private void CreateFilterDropdownList()
    {
        resourceTypes = ReflectiveEnumerator.GetEnumerableOfType<Resource>();
        resTypes = new List<System.Type>((IEnumerable<System.Type>)resourceTypes);
        resTypes.Add(typeof(Resource));
        resTypeStrings = new string[resTypes.Count + 1];
        for (int i = 0; i < resTypes.Count; i++)
        {
            resTypeStrings[i] = resTypes[i].Name;
        }
        resTypeStrings[resTypeStrings.Length - 1] = "All";
        selected = resTypeStrings.Length - 1;
    }

    private void ResizeScrollView()
    {
        GUI.DrawTexture(cursorChangeRect, EditorGUIUtility.whiteTexture);
        EditorGUIUtility.AddCursorRect(cursorChangeRect, MouseCursor.ResizeVertical);

        if (Event.current.type == EventType.MouseDown && cursorChangeRect.Contains(Event.current.mousePosition))
        {
            resize = true;
        }
        if (resize)
        {
            currentScrollViewHeight = Event.current.mousePosition.y;
            cursorChangeRect.Set(cursorChangeRect.x, currentScrollViewHeight, cursorChangeRect.width, cursorChangeRect.height);
        }
        if (Event.current.type == EventType.MouseUp)
            resize = false;
    }


    private void OnGUI()
    {
        GUILayout.BeginVertical();
        scrollPos = GUILayout.BeginScrollView(scrollPos, GUILayout.Height(currentScrollViewHeight));



        CreateResourcesEditor();


        GUILayout.EndScrollView();

        ResizeScrollView();
        GUILayout.FlexibleSpace();

        GUILayout.Label("Resource Database:", EditorStyles.boldLabel);

        CreateResourcesDisplay();

        GUILayout.EndVertical();
        Repaint();

        Apply();
    }

    private void Apply()
    {
        serializedObject.ApplyModifiedProperties();
        serializedObject.Update();
    }

    private void CreateResourcesEditor()
    {
        GUILayout.Label("Resources Editor:", EditorStyles.boldLabel);
        tab = (tabs)GUILayout.Toolbar((int)tab, toolbarStrings);
        switch (tab)
        {
            case tabs.Characters:
                
                DisplayTabContent(typeof(CharacterTemplate));
                
                
                //if (characterTemplate != null)
                //    if (GUILayout.Button("Add Character to database", GUILayout.ExpandWidth(false)))
                //    {
                //        _resourceDatabaseObject.AddResource(characterTemplate);
                //        _resourceDatabaseObject.RepopulateResourcesListFromDatabase();
                //        EditorUtility.SetDirty(_resourceDatabaseObject);
                //        if (characterTemplate.Name != "")
                //        {
                //            string extension = string.Format("Assets/Resources/{0}.asset", characterTemplate.Name);
                //            Debug.Log("extension: " + extension);
                //            Debug.Log(path);

                //            Debug.Log(AssetDatabase.GenerateUniqueAssetPath(extension));
                //            string AssetRenameErrorMessage = AssetDatabase.RenameAsset(path, AssetDatabase.GenerateUniqueAssetPath(characterTemplate.Name));
                //            if (AssetRenameErrorMessage != string.Empty)
                //            {
                //                Debug.LogError(AssetRenameErrorMessage);
                //            }
                //        }
                //        Repaint();


                //    }
                //Editor characterEditor = Editor.CreateEditor(characterTemplate);
                //if (characterTemplate != null)
                //{
                //    characterEditor.OnInspectorGUI();
                //}
                //if (characterTemplate == null)
                //{
                //    if (GUILayout.Button("New Character", GUILayout.ExpandWidth(false)))
                //    {
                //        characterTemplate = ScriptableObject.CreateInstance(typeof(CharacterTemplate)) as CharacterTemplate;

                //        path = AssetDatabase.GenerateUniqueAssetPath("Assets/Resources/Character.asset");

                //        AssetDatabase.CreateAsset(characterTemplate, path);
                //        AssetDatabase.SaveAssets();
                //        AssetDatabase.Refresh();

                //        CharacterTemplate loadedAsset = (CharacterTemplate)AssetDatabase.LoadAssetAtPath<CharacterTemplate>(path);
                //        //testDatabase.CharacterTemplates.Add(loadedAsset);

                //        newlyCreatedTemplate = EditorGUILayout.ObjectField(newlyCreatedTemplate, typeof(CharacterTemplate), true);
                //        //EditorUtility.FocusProjectWindow();

                //        //Selection.activeObject = example;
                //    }
                //}
                break;
            case tabs.Items:

                DisplayTabContent(typeof(EquipmentItem));

                //if (equipmentItem != null)
                //    if (GUILayout.Button("Add EquipmentItem to database", GUILayout.ExpandWidth(false)))
                //    {
                //        _resourceDatabaseObject.AddResource(equipmentItem);
                //        _resourceDatabaseObject.RepopulateResourcesListFromDatabase();
                //        EditorUtility.SetDirty(_resourceDatabaseObject);
                //        if (equipmentItem.Name != "")
                //        {
                //            string extension = string.Format("Assets/Resources/{0}.asset", equipmentItem.Name);
                //            Debug.Log("extension: " + extension);
                //            Debug.Log(path);

                //            Debug.Log(AssetDatabase.GenerateUniqueAssetPath(extension));
                //            string AssetRenameErrorMessage = AssetDatabase.RenameAsset(path, AssetDatabase.GenerateUniqueAssetPath(equipmentItem.Name));
                //            if (AssetRenameErrorMessage != string.Empty)
                //            {
                //                Debug.LogError(AssetRenameErrorMessage);
                //            }
                //        }
                //        Repaint();
                //    }
                //Editor equipmentEditor = Editor.CreateEditor(equipmentItem);
                //if (equipmentItem != null)
                //{
                //    equipmentEditor.OnInspectorGUI();
                //}
                //if (equipmentItem == null)
                //{
                //    if (GUILayout.Button("New Equipment Item", GUILayout.ExpandWidth(false)))
                //    {
                //        equipmentItem = ScriptableObject.CreateInstance(typeof(EquipmentItem)) as EquipmentItem;

                //        path = AssetDatabase.GenerateUniqueAssetPath("Assets/Resources/EquipmentItem.asset");

                //        AssetDatabase.CreateAsset(equipmentItem, path);
                //        AssetDatabase.SaveAssets();
                //        AssetDatabase.Refresh();

                //        CharacterTemplate loadedAsset = (CharacterTemplate)AssetDatabase.LoadAssetAtPath<CharacterTemplate>(path);
                //        //testDatabase.CharacterTemplates.Add(loadedAsset);

                //        newlyCreatedTemplate = EditorGUILayout.ObjectField(newlyCreatedTemplate, typeof(CharacterTemplate), true);
                //        //EditorUtility.FocusProjectWindow();

                //        //Selection.activeObject = example;
                //    }
                //}
                break;
            case tabs.Dialogue:
                break;
            default:
                break;

        }
    }

    private void DisplayTabContent(System.Type resourceType)
    {
        string typeName = resourceType.Name;

        if (resource == null) // before we've created the resource
        {
            if (GUILayout.Button("New " + typeName, GUILayout.ExpandWidth(false)))
            {
                resource = ResourceFactory.Create(typeName);
                Debug.Log(typeName);
                Debug.Log(resource);
                newlyCreatedResource = EditorGUILayout.ObjectField(newlyCreatedResource, typeof(Resource), true);
            }
        }

        else if (resource != null) // once we have a resource to work with
        {
            if (GUILayout.Button("Add " + typeName + " to database", GUILayout.ExpandWidth(false)))
            {
                _resourceDatabaseObject.AddResource(resource);
                _resourceDatabaseObject.RepopulateResourcesListFromDatabase();
                EditorUtility.SetDirty(_resourceDatabaseObject);
                if (resource.Name != "")
                {
                    Debug.Log(AssetDatabase.GenerateUniqueAssetPath(ResourceFactoryUtilities.GenerateAssetPath(resource, resource.Name)));
                    path = ResourceFactoryUtilities.GetResourceAssetPath(resource);
                    string AssetRenameErrorMessage = AssetDatabase.RenameAsset(path, resource.Name);
                    if (AssetRenameErrorMessage != string.Empty)
                    {
                        Debug.LogError(AssetRenameErrorMessage);
                    }
                }
                Repaint();
            }

            Editor resourceEditor = Editor.CreateEditor(resource);
            resourceEditor.OnInspectorGUI();
        }
    }

    private void CreateResourcesDisplay()
    {
        currentProperty = serializedObject.FindProperty("resources");

        CreateDatabaseDatabaseFilters();

        if (GUILayout.Button("Print Database", GUILayout.ExpandWidth(false)))
        {
            _resourceDatabaseObject.Print();
        }

        if (Event.current.keyCode == KeyCode.Return)
        {
            searchText = searchTextField;
        }

        if (selected == resTypeStrings.Length - 1)
        {
            DrawProperties(currentProperty, true, searchText);
        }
        else
        {
            DrawProperties(currentProperty, true, resTypes[selected], searchText);
        }
    }

    private void CreateDatabaseDatabaseFilters()
    {
        EditorGUILayout.BeginHorizontal();
        selected = EditorGUILayout.Popup(selected, resTypeStrings, GUILayout.ExpandWidth(false));
        searchTextField = GUILayout.TextField(searchTextField);



        if (GUILayout.Button("Search", GUILayout.ExpandWidth(false)))
        {
            searchText = searchTextField;
        }
        EditorGUILayout.EndHorizontal();
    }
}
