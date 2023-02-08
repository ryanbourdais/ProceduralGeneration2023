using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor (typeof (NoiseGenerator))]
public class PerlinGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        NoiseGenerator terrainGen = (NoiseGenerator)target;
        
        if(DrawDefaultInspector())
        {
            if(terrainGen.autoUpdate)
            {
                terrainGen.GenerateTerrain();
            }
        }

        if(GUILayout.Button("Generate"))
        {
            terrainGen.GenerateTerrain();
        }
    }
}
