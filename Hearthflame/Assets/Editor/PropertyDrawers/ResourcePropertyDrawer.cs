using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(Resource))]
public class ResourcePropertyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {

		//base.OnGUI(position, property, label);
		EditorGUI.BeginProperty(position, label, property);
		position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
		GUI.backgroundColor = Color.cyan;
		EditorGUI.PropertyField(position, property, GUIContent.none);
		EditorGUI.EndProperty();
		//Debug.Log(fieldInfo.FieldType);
    }
}
