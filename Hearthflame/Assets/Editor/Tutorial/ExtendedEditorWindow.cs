using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using GramophoneUtils;

public class ExtendedEditorWindow : EditorWindow
{
    protected SerializedObject serializedObject;
    protected SerializedProperty currentProperty;

    protected void DrawProperties(SerializedProperty prop, bool drawChildren, string searchText)
    {
        string lastPropPath = string.Empty;
        foreach (SerializedProperty p in prop)
        {
            if (p.isArray && p.propertyType == SerializedPropertyType.Generic)
            {
                EditorGUILayout.BeginHorizontal();
                p.isExpanded = EditorGUILayout.Foldout(p.isExpanded, p.displayName);
                EditorGUILayout.EndHorizontal();

                if (p.isExpanded)
                {
                    EditorGUI.indentLevel++;
                    DrawProperties(p, drawChildren, searchText);
                    EditorGUI.indentLevel--;
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(lastPropPath) && p.propertyPath.Contains(lastPropPath)) { continue; }
                {
                    lastPropPath = p.propertyPath;
                }                
                if ((p.objectReferenceValue.name.ToLower()).Contains(searchText.ToLower()))
                {
                    EditorGUILayout.BeginHorizontal();

                    EditorGUILayout.PropertyField(p, drawChildren);

                    DrawRemoveAndDeleteButtons(p);

                    EditorGUILayout.EndHorizontal();
                }
            }
        }
    }

    protected void DrawProperties(SerializedProperty prop, bool drawChildren, System.Type type, string searchText)
    {
        string lastPropPath = string.Empty;
        foreach (SerializedProperty p in prop)
        {
            if (p.isArray && p.propertyType == SerializedPropertyType.Generic)
            {
                EditorGUILayout.BeginHorizontal();
                p.isExpanded = EditorGUILayout.Foldout(p.isExpanded, p.displayName);
                EditorGUILayout.EndHorizontal();

                if (p.isExpanded)
                {
                    EditorGUI.indentLevel++;
                    DrawProperties(p, drawChildren, type, searchText);
                    EditorGUI.indentLevel--;
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(lastPropPath) && p.propertyPath.Contains(lastPropPath)) { continue; }
                {
                    lastPropPath = p.propertyPath;
                }
                
                System.Type foundType = null;
                if (p.propertyType.Equals(UnityEditor.SerializedPropertyType.ObjectReference) && p.objectReferenceValue != null)
                {
                    foundType = p.objectReferenceValue.GetType();
                }    
                if (type == foundType && foundType != null)
                {
                    if ((p.objectReferenceValue.name.ToLower()).Contains(searchText.ToLower()))
                    {
                        EditorGUILayout.BeginHorizontal();

                        EditorGUILayout.PropertyField(p, drawChildren);

                        DrawRemoveAndDeleteButtons(p);

                        EditorGUILayout.EndHorizontal();
                    }
                }
            }
        }
    }


    private void DrawRemoveAndDeleteButtons(SerializedProperty p)
    {
        if ((ResourceDatabase)serializedObject.targetObject)
        {
            ResourceDatabase resourceDatabase = (ResourceDatabase)serializedObject.targetObject;
            Resource resource = (Resource)p.objectReferenceValue;

            if (EditorGUILayout.LinkButton("Delete Resource"))
            {
                if (EditorUtility.DisplayDialog("Confirm Delete", "Are you sure you wish to delete?", "Confirm", "Cancel"))
                {
                    if (resourceDatabase.Contains(resource))
                    {
                        resourceDatabase.RemoveResource(resource);
                    }
                    ResourceFactoryUtilities.DeleteResourceAsset(resource);
                }
            }

            if (EditorGUILayout.LinkButton("Remove Resource"))
            {
                if (EditorUtility.DisplayDialog("Confirm Delete", "Are you sure you wish to delete?", "Confirm", "Cancel"))
                {
                    if (resourceDatabase.Contains(resource))
                    {
                        resourceDatabase.RemoveResource(resource);
                    }
                }
            }
        }
    }
}
