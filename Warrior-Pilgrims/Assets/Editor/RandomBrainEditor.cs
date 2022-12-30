using System.Collections;
using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(RandomBrain))]
public class RandomBrainEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        RandomBrain brain = (RandomBrain)target;

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

