using System.Collections;
using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(RandomBrainDeprecated))]
public class RandomBrainEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        RandomBrainDeprecated brain = (RandomBrainDeprecated)target;

        if (GUILayout.Button("Reset Skills"))
        {
            brain.ResetSkills();
        }        
        if (GUILayout.Button("Reset Targets"))
        {
            brain.ResetTargets();
        }
    }
}