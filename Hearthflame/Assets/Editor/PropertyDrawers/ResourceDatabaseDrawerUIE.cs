using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;


[CustomPropertyDrawer(typeof(ResourceDatabase))]
public class ResourceDatabaseDrawerUIE : PropertyDrawer
{
	public override VisualElement CreatePropertyGUI(SerializedProperty property)
	{
		// Create property container element.
		var container = new VisualElement();

		// Create property fields.
		var resourcesField = new PropertyField(property.FindPropertyRelative("resources"));
		//var unitField = new PropertyField(property.FindPropertyRelative("unit"));
		//var nameField = new PropertyField(property.FindPropertyRelative("name"), "Fancy Name");

		// Add fields to the container.
		container.Add(resourcesField);
		//container.Add(unitField);
		//container.Add(nameField);

		return container;
	}

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
		//Debug.Log(fieldInfo.FieldType);
		//base.OnGUI(position, property, label);
		EditorGUI.BeginProperty(position, label, property);
		position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
		GUI.backgroundColor = Color.cyan;
		EditorGUI.PropertyField(position, property, GUIContent.none);
		EditorGUI.EndProperty();
	}
}