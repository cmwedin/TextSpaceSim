using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;

[CustomEditor(typeof(CharacterSO))]
public class CharacterCustomEditor : Editor {
    protected SerializedProperty Attributes;
    protected SerializedProperty Name;
    bool showAttributes = true;
    private void OnEnable() {
        Name = serializedObject.FindProperty("_name");
        Attributes = serializedObject.FindProperty("Attributes");
    }
    public override void OnInspectorGUI() {
        /*if(GUILayout.Button("Open Editor Window")) {
            CharacterCustomEditorWindow.Open((CharacterSO)target);
        }*/
        //DrawDefaultInspector();
        serializedObject.Update();
        GUILayout.Label(Name.stringValue ?? "Error Name Null");
        showAttributes = EditorGUILayout.BeginFoldoutHeaderGroup(showAttributes, "Attributes");
        if(showAttributes) {
            int attribMin = 1;
            int attribMax = 10;
            foreach (SerializedProperty item in Attributes) {
                string path = item.propertyPath;
                SerializedProperty attribName = serializedObject.FindProperty($"{path}._name");
                SerializedProperty attribValue = serializedObject.FindProperty($"{path}._value");
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label(attribName.stringValue);
                attribValue.intValue = EditorGUILayout.IntSlider(attribValue.intValue, attribMin, attribMax);
                EditorGUILayout.EndHorizontal();
            }
            
        }
    }
    
}
/*public class AssetHandler
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
}*/