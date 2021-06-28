using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CharacterCustomEditorWindow : EditorWindow {
    protected SerializedObject targetCharacter;
    protected SerializedProperty Attributes;
    bool showAttributes = true;
    public static void Open(CharacterSO character) {
        CharacterCustomEditorWindow window = GetWindow<CharacterCustomEditorWindow>("Character Editor");
        window.titleContent = new GUIContent("CharacterCustomWindow");
        window.targetCharacter = new SerializedObject(character);
        window.Show();
    }

    private void OnGUI() {
        SerializedProperty Name = targetCharacter.FindProperty("Name");
        GUILayout.Label(Name.stringValue);
        showAttributes = EditorGUILayout.BeginFoldoutHeaderGroup(showAttributes, "Attributes");
        if(showAttributes) {
            Attributes = targetCharacter.FindProperty(propertyPath: "Attributes");

        }
    }
}
