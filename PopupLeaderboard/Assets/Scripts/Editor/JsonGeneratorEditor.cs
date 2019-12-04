using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(JsonGenerator))]
public class JsonGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        JsonGenerator generator = (JsonGenerator)target;

        var isPressedButton = GUILayout.Button("Generate Json");

        if (isPressedButton)
        {
            generator.GenerateJson();
        }
    }
}
