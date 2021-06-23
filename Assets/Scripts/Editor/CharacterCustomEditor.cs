using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;

public class AssetHandler
{
    [OnOpenAsset()]
    public static bool OpenEditor(int instanceId, int line) {
        CharacterSO so = EditorUtility.InstanceIDToObject(instanceId) as CharacterSO;
        if (so != null)
        {
            CharacterCustomEditorWindow.Open(so);
            return true;
        }
        return false;
    }
}

[CustomEditor(typeof(CharacterSO))]
public class CharacterCustomEditor : Editor {
    public override void OnInspectorGUI() {
        if(GUILayout.Button("OpenEditor")) {
            CharacterCustomEditorWindow.Open((CharacterSO)target);
        }
    }
}