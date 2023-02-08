using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor (typeof (TextureModifier))]
public class TextureModifierEditor : Editor
{
    public override void OnInspectorGUI()
    {
        TextureModifier textureGen = (TextureModifier)target;
        
        if(DrawDefaultInspector())
        {
            if(textureGen.autoUpdate)
            {
                textureGen.ComponentRunner();
            }
        }

        if(GUILayout.Button("Generate"))
        {
            textureGen.ComponentRunner();
        }
    }
}
