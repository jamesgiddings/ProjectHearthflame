//using UnityEngine;
//using System.Collections;
//using UnityEditor;
//using GramophoneUtils.Items.Containers;
//using UnityEngine.UIElements;
//using UnityEditor.UIElements;

//// Creates a custom Label on the inspector for all the scripts named ScriptName
//// Make sure you have a ScriptName script in your
//// project, else this will not work.
//[CustomEditor(typeof(EquipmentSlot))]
//public class EquipmentSlotEditor : Editor
//{
//	public override void OnInspectorGUI()
//	{
//		GUILayout.Label("This is a Label in a Custom Editor");
//	}
//}



//// IngredientDrawerUIE
//[CustomPropertyDrawer(typeof(EquipmentSlot))]
//public class IngredientDrawerUIE : PropertyDrawer
//{
//    public override VisualElement CreatePropertyGUI(SerializedProperty property)
//    {
//        // Create property container element.
//        var container = new VisualElement();

//        // Create property fields.
//        var amountField = new PropertyField(property.FindPropertyRelative("amount"));
//        var unitField = new PropertyField(property.FindPropertyRelative("unit"));
//        var nameField = new PropertyField(property.FindPropertyRelative("name"), "Fancy Name");

//        // Add fields to the container.
//        container.Add(amountField);
//        container.Add(unitField);
//        container.Add(nameField);

//        return container;
//    }
//}
